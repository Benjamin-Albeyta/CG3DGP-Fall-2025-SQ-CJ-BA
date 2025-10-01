using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float maxSpeed = 10f;
    public float groundFriction = 5f;
    public float airControlMultiplier = 0.5f;

    [Header("Rotation")]
    public float rotateSpeed = 5f;

    [Header("Jump")]
    public float jumpSpeed = 8f;   // simplified: just upward velocity

    [Header("Dash")]
    public float dashSpeed = 25f;       // target max dash velocity
    public float dashAccelRate = 60f;   // how fast we accelerate into dash speed
    public float dashCooldown = 1f;     // cooldown
    public float dashReturnRate = 5f;   // how fast we blend back to maxSpeed

    [Header("References")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Vector2 movementValue;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;

    // Dash state
    private Vector3 dashDir;
    private bool reachedDashSpeed = false;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // --- Input Callbacks ---
    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>() * speed;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            dashDir = transform.TransformDirection(new Vector3(movementValue.x, 0, movementValue.y).normalized);
            if (dashDir == Vector3.zero) dashDir = transform.forward;

            isDashing = true;
            reachedDashSpeed = false;

            StartCoroutine(DashCooldownRoutine());
        }
    }

    private void FixedUpdate()
    {
        // --- Ground check ---
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // --- Movement ---
        Vector3 moveForce = new Vector3(movementValue.x, 0, movementValue.y);
        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (isGrounded)
        {
            rb.AddRelativeForce(moveForce * Time.fixedDeltaTime, ForceMode.Force);

            // Apply friction if no input
            if (movementValue.sqrMagnitude < 0.01f)
            {
                Vector3 friction = -horizontalVel * groundFriction * Time.fixedDeltaTime;
                rb.AddForce(friction, ForceMode.VelocityChange);
            }
        }
        else
        {
            rb.AddRelativeForce(moveForce * airControlMultiplier * Time.fixedDeltaTime, ForceMode.Force);
        }

        // --- Dash logic ---
        if (isDashing)
        {
            Vector3 dashVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (!reachedDashSpeed)
            {
                // Accelerate toward dashSpeed
                Vector3 targetVel = dashDir.normalized * dashSpeed;
                Vector3 newVel = Vector3.MoveTowards(dashVel, targetVel, dashAccelRate * Time.fixedDeltaTime);

                rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);

                if (newVel.magnitude >= dashSpeed * 0.95f) // close enough
                {
                    reachedDashSpeed = true;
                }
            }
            else
            {
                // Decelerate smoothly back toward maxSpeed
                if (dashVel.magnitude > maxSpeed)
                {
                    Vector3 decelVel = Vector3.Lerp(dashVel, dashDir.normalized * maxSpeed, dashReturnRate * Time.fixedDeltaTime);
                    rb.velocity = new Vector3(decelVel.x, rb.velocity.y, decelVel.z);
                }
                else
                {
                    isDashing = false; // finished dash
                }
            }
        }
        else
        {
            // Normal speed clamp
            horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
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
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
