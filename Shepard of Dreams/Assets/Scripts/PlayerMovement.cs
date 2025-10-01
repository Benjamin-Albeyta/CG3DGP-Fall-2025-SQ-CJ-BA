using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    //TO-DO ORGANIZE THESE FUCKING VARIABLES OMG THEY ARE A MESS
    public float speed;
    public float maxSpeed;
    public float groundFriction;

    public float rotateSpeed;

    private Vector2 movementValue;
    public float airControlMultiplier;

    private float lookValue;
    public float maxJumpTime;
    public float jumpHoldForce;

    public float jumpForce;
    public float dashForce;
    public float dashCooldown;

    private Rigidbody rb;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    public float jumpSpeed;

    public float currentSpeed;

    private bool isGrounded;
    private bool canDash = true;

    private bool isDashing = false;

      // Jump tracking
    private bool jumpTriggered = false;

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
            jumpTriggered = true;
        }
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            Vector3 dashDir = transform.TransformDirection(new Vector3(movementValue.x, 0, movementValue.y).normalized);
            if (dashDir == Vector3.zero)
                dashDir = transform.forward;

            rb.AddForce(dashDir * dashForce, ForceMode.VelocityChange);
            isDashing = true;
            StartCoroutine(DashCooldownRoutine());
        }
    }

    private void FixedUpdate()
    {
        // --- Ground check ---
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // --- Movement ---
        Vector3 moveForce = new Vector3(movementValue.x, 0, movementValue.y);
        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reuse for friction & clamping

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

        // --- Jump logic (velocity-based) ---
        if (jumpTriggered)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            jumpTriggered = false;
        }

        // --- Clamp horizontal speed if not dashing ---
        horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z); // recalc after jump/forces
        if (!isDashing && horizontalVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = horizontalVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        // --- Update current speed ---
        currentSpeed = horizontalVel.magnitude;
    }

    private IEnumerator DashCooldownRoutine()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        isDashing = false;
    }
}