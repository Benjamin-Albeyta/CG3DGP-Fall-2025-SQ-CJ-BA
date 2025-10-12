/**
  * Author: Benjamin Albeyta
  * Date Created: 9/20/2025
  * Date Last Updated: 10/11/2025
  * Summary: Handles player movement and associated checks
  */


using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 3500f;
    public float rotateSpeed = 5f;
    public float maxSpeed = 10f;
    public float airControl = 5f;

    [Header("Drag Settings")]
    public float baseGroundDrag = 0.0001f;
    public float maxGroundDrag = 5f;
    public float airDrag = 0f;
    public float dashDrag = 0.1f;
    public float dashDragDuration = 1.5f;

    [Header("Jump")]
    public float holdForce = 350f;
    public float holdJumpDecay = 0.5f;

    private bool jumpHeld = false;
    private bool jumpStarted = false;
    private float currentHoldForce;
    public float initalJumpForce = 10f;
    private bool hasLeftGround = false; // NEW - tracks whether we actually left the ground

    [Header("Wall Jump")]
    public float wallJumpUpForce = 6f;          // Upward push
    public float wallJumpHorizontalForce = 5f;  // Side push away from wall
    public int maxWallJumps = 2;                // Number of wall jumps before landing
    public float wallCheckDistance = 0.6f;      // How close to wall
    public LayerMask wallMask;                  // Which layers are walls
    public float wallStickGravityScale = 0.3f;  // How much gravity applies while sticking
    public float wallStickDuration = 2f; // NEW - how long reduced gravity applies

    private int remainingWallJumps;
    private bool isTouchingWall;
    private Vector3 lastWallNormal;
    private float wallClingTimer = 0f; // NEW - track cling duration


    [Header("Dash")]
    public float dashForce = 35f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;
    public GameObject[] dashIndicators;

    [Header("References")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 movementValue;
    private bool isGrounded;
    private bool wasGrounded = true;

    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 dashDir;
    private float dashDragTimer = 0f;
    private float jumpStartTime = 0f;
    private const float liftOffGraceTime = 0.2f; // seconds allowed to leave ground

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (dashIndicators != null)
        {
            foreach (var indicator in dashIndicators)
                if (indicator != null) indicator.SetActive(false);
        }
    }

    public bool IsJumpHeld() => jumpHeld;

    public void OnMove(InputValue value) => movementValue = value.Get<Vector2>();

    public void OnJump(InputValue value)
    {
        jumpHeld = value.isPressed;
        // --- Normal ground jump ---
        if (isGrounded && !jumpStarted)
        {
            jumpStarted = true;
            hasLeftGround = false;
            jumpStartTime = Time.time;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * initalJumpForce, ForceMode.Impulse);
            currentHoldForce = holdForce;

            Debug.Log("Jump Started (ground)");
        }
            // --- Wall jump ---
        else if (!isGrounded && isTouchingWall && remainingWallJumps > 0)
        {
            DoWallJump();
        }
        

