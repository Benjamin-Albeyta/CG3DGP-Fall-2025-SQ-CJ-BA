using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float speed;

    public float rotateSpeed;

    private Vector2 movementValue;

    private float lookValue;

    public float jumpForce;
    public float dashForce;
    public float dashCooldown;

    private Rigidbody rb;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private bool isGrounded;
    private bool canDash = true;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent physics from messing with rotation
    }

    // --- INPUT CALLBACKS ---

    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>() * speed;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            Vector3 dashDir = transform.TransformDirection(new Vector3(movementValue.x, 0, movementValue.y).normalized);

            if (dashDir == Vector3.zero)
                dashDir = transform.forward; // dash forward if no movement input

            rb.AddForce(dashDir * dashForce, ForceMode.VelocityChange);
            StartCoroutine(DashCooldownRoutine());
        }
    }

    // --- UPDATE & GROUND CHECK ---

    void Update()
    {
        // Ground check with small sphere at feet
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Movement
        rb.AddRelativeForce(
            movementValue.x * Time.deltaTime,
            0,
            movementValue.y * Time.deltaTime);
    }

    private IEnumerator DashCooldownRoutine()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
