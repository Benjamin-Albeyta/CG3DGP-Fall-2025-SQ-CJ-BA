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
