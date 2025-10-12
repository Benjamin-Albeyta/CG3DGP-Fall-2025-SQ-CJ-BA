/**
  * Author: Benjamin Albeyta
  * Date Created: 9/20/2025
  * Date Last Updated: 10/11/2025
  * Summary: Handles player gravity by applying constant downward force on objects attached to 
  */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [Header("Gravity Settings")]
    [Tooltip("Downward acceleration applied constantly (m/s²).")]
    public float customGravity = 15f;

    [Tooltip("Extra downward force to keep player stuck to slopes.")]
    public float groundStickForce = 25f;

    [Tooltip("How far to raycast below to find the ground.")]
    public float groundCheckDistance = 0.4f;

    [Tooltip("Layers considered ground.")]
    public LayerMask groundMask;
    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 groundNormal = Vector3.up;

    public bool IsGrounded => isGrounded;
    public Vector3 GroundNormal => groundNormal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // disable Unity’s built-in gravity
    }

    private void FixedUpdate()
    {
        UpdateGroundState();

        // Apply gravity direction
        if (isGrounded)
        {
            // Align gravity along the slope so we press into it
            Vector3 slopeGravity = Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized;
            rb.AddForce(slopeGravity * customGravity, ForceMode.Acceleration);

            // Apply an extra "stick" force directly into the slope to avoid bouncing
            rb.AddForce(-groundNormal * groundStickForce, ForceMode.Acceleration);
        }
        else
        {
            // In air — apply normal downward gravity
            rb.AddForce(Vector3.down * customGravity, ForceMode.Acceleration);
        }
    }

    private void UpdateGroundState()
    {
        // Raycast slightly below player to find ground normal
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundMask))
        {
            isGrounded = true;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up;
        }
    }

#if UNITY_EDITOR
    // Optional: visualize ground ray in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
#endif
}

