/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 9/20/2025
  * Date Last Updated: 12/10/2025
  * Summary: Handles player movement and associated checks, max jump height that can be comfortably reached is a platform at y = 4, also handles gravity and calls PlayerSquashStretch.cs, 
   *as well as returning variables for the animation control states and triggering sound effects

  * Update: Updated adding colors changing on the particle effects of orbs to get darker less jumps remaining and changed to use PlayerDeathHandler
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
    private bool isMovementLocked = false;


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
    private bool hasLeftGround = false;         // tracks whether we actually left the ground

    [Header("Wall Jump")]
    public float wallJumpUpForce = 6f;          // Upward push
    public float wallJumpHorizontalForce = 5f;  // Side push away from wall
    public int maxWallJumps = 15;                // Number of wall jumps before landing
    public float wallCheckDistance = 0.6f;      // How close to wall
    public LayerMask wallMask;                  // Which layers are walls
    public float wallStickGravityScale = 0.3f;  // How much gravity applies while sticking
    public float wallStickDuration = 2f;        //how long reduced gravity applies

    private int remainingWallJumps;
    private bool isTouchingWall;
    private Vector3 lastWallNormal;             
    private float wallClingTimer = 0f;          //track cling duration


    [Header("Dash")]
    public float dashForce = 35f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;
    public GameObject[] dashIndicators;

    private MeshRenderer[] skinnedMeshRenderers;

    [Header("References")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public Transform cameraTransform;
    public ParticleSystem[] particleSystems;
    public PlayerDeathHandler deathHandler;
    private Color originalColor;

    

    [Header("Custom Gravity")]
    public float baseGravity = 15f;          // Default gravity strength
    public float fallGravityMultiplier = 2f; // Gravity multiplier when falling
    public float lowJumpMultiplier = 1.5f;   // Gravity multiplier for early jump release
    public float peakGravityDelay = 0.05f;   // Short delay at jump peak
    private bool peakGravityApplied = false; // track if peak gravity coroutine started

    [Header("Particle Systems")]
    public ParticleSystem dustParticles; // Assign in Inspector
    public ParticleSystem landingParticles;


    private Rigidbody rb;
    private Vector2 movementValue;
    private bool isGrounded;
    private bool wasGrounded = true;

    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 dashDir;
    private float dashDragTimer = 0f;
    private float jumpStartTime = 0f;
    private const float liftOffGraceTime = 0.2f; // seconds allowed to leave ground before counts as a jump
    private int jumpHoldFrameCount = 0;
    private const int maxJumpHoldFrames = 23;

    private PlayerSquashStretch squashStretch;

    private Animator animator;

    private float fallThreshold = -10f;

    [Header("Dash Effect")]
    public Material dashGhostMaterial;
    public float ghostDuration;
    Transform modelVisual;

    [Header("Sound Effects")]
    public AudioSource DashSFX;
    public AudioSource JumpSFX;
    public AudioSource SheepBaa;
    public AudioSource LandSFX;
    public AudioSource DashReload;
    public float chanceToPlay = 0.1f;



    private void Awake()
    {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false; // disable built-in gravity

        //Gets the squash and stretch component to be used
        squashStretch = GetComponent<PlayerSquashStretch>();

        modelVisual = transform.Find("Feet Ground idea");

        //makes sure that the dash indicators (horns) are properly instantiated at the start 
        if (dashIndicators != null)
        {
            foreach (var indicator in dashIndicators)
                if (indicator != null) indicator.SetActive(false);
        }

        //Gets the animator component
        animator = GetComponentInChildren<Animator>();

        
        ColorUtility.TryParseHtmlString("#FFFF00", out originalColor);

    }

    public void OnMove(InputValue value)
    {
        if (isMovementLocked) return;

        movementValue = value.Get<Vector2>();
    }


    public void OnJump(InputValue value)
    {

        if (isMovementLocked) return;

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
            JumpSFX.Play();
            TryPlayBaa();

            Debug.Log("Jump Started (ground)");
        }
        // --- Wall jump ---
        else if (!isGrounded && isTouchingWall && remainingWallJumps > 0)
        {
            
            //Right here is where I've gotta insert the color alteration stuff
            UpdateColors(remainingWallJumps);
            DoWallJump();
            JumpSFX.Play();
            TryPlayBaa();

            squashStretch?.StretchVertical();
        }

        if (!jumpHeld && jumpStarted)
        {
            currentHoldForce = 0f;
            Debug.Log("Jump Released");
        }
    }

    //Updates the colors of surrounding particles
    void UpdateColors(int jumps)
    {
        jumps = Mathf.Clamp(jumps, 0, 15);

        float t = 1f - (jumps / 15f);  // normalize 0–15 to 1–0

        Color newColor = Color.Lerp(originalColor, Color.black, t);

        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.startColor = newColor;
        }
    }



    public void OnDash(InputValue value)
    {
        if (isMovementLocked) return;

        if (value.isPressed && canDash)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

            Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;
            if (moveDir.sqrMagnitude < 0.01f) moveDir = transform.forward;

            // Flatten dashDir so it always ignores slope steepness
            dashDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;
            DashSFX.Play();
            TryPlayBaa();

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

    void Update()
    {
        //For killing via falling
        if (transform.position.y < fallThreshold)
        {

            if (deathHandler != null)
            {
                deathHandler.RunDeathSequence();
            }
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        CheckForWall();

        float horizontalSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
        animator.SetFloat("Velocity", horizontalSpeed);

        // Reset wall jumps on landing
        if (isGrounded)
        {
            remainingWallJumps = maxWallJumps;
            UpdateColors(15);
            wallClingTimer = 0f;

            if (!dustParticles.isPlaying)
            {
                dustParticles.Play();
                Debug.Log("Start particles");
            }
        }
        else
        {
            if (dustParticles.isPlaying)
            {
                dustParticles.Stop();
                Debug.Log("Stop particles");
            }
            animator.SetBool("In Air", true);
        }

        if (!isTouchingWall) wallClingTimer = 0f;

        // --- LANDING EFFECT ---
        if (isGrounded && !wasGrounded)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("In Air", false);
            squashStretch?.SquashVertical(); // compress on landing
            StartCoroutine(ResetSquashAfterFrames(10));

            // --- Landing Particles ---
            if (landingParticles != null)
            {
                landingParticles.transform.position = groundCheck.position; // place at feet
                landingParticles.Play();
                LandSFX.Play();
            }

        }
        // --- DASH EFFECT ---
        if (isDashing)
        {
            animator.SetBool("Dashing", true);
            squashStretch?.SquashVertical(); // squash horizontally while dashing
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
        //Matches the players movement to the direction they are facing with the camera
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
                // Air movement
                if (moveDir.sqrMagnitude > 0.01f)
                {
                    // Move while in air (normal control)
                    Vector3 desiredVel = moveDir * (maxSpeed * 0.8f);
                    Vector3 accel = (desiredVel - horizontalVel) * (airControl * Time.fixedDeltaTime);
                    rb.AddForce(accel, ForceMode.VelocityChange);
                }
                else
                {
                    // Slow down quickly when no input midair ---
                    float airStopDamping = 5f;
                    Vector3 slowVel = Vector3.Lerp(horizontalVel, Vector3.zero, airStopDamping * Time.fixedDeltaTime);
                    rb.velocity = new Vector3(slowVel.x, rb.velocity.y, slowVel.z);
                }
            }
        }
    }




    
    private void HandleRotation()
    {
        //Rotate the player to match the direction being faced by the camera
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
            animator.SetBool("Jumping", true);
            animator.SetBool("In Air", true);
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
            animator.SetBool("Jumping", false);
            animator.SetBool("In Air", false);
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

        //Push away and up at these amounts
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





    //System for controlling speed through increased drag on the rigidbody
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

    //For controlling the horns, disapear when dashing and reappear when dash is ready to be used
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

        // visually stretch during dash
        squashStretch?.SquashVertical();

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.AddForce(dashDir * dashForce, ForceMode.Force);
            timer += Time.fixedDeltaTime;
            if (skinnedMeshRenderers == null)
            {
                

                skinnedMeshRenderers = modelVisual.GetComponentsInChildren<MeshRenderer>();
            }

            Debug.Log(skinnedMeshRenderers);

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                MeshFilter meshFilter = skinnedMeshRenderers[i].GetComponent<MeshFilter>();
                if (meshFilter == null || meshFilter.sharedMesh == null)
                    continue; // skip if there's no mesh to copy

                GameObject Trail = new GameObject("DashTrail");
                MeshRenderer mr = Trail.AddComponent<MeshRenderer>();
                MeshFilter mf = Trail.AddComponent<MeshFilter>();

                // Copy mesh and material from the original model
                mf.mesh = meshFilter.sharedMesh;
                mr.material = dashGhostMaterial;

                // Match transform so the ghost mesh spawns in the same place
                Trail.transform.position = skinnedMeshRenderers[i].transform.position;
                Trail.transform.rotation = skinnedMeshRenderers[i].transform.rotation;
                Trail.transform.localScale = skinnedMeshRenderers[i].transform.lossyScale;

                // Auto-destroy after a short delay
                //Destroy(Trail, 0.4f);
                Trail.AddComponent<FadeOut>().duration = ghostDuration;
            }

            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
        animator.SetBool("Dashing", false);
        StartCoroutine(ResetStretchAfterDelay(0.1f));
    }

    //Breifly waits to reset the stretch until after the dash has ended its inital momentum
    private IEnumerator ResetStretchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        squashStretch?.ResetScale();
    }


    //Keeps track of the time between potential dash uses
    private IEnumerator DashCooldownRoutine()
    {
        canDash = false;
        if (dashIndicators != null)
            foreach (var indicator in dashIndicators)
                if (indicator != null) indicator.SetActive(false);

        yield return new WaitForSeconds(dashCooldown);
        DashReload.Play();
        canDash = true;
    }

    //For situations where the player can't move, locks out movement for a specified duration
    public void MovementLockOut(float duration)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(MovementLockOutRoutine(duration));
    }


    private IEnumerator MovementLockOutRoutine(float duration)
    {
        isMovementLocked = true;
        movementValue = Vector2.zero; // clear any current input
        jumpHeld = false;
        yield return new WaitForSeconds(duration);
        isMovementLocked = false;
    }

    public void TryPlayBaa()
    {
        if (Random.value < chanceToPlay)
        {
            SheepBaa.Play();
        }
    }

} 