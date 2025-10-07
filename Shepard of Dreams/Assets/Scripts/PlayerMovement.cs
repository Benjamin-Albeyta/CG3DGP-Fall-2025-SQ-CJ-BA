/*using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float maxSpeed = 10f;
    public float groundFriction = 5f;
    public float airControlMultiplier = 0.5f;
    public float rotateSpeed = 10f; // smooth facing rotation

    [Header("Jump")]
    public float jumpSpeed = 8f;

    [Header("Dash")]
    public float dashSpeed = 25f;
    public float dashAccelRate = 60f;
    public float dashCooldown = 1f;
    public float dashReturnRate = 5f;
    public GameObject[] dashIndicators; // assign 3D models in prefab

    [Header("References")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public Transform cameraTransform; // assign your third-person camera here
    private Rigidbody rb;
    private Vector2 movementValue;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;

    private Vector3 dashDir;
    private bool reachedDashSpeed = false;

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

    public void OnMove(InputValue value)
    {
        Debug.Log("Move " + value.Get<Vector2>());
        movementValue = value.Get<Vector2>() * speed;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

            Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;
            if (moveDir.sqrMagnitude < 0.01f)
                moveDir = transform.forward;

            dashDir = moveDir.normalized;
            isDashing = true;
            reachedDashSpeed = false;

            StartCoroutine(DashCooldownRoutine());
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Camera-relative movement
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;
        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (isGrounded)
        {
            rb.AddForce(moveDir * Time.fixedDeltaTime, ForceMode.Force);

            if (moveDir.sqrMagnitude < 0.01f)
                rb.AddForce(-horizontalVel * groundFriction * Time.fixedDeltaTime, ForceMode.VelocityChange);

        }
        else
        {
            rb.AddForce(moveDir * airControlMultiplier * Time.fixedDeltaTime, ForceMode.Force);
        }

        if (cameraTransform != null)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            if (cameraForward.sqrMagnitude > 0.01f)
                {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
            }
        }

        // Dash logic
        HandleDash(horizontalVel);

        // Show dash indicators
        if (dashIndicators != null && canDash)
        {
            foreach (var indicator in dashIndicators)
                if (indicator != null)
                    indicator.SetActive(true);
        }
    }

    private void HandleDash(Vector3 horizontalVel)
    {
        if (isDashing)
        {
            Vector3 dashVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (!reachedDashSpeed)
            {
                Vector3 targetVel = dashDir * dashSpeed;
                Vector3 newVel = Vector3.MoveTowards(dashVel, targetVel, dashAccelRate * Time.fixedDeltaTime);
                rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);

                if (newVel.magnitude >= dashSpeed * 0.95f)
                    reachedDashSpeed = true;
            }
            else
            {
                if (dashVel.magnitude > maxSpeed)
                {
                    Vector3 decelVel = Vector3.Lerp(dashVel, dashDir.normalized * maxSpeed, dashReturnRate * Time.fixedDeltaTime);
                    rb.velocity = new Vector3(decelVel.x, rb.velocity.y, decelVel.z);
                }
                else
                    isDashing = false;
            }
        }
        else
        {
            if (horizontalVel.magnitude > maxSpeed)
            {
                Vector3 limitedVel = horizontalVel.normalized * maxSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
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
*/

//I want to be able to change this script so that it feels a bit better to use, going to try using rigidbody but also going to
//need to try other options as well.