if (!jumpHeld && jumpStarted)
        {
            currentHoldForce = 0f;
            Debug.Log("Jump Released");
        }
    }


    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

            Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;
            if (moveDir.sqrMagnitude < 0.01f) moveDir = transform.forward;

            //dashDir = moveDir.normalized;
            // Flatten dashDir so it always ignores slope steepness
            dashDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;

            StartCoroutine(DashRoutine());
            StartCoroutine(DashCooldownRoutine());
        }
    }

    private float GetSlopeAngle()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundDistance + 1f, groundMask))
        {
            return Vector3.Angle(hit.normal, Vector3.up);
        }
    return 0f;
    }


    private Vector3 GetSlopeAdjustedDirection(Vector3 moveDir)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundDistance + 0.5f, groundMask))
        {
            return Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;
        }
        return moveDir;
    }


    private void FixedUpdate()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        CheckForWall();
        // Reset wall jumps on landing
        if (isGrounded)
        {
            remainingWallJumps = maxWallJumps;
            wallClingTimer = 0f; // NEW - reset cling timer when grounded
        }

        if (isTouchingWall && !isGrounded && rb.velocity.y < 0f)
        {
            wallClingTimer += Time.fixedDeltaTime;

            if (wallClingTimer < wallStickDuration)
            {
                // Reduced falling for limited time
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * wallStickGravityScale, rb.velocity.z);
            }
            // After timer expires, normal gravity applies
        }
        else
        {
            // Reset cling timer if not touching wall
            if (!isTouchingWall) wallClingTimer = 0f;
        }


        // For allowing more control
        if (isGrounded && rb.velocity.y < 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }

        if (isGrounded)
        {
            if (rb.velocity.y < -2f)
                rb.velocity = new Vector3(rb.velocity.x, -2f, rb.velocity.z);

            // Extra stick-to-ground force
            rb.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
        }



        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleDrag();
        HandleDashIndicators();

        wasGrounded = isGrounded;
    }

    private void HandleMovement()
    {
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;

        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        if (!isDashing && moveDir.sqrMagnitude > 0.01f)
        {
            if (isGrounded)
            {
                // --- NEW slope adjustment ---
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundDistance + 1f, groundMask))
                {
                    Vector3 slopeDir = Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;
                    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                    // Move along slope
                    rb.AddForce(slopeDir * moveForce * Time.fixedDeltaTime, ForceMode.Force);

                    // --- NEW downhill speed boost ---
                    if (slopeAngle > 2f)
                    {
                        float downhillBoost = Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
                        rb.AddForce(slopeDir * downhillBoost * moveForce * 0.3f * Time.fixedDeltaTime, ForceMode.Force);
                    }

                    // --- NEW stick to ground ---
                    if (rb.velocity.y <= 0f)
                    {
                        rb.AddForce(-hit.normal * 20f, ForceMode.Acceleration);
                    }
                }
            }
            else
            {
                // Air control
                Vector3 horizontalVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                if (horizontalVel.magnitude > 0.1f)
                {
                    Vector3 desiredDir = moveDir.normalized;
                    Vector3 newVel = Vector3.RotateTowards(
                        horizontalVel,
                        desiredDir * horizontalVel.magnitude,
                        airControl * Time.fixedDeltaTime,
                        0f
                        );
                    rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
                }
            }
        }
    } 

    
    private void HandleRotation()
    {
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;

        if (cameraTransform != null && moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }
    }


    private void HandleJump()
    {
        // Confirm lift-off (once actually not grounded)
        if (jumpStarted && !hasLeftGround && !isGrounded)
        {
            hasLeftGround = true;
            Debug.Log("Lift-off confirmed");
        }

        // If we never left ground within grace time, cancel jump
        if (jumpStarted && !hasLeftGround && isGrounded && Time.time - jumpStartTime > liftOffGraceTime)
        {
            jumpStarted = false;
            currentHoldForce = 0f;
            Debug.Log("Jump canceled — never left ground (timeout)");
        }

        // Reset jump when landing
        if (isGrounded && !wasGrounded)
        {
            jumpStarted = false;
            hasLeftGround = false;
            currentHoldForce = 0f;
            Debug.Log("Landed, jump reset");
        }

        // Apply hold force only if truly airborne
        if (jumpHeld && hasLeftGround && jumpStarted && currentHoldForce > 40f && !isGrounded)
        {
            rb.AddForce(Vector3.up * currentHoldForce, ForceMode.Force);
            currentHoldForce *= holdJumpDecay;
        }
    }
    
    private void CheckForWall()
    {
        isTouchingWall = false;
        lastWallNormal = Vector3.zero;

        // Check forward, right, and left directions
        Vector3[] directions =
        {
            transform.forward,
            transform.right,
            -transform.right
        };

        foreach (var dir in directions)
        {
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, wallCheckDistance, wallMask))
            {
                isTouchingWall = true;
                lastWallNormal += hit.normal; // accumulate normals (for corners)
                Debug.DrawRay(transform.position, dir * wallCheckDistance, Color.yellow);
            }
        }

        if (isTouchingWall)
        {
            lastWallNormal.Normalize(); // average direction
        }
    }

    private void DoWallJump()
    {
        remainingWallJumps--;

        // Reset velocity for consistent launch behavior
        rb.velocity = Vector3.zero;

        // Jump direction = Up + away from wall
        Vector3 upComponent = Vector3.up * 0.7f;             // 70% upward
        Vector3 awayComponent = lastWallNormal * 0.3f;       // 30% push away
        Vector3 jumpDir = (upComponent + awayComponent).normalized;

        // Apply wall jump force (only this controls strength)
        rb.AddForce(jumpDir * wallJumpUpForce, ForceMode.Impulse);

        // Optional: rotate player slightly away from wall
        if (lastWallNormal != Vector3.zero)
        {
            Quaternion awayRotation = Quaternion.LookRotation(-lastWallNormal, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, awayRotation, 0.4f);
        }

        Debug.DrawRay(transform.position, jumpDir * 2f, Color.cyan, 1.0f);
        Debug.Log($"Wall Jump! Direction: {jumpDir}, Remaining: {remainingWallJumps}");
        wallClingTimer = 0f; // NEW - reset cling timer when wall jumping
    }





    private void HandleDrag()
    {
        if (isDashing || dashDragTimer > 0f)
        {
            rb.drag = dashDrag;
            if (!isDashing) dashDragTimer -= Time.fixedDeltaTime;
            return;
        }

        if (isGrounded)
        {

            float horizontalSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
            float t = Mathf.Clamp01(horizontalSpeed / maxSpeed);
            float expT = Mathf.Pow(t, 8f); // increase exponent to make drag rise more steeply

            rb.drag = Mathf.Lerp(baseGroundDrag, maxGroundDrag, expT);
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void HandleDashIndicators()
    {
        if (dashIndicators != null)
        {
            foreach (var indicator in dashIndicators)
                if (indicator != null) indicator.SetActive(canDash);
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        dashDragTimer = dashDragDuration;

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.AddForce(dashDir * dashForce, ForceMode.Force);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
    }

    private IEnumerator DashCooldownRoutine()
    {
        canDash = false;
        if (dashIndicators != null)
            foreach (var indicator in dashIndicators)
                if (indicator != null) indicator.SetActive(false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
} 