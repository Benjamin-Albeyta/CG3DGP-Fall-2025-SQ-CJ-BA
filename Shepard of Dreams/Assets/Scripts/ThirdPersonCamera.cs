using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;          // The player
    public Vector3 offset = new Vector3(0, 3, -6); // Default offset from player

    [Header("Rotation")]
    public float rotationSpeed = 5f;   // Mouse sensitivity
    public float pitchMin = -30f;      // Minimum vertical angle
    public float pitchMax = 60f;       // Maximum vertical angle

    [Header("Camera Collision")]
    public float collisionRadius = 0.3f;   // Sphere radius for collision check
    public LayerMask collisionMask;        // Layers to consider as obstacles

    private float yaw;   // Horizontal rotation
    private float pitch; // Vertical rotation
    private Vector3 currentOffset;

    private void Start()
    {
        currentOffset = offset;
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        //Camera rotation input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * rotationSpeed;
        pitch -= mouseY * rotationSpeed;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        //rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        //Calculate camera position
        Vector3 desiredPosition = target.position + rotation * offset;

        //Camera collision
        RaycastHit hit;
        Vector3 direction = desiredPosition - target.position;
        if (Physics.SphereCast(target.position, collisionRadius, direction.normalized, out hit, direction.magnitude, collisionMask))
        {
            desiredPosition = hit.point - direction.normalized * collisionRadius;
        }

        //Apply position and rotation
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f); // Look slightly above player's center
    }
}