/*
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 30f;
    public float rotateSpeed = 10f;

    [Header("Drag Settings")]
    public float groundDrag = 6f;
    public float airDrag = 0.5f;
    public float dashDrag = 0.1f;

    [Header("Jump")]
    public float jumpForce = 8f;              // initial jump impulse
    public float holdJumpForce = 5f;          // extra force while holding
    public float holdJumpDecay = 0.9f;        // multiplier each frame
    public float fallMultiplier = 2.5f;       // stronger gravity when falling
    public float lowJumpMultiplier = 2f;      // stronger gravity if jump released early
    private bool jumpHeld = false;
    private float currentHoldForce;

    // Jump height tracking
    private float jumpStartHeight;
    private float highestPoint;
    private bool trackingJump = false;

    [Header("Dash")]
    public float dashForce = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public GameObject[] dashIndicators;

    [Header("References")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 movementValue;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 dashDir;

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

    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded)
            {
                // Initial jump impulse
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // Enable hold jump
                jumpHeld = true;
                currentHoldForce = holdJumpForce;

                // Start tracking jump height
                jumpStartHeight = transform.position.y;
                highestPoint = jumpStartHeight;
                trackingJump = true;
            }
            else
            {
                // Already in air, just flag held
                jumpHeld = true;
            }   
        }
        else
        {
            // Released
            jumpHeld = false;
        }
    }  


    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

            Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;
            if (moveDir.sqrMagnitude < 0.01f)
                moveDir = transform.forward;

            dashDir = moveDir.normalized;

            StartCoroutine(DashRoutine());
            StartCoroutine(DashCooldownRoutine());
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Camera-relative input
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;

        Debug.Log("Hold jump" + jumpHeld);

        
        // Track highest Y while in air
        if (trackingJump)
        {
            if (transform.position.y > highestPoint)
                highestPoint = transform.position.y;

            if (isGrounded && rb.velocity.y <= 0f)
            {
                float jumpHeight = highestPoint - jumpStartHeight;
                Debug.Log($"Jump height reached: {jumpHeight:F2} units");
                trackingJump = false;
            }
        }



        if (moveDir.sqrMagnitude > 1f)
            moveDir.Normalize();

        // Movement
        if (!isDashing)
        {
            rb.AddForce(moveDir * moveForce * Time.fixedDeltaTime, ForceMode.Force);
        }

        // Rotation
        if (cameraTransform != null && moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }

        // Gravity modifiers
        if (rb.velocity.y < 0)
        {
            // Falling → increase gravity
            rb.AddForce(Vector3.down * (Physics.gravity.y * (fallMultiplier - 1)) * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        else if (rb.velocity.y > 0 && !jumpHeld)
        {
            // Released jump early → low jump
            rb.AddForce(Vector3.down * (Physics.gravity.y * (lowJumpMultiplier - 1)) * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        // Variable jump sustain
        if (jumpHeld && currentHoldForce > 0f)
        {
            rb.AddForce(Vector3.up * currentHoldForce * Time.fixedDeltaTime, ForceMode.Force);
            currentHoldForce *= holdJumpDecay; // decay per frame
        }

        // Drag values
        if (isDashing)
            rb.drag = dashDrag;
        else if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;

        // Dash indicators
        if (dashIndicators != null)
        {
            foreach (var indicator in dashIndicators)
                if (indicator != null) indicator.SetActive(canDash);
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
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
} */

