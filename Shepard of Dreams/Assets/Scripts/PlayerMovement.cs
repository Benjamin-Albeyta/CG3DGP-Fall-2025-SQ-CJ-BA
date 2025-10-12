/**
  * Author: Benjamin Albeyta
  * Date Created: 9/20/2025
  * Date Last Updated: 10/12/2025
  * Summary: Handles player movement and associated checks, max jump height that can be comfortably reached is a platform at y = 4
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

    [Header("Custom Gravity")]
    public float baseGravity = 15f;          // Default gravity strength
    public float fallGravityMultiplier = 2f; // Gravity multiplier when falling
    public float lowJumpMultiplier = 1.5f;   // Gravity multiplier for early jump release
    public float peakGravityDelay = 0.05f;   // Short delay at jump peak
    private bool peakGravityApplied = false; // track if peak gravity coroutine started


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
    private int jumpHoldFrameCount = 0;
    private const int maxJumpHoldFrames = 23;

    private PlayerSquashStretch squashStretch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false; // disable built-in gravity

        squashStretch = GetComponent<PlayerSquashStretch>(); // optional

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

            //squashStretch?.StretchVertical(); // elongate upward

            Debug.Log("Jump Started (ground)");
        }
        // --- Wall jump ---
        else if (!isGrounded && isTouchingWall && remainingWallJumps > 0)
        {
            DoWallJump();
            squashStretch?.StretchVertical(); // optional, adds visual pop on wall jump
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


    private void ApplyGravity()
    {
        if (isGrounded || isDashing)
        {
            peakGravityApplied = true;
            return; // skip gravity on ground or during dash
        }

        float verticalVel = rb.velocity.y;

        if (isTouchingWall && !isGrounded && rb.velocity.y < 0f)
        {

            wallClingTimer += Time.fixedDeltaTime;

            if (wallClingTimer < wallStickDuration)
            {
                rb.AddForce(Vector3.down * baseGravity * wallStickGravityScale, ForceMode.Acceleration);
            } else
            {
                StartCoroutine(ApplyFallGravityAfterDelay());
            }
        }



        if (verticalVel > 0.1f) // rising
        {
            if (!jumpHeld)
            {
                // Jump released early → pull down faster
                rb.AddForce(Vector3.down * baseGravity * lowJumpMultiplier, ForceMode.Acceleration);
            }
            else
            {
                // Floaty upward
                rb.AddForce(Vector3.down * baseGravity * 0.5f, ForceMode.Acceleration);
            }

            peakGravityApplied = false; // reset for peak detection
        }
        else if (verticalVel <= 0.1f && !peakGravityApplied) // near or past peak
        {
            peakGravityApplied = true;
            StartCoroutine(ApplyFallGravityAfterDelay());
        }
        else if (verticalVel < -0.1f) // falling after peak
        {
            rb.AddForce(Vector3.down * baseGravity * fallGravityMultiplier, ForceMode.Acceleration);
        }
        else
        {
            StartCoroutine(ApplyFallGravityAfterDelay());
        }
    }
        

    private IEnumerator ApplyFallGravityAfterDelay()
    {
        yield return new WaitForSeconds(peakGravityDelay);
        rb.AddForce(Vector3.down * baseGravity * fallGravityMultiplier, ForceMode.Acceleration);
    }



    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        CheckForWall();

        // Reset wall jumps on landing
        if (isGrounded)
        {
            remainingWallJumps = maxWallJumps;
            wallClingTimer = 0f;
        }

        if (!isTouchingWall) wallClingTimer = 0f;

        // --- LANDING EFFECT ---
        if (isGrounded && !wasGrounded)
        {
            squashStretch?.SquashVertical(); // compress on landing
            StartCoroutine(ResetSquashAfterFrames(10));
        }

        // --- WALL CONTACT EFFECT ---
        if (isTouchingWall && !isGrounded && rb.velocity.y <= 0f)
        {
            squashStretch?.SquashHorizontal(); // squish sideways when clinging to wall
        }

        // --- DASH EFFECT ---
        if (isDashing)
        {
            squashStretch?.StretchHorizontal(); // stretch horizontally while dashing
        }

        // --- Track jump hold ---
        if (jumpHeld)
        {
            jumpHoldFrameCount++;

            if (jumpHoldFrameCount >= maxJumpHoldFrames)
            {
                jumpHeld = false;
                jumpHoldFrameCount = 0;
                Debug.Log("Jump auto-released after 23 frames");
            }
        }
        else
        {
            jumpHoldFrameCount = 0;
        }

        HandleJump();
        ApplyGravity();
        HandleMovement();
        HandleRotation();
        HandleDrag();
        HandleDashIndicators();

        wasGrounded = isGrounded;

        // When idle or airborne
        if (!isGrounded && !jumpHeld)
        {
            squashStretch?.ResetScale();
        }

    }

    private IEnumerator ResetSquashAfterFrames(int frameCount)
    {   
        for (int i = 0; i < frameCount; i++)
            yield return new WaitForFixedUpdate(); // physics frame

        squashStretch?.ResetScale();
    }



        


    private void HandleMovement()
    {
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;

        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (!isDashing)
        {
            if (isGrounded)
            {
                // Grounded movement
                if (moveDir.sqrMagnitude > 0.01f)
                    rb.AddForce(moveDir * moveForce * Time.fixedDeltaTime, ForceMode.Force);
            }
            else
            {
                // --- AIR MOVEMENT ---
                if (moveDir.sqrMagnitude > 0.01f)
                {
                    // Move while in air (normal control)
                    Vector3 desiredVel = moveDir * (maxSpeed * 0.8f);
                    Vector3 accel = (desiredVel - horizontalVel) * (airControl * Time.fixedDeltaTime);
                    rb.AddForce(accel, ForceMode.VelocityChange);
                }
                else
                {
                    // --- NEW: Slow down quickly when no input midair ---
                    float airStopDamping = 5f; // tweak between 3–8 for feel
                    Vector3 slowVel = Vector3.Lerp(horizontalVel, Vector3.zero, airStopDamping * Time.fixedDeltaTime);
                    rb.velocity = new Vector3(slowVel.x, rb.velocity.y, slowVel.z);
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
            squashStretch?.StretchVertical(); // elongate upward
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
            //Checks for a wall in the given direction
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

        rb.velocity = Vector3.zero;

        Vector3 upComponent = Vector3.up * 0.7f;
        Vector3 awayComponent = lastWallNormal * 0.3f;
        Vector3 jumpDir = (upComponent + awayComponent).normalized;

        rb.AddForce(jumpDir * wallJumpUpForce, ForceMode.Impulse);

        squashStretch?.StretchVertical(); // elongate on wall jump for visual feedback

        if (lastWallNormal != Vector3.zero)
        {
            Quaternion awayRotation = Quaternion.LookRotation(-lastWallNormal, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, awayRotation, 0.4f);
        }

        Debug.DrawRay(transform.position, jumpDir * 2f, Color.cyan, 1.0f);
        Debug.Log($"Wall Jump! Direction: {jumpDir}, Remaining: {remainingWallJumps}");
        wallClingTimer = 0f;
    }






    private void HandleDrag()
    {
        if (isDashing || dashDragTimer > 0f)
        {
            rb.drag = dashDrag;
            if (!isDashing) dashDragTimer -= Time.fixedDeltaTime;
            return;
        }

        bool noInput = movementValue.sqrMagnitude < 0.01f;

        if (isGrounded)
        {
            if (noInput)
            {
                rb.drag = maxGroundDrag;
            }
            else
            {
                float horizontalSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
                float t = Mathf.Clamp01(horizontalSpeed / maxSpeed);
                float expT = Mathf.Pow(t, 6f); // increase exponent to make drag rise more steeply

                rb.drag = Mathf.Lerp(baseGroundDrag, maxGroundDrag, expT);
            }
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

        squashStretch?.StretchHorizontal(); // visually stretch during dash
        //squashStretch?.SquashVertical();

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.AddForce(dashDir * dashForce, ForceMode.Force);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
        StartCoroutine(ResetStretchAfterDelay(0.1f));
    }

    private IEnumerator ResetStretchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        squashStretch?.ResetScale();
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