/*
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 1000f;
    public float rotateSpeed = 10f;
    public float maxSpeed = 10f;          // New: max ground speed
    public float airControl = 0.5f;      // New: reduces air movement influence

    [Header("Drag Settings")]
    public float baseGroundDrag = 0f;    // New: minimum drag on ground
    public float maxGroundDrag = 5f;     // New: maximum drag when near max speed
    public float airDrag = 0.5f;
    public float dashDrag = 0.1f;

    [Header("Jump")]
    public float jumpForce = 6f;
    //the value by which the current hold force decays 
    public float holdJumpDecay = 0.95f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    private bool jumpHeld = false;

    // a fading “sustain” push that rewards holding the button, like classic platformer variable jump height.
    private float currentHoldForce = 5f;

    public float holdForce = 5f;

    private float jumpStartHeight;
    private float highestPoint;
    private bool trackingJump = false;

    [Header("Dash")]
    public float dashForce = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public GameObject[] dashIndicators;

    [Header("References")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 movementValue;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 dashDir;

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

    public void OnMove(InputValue value) => movementValue = value.Get<Vector2>();

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                jumpHeld = true;

                currentHoldForce = holdForce;

                jumpStartHeight = transform.position.y;
                highestPoint = jumpStartHeight;
                trackingJump = true;
            }
            else jumpHeld = true;
        }
        else jumpHeld = false;
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

            Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;
            if (moveDir.sqrMagnitude < 0.01f)
                moveDir = transform.forward;

            dashDir = moveDir.normalized;

            StartCoroutine(DashRoutine());
            StartCoroutine(DashCooldownRoutine());
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;

        if (moveDir.sqrMagnitude > 1f)
            moveDir.Normalize();

        // Track jump height
        if (trackingJump)
        {
            if (transform.position.y > highestPoint)
                highestPoint = transform.position.y;

            if (isGrounded && rb.velocity.y <= 0f)
            {
                float jumpHeight = highestPoint - jumpStartHeight;
                Debug.Log($"Jump height reached: {jumpHeight:F2} units");
                trackingJump = false;
            }
        }

        // Movement force
        // Movement force / steering
        if (!isDashing && moveDir.sqrMagnitude > 0.01f)
        {
            if (isGrounded)
            {
                // Normal grounded acceleration
                rb.AddForce(moveDir * moveForce * Time.fixedDeltaTime, ForceMode.Force);
            }
            else
            {
                // --- Air steering ---
                Vector3 horizontalVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                if (horizontalVel.magnitude > 0.1f)
                {
                    // Find target direction based on input
                    Vector3 desiredDir = moveDir.normalized;

                    // Smoothly rotate velocity toward input direction
                    Vector3 newVel = Vector3.RotateTowards(
                        horizontalVel,
                        desiredDir * horizontalVel.magnitude,    // keep same speed
                        airControl * Time.fixedDeltaTime,        // rotation speed depends on airControl
                        0f
                        );

                    // Apply the steered velocity while preserving Y component
                    rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
                }
            }
        }


        // Rotation
        if (cameraTransform != null && moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }

        // Gravity mods
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * (Physics.gravity.y * (fallMultiplier - 1)) * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        else if (rb.velocity.y > 0 && !jumpHeld)
        {
            rb.AddForce(Vector3.down * (Physics.gravity.y * (lowJumpMultiplier - 1)) * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        // Hold jump
        if (jumpHeld && currentHoldForce > 0.5f)
        {
            rb.AddForce(Vector3.up * currentHoldForce * Time.fixedDeltaTime, ForceMode.Force);
            currentHoldForce *= holdJumpDecay;
            Debug.Log("Hold force being applied" + currentHoldForce);
        }

        // Dynamic drag system
        if (isDashing)
        {
            rb.drag = dashDrag;
        }
        else if (isGrounded)
        {
            float horizontalSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
            float t = Mathf.Clamp01(horizontalSpeed / maxSpeed);
            rb.drag = Mathf.Lerp(baseGroundDrag, maxGroundDrag, t);  // scales drag with speed
        }
        else
        {
            rb.drag = airDrag;
        }

        // Dash indicators
        if (dashIndicators != null)
        {
            foreach (var indicator in dashIndicators)
                if (indicator != null) indicator.SetActive(canDash);
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        float timer = 0f;

        while (timer < dashDuration)
        {
            rb.AddForce(dashDir * dashForce, ForceMode.Force);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        //I want to test an idea, I want the decreased drag to continue to apply for a set period of time even after the dash so it
        //doesn't just end instantly

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
*/

/*
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 1500f;
    public float rotateSpeed = 10f;
    public float maxSpeed = 10f;
    public float airControl = 3f;

    [Header("Drag Settings")]
    public float baseGroundDrag = 0f;
    public float maxGroundDrag = 5f;
    public float airDrag = 0f;
    public float dashDrag = 0.1f;
    public float dashDragDuration = 1f; // how long drag stays reduced after dash

    [Header("Jump")]
    public float holdForce = 100f;
    public float holdJumpDecay = 0.8f;
    //public float fallMultiplier = 3.5f;
    //public float lowJumpMultiplier = 2f;
    //public int jumpFrameLimit = 8;
    private bool jumpHeld = false;
    private bool jumpStarted = false; // tracks if a jump is currently active
    private float currentHoldForce;

    [Header("Dash")]
    public float dashForce = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public GameObject[] dashIndicators;

    [Header("References")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 movementValue;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 dashDir;
    private float dashDragTimer = 0f;

    private bool wasGrounded = true;

    private int jumpFrameCount;

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

    public bool IsJumpHeld()
    {
        return jumpHeld;
    }


    public void OnMove(InputValue value) => movementValue = value.Get<Vector2>();
    public void OnJump(InputValue value)
    {
        // Track the current button state
        jumpHeld = value.isPressed; // true while held, false on release

    // Only initialize a new jump when grounded and button pressed
    if (jumpHeld && isGrounded)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset vertical velocity
        currentHoldForce = holdForce;
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

            dashDir = moveDir.normalized;

            StartCoroutine(DashRoutine());
            StartCoroutine(DashCooldownRoutine());
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 moveDir = camForward * movementValue.y + camRight * movementValue.x;

        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        // Movement / Air steering
        if (!isDashing && moveDir.sqrMagnitude > 0.01f)
        {
            if (isGrounded)
            {
                // Grounded acceleration
                rb.AddForce(moveDir * moveForce * Time.fixedDeltaTime, ForceMode.Force);
            }
            else
            {
                // Air steering (redirect existing horizontal velocity)
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

        // Rotation
        if (cameraTransform != null && moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }


        //jumpHeld = jumpAction.ReadValue<float>() > 0.5f;


        //THIS SECTION STILL NEEDS TO BE FIXED BUT I'VE BEEN AT IT FOR TOO FUCKING LONG ASJDKFL;JASD;KLFJ
        //Needs to be changed so that it properly works and actually stops the player from jumping in mid air, idk why it currently
        //doesn't work in that respect and I might need to just generally rewrite it overall from the bottom up, I'll see but
        //everything else is better and this is a better system for a jump, still need to test slopes and etc as well; plus refine values
        //Also change the starting position of the camera (further out)

        //Currently the issue with the jump is that it keeps the values of the jump going, so if you let go and then press it again
        //it will still jump with the reduced vertical velocity that it would have if you had just held the button, and I don't
        //know why it does this, I think it's because the current implementation of the jump is so weird and messed up, I think
        //The overall implementation should be redone in a cleaner way which could fix this issue as rn all of it is in
        //update as opposed to the OnJump itself, which is very strange and I'm not sure its a good thing but it might be necessary

        //If OnJump only runs 2 times, once at inital press and once at release, then maybe changing the code so that  
        //when you initally press jump it checks if it is currently on the ground, if it is then it checks a variable allowing
        //the update function's jump, if not then it does nothing until ground touched, and the release causes it to change that
        //variable so that it becomes a (no you cannot jump until ground is touched, and that even means the held one)


        //Change this so it checks / only increments once while leaving the ground
        // Detect jump press only on transition from grounded -> pressed
        if (jumpHeld && isGrounded && !jumpStarted)
        {
            jumpStarted = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset vertical velocity
            currentHoldForce = holdForce;
            jumpFrameCount = 0;

            Debug.Log("Jump Started from Ground");
        }

        // Increment jumpFrameCount only while jump is held and jumpStarted
        if (jumpHeld && jumpStarted)
        {
            jumpFrameCount += 1;
            Debug.Log("Jump Held, frame count: " + jumpFrameCount);
        }

        // Reset jumpStarted when landing
        if (isGrounded && !wasGrounded)
        {
            jumpStarted = false;
            jumpFrameCount = 0;
            Debug.Log("Landed, jump reset");
        }

        // Store grounded state for next frame
        wasGrounded = isGrounded;

        //Debug.Log("Holding jump: " + jumpHeld);
        //jumpHeldPast = jumpHeld;


        // Sustained jump
        if (jumpHeld && currentHoldForce > 40f && jumpStarted)
        {
            rb.AddForce(Vector3.up * currentHoldForce, ForceMode.Force);
            currentHoldForce *= holdJumpDecay;
            Debug.Log($"Applying hold jump force: {currentHoldForce:F2}");
        }
        
        // Dynamic drag
        if (isDashing || dashDragTimer > 0f)
        {
            rb.drag = dashDrag;
            if (!isDashing) dashDragTimer -= Time.fixedDeltaTime;
        }
        else if (isGrounded)
        {
            
            //float horizontalSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
            //float t = Mathf.Clamp01(horizontalSpeed / maxSpeed);
            //rb.drag = Mathf.Lerp(baseGroundDrag, maxGroundDrag, t); 

            //No longer a linear curve
            float horizontalSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;

            
            //float t = Mathf.Clamp01(horizontalSpeed / maxSpeed);
            //t = Mathf.Pow(t, 3);
            //rb.drag = Mathf.Lerp(baseGroundDrag, maxGroundDrag, t); 

            float t = Mathf.Clamp01(horizontalSpeed / maxSpeed);

            // Exponential ramp: remains small for most of the range, jumps sharply near max
            float expT = Mathf.Pow(t, 5); // fifth power curve for strong end ramp

            rb.drag = Mathf.Lerp(baseGroundDrag, maxGroundDrag, expT);

        }
        else rb.drag = airDrag;

        // Debug horizontal speed
        //float horizontalVelMag = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
        //Debug.Log($"Horizontal Speed: {horizontalVelMag:F2}");
        
        // Dash indicators
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
*/

/*
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 1500f;
    public float rotateSpeed = 10f;
    public float maxSpeed = 10f;
    public float airControl = 3f;

    [Header("Drag Settings")]
    public float baseGroundDrag = 0f;
    public float maxGroundDrag = 5f;
    public float airDrag = 0f;
    public float dashDrag = 0.1f;
    public float dashDragDuration = 1.5f;

    [Header("Jump")]
    public float holdForce = 150f;
    public float holdJumpDecay = 0.5f;

    private bool jumpHeld = false;
    private bool jumpStarted = false;
    private float currentHoldForce;
    public float initalJumpForce = 5f;
    private bool hasLeftGround = false; // NEW - tracks whether we actually left the ground


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

        if (jumpHeld && isGrounded && !jumpStarted)
        {   
            jumpStarted = true;
            hasLeftGround = false;
            jumpStartTime = Time.time; // record when jump began
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * initalJumpForce, ForceMode.Impulse);
            currentHoldForce = holdForce;

            Debug.Log("Jump Started (waiting to confirm lift-off)");
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
                //rb.AddForce(moveDir * moveForce * Time.fixedDeltaTime, ForceMode.Force);
                Vector3 slopeDir = GetSlopeAdjustedDirection(moveDir);
                rb.AddForce(slopeDir * moveForce * Time.fixedDeltaTime, ForceMode.Force);

                // --- slope assist ---
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundDistance + 1f, groundMask))
                {
                    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                    if (slopeAngle > 0.1f && slopeAngle < 45f) // gentle slopes only
                    {
                        // Add a little push upward against gravity
                        rb.AddForce(Vector3.up * (slopeAngle * 0.5f), ForceMode.Acceleration);
                    }
                }
            }
            else
            {
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




    private void HandleDrag()
    {
        if (isDashing || dashDragTimer > 0f)
        {
            rb.drag = dashDrag;
            if (!isDashing) dashDragTimer -= Time.fixedDeltaTime;
        }
        else if (isGrounded)
        {
            float horizontalSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
            float t = Mathf.Clamp01(horizontalSpeed / maxSpeed);
            float expT = Mathf.Pow(t, 5);
            rb.drag = Mathf.Lerp(baseGroundDrag, maxGroundDrag, expT);
        }
        else rb.drag = airDrag;
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
*/



using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 1500f;
    public float rotateSpeed = 10f;
    public float maxSpeed = 10f;
    public float airControl = 3f;

    [Header("Drag Settings")]
    public float baseGroundDrag = 0f;
    public float maxGroundDrag = 5f;
    public float airDrag = 0f;
    public float dashDrag = 0.1f;
    public float dashDragDuration = 1.5f;

    [Header("Jump")]
    public float holdForce = 150f;
    public float holdJumpDecay = 0.5f;

    private bool jumpHeld = false;
    private bool jumpStarted = false;
    private float currentHoldForce;
    public float initalJumpForce = 5f;
    private bool hasLeftGround = false; // NEW - tracks whether we actually left the ground

    [Header("Wall Jump")]
    public float wallJumpUpForce = 6f;          // Upward push
    public float wallJumpHorizontalForce = 5f;  // Side push away from wall
    public int maxWallJumps = 2;                // Number of wall jumps before landing
    public float wallCheckDistance = 0.6f;      // How close to wall
    public LayerMask wallMask;                  // Which layers are walls
    public float wallStickGravityScale = 0.3f;  // How much gravity applies while sticking

    private int remainingWallJumps;
    private bool isTouchingWall;
    private Vector3 lastWallNormal;


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
        
        /* if (jumpHeld && isGrounded && !jumpStarted)
        {   
            jumpStarted = true;
            hasLeftGround = false;
            jumpStartTime = Time.time; // record when jump began
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * initalJumpForce, ForceMode.Impulse);
            currentHoldForce = holdForce;

            Debug.Log("Jump Started (waiting to confirm lift-off)");
        } */

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
        }

        // Optional: wall-stick slowdown (if you want slower sliding on walls)
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * wallStickGravityScale, rb.velocity.z);
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
            float slopeAngle = GetSlopeAngle();
            float horizontalSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
            float t = Mathf.Clamp01(horizontalSpeed / maxSpeed);
            float expT = Mathf.Pow(t, 5);

            // --- NEW slope-aware drag ---
            float slopeMultiplier = 1f - Mathf.Clamp01(slopeAngle / 45f);
            rb.drag = Mathf.Lerp(baseGroundDrag, maxGroundDrag, expT) * slopeMultiplier;
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
