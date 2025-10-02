using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // The player
    public Vector3 offset = new Vector3(0, 3, -6);

    [Header("Camera Reference")]
    public Transform cameraTransform; // The actual camera object

    [Header("Rotation")]
    public float rotationSpeed = 5f;
    public float pitchMin = -30f;
    public float pitchMax = 60f;

    [Header("Camera Collision")]
    public float collisionRadius = 0.3f;
    public LayerMask collisionMask;

    private float yaw;
    private float pitch;
    private Vector3 currentOffset;

    // Input value received from Send Messages
    private Vector2 lookInput;

    private void Start()
    {
        if (target == null || cameraTransform == null)
        {
            Debug.LogError("ThirdPersonCamera: Target or CameraTransform not assigned!");
            enabled = false;
            return;
        }

        currentOffset = offset;
        Vector3 angles = cameraTransform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // This method name must exactly match the Input Action name (e.g., "Look")
    // PlayerInput set to Send Messages will call this automatically
    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        if (target == null || cameraTransform == null) return;

        // Apply rotation from input
        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        pitch -= mouseY * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Calculate rotation quaternion
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Calculate desired camera position
        Vector3 desiredPosition = target.position + rotation * offset;

        // Camera collision
        Vector3 direction = desiredPosition - target.position;
        if (Physics.SphereCast(target.position, collisionRadius, direction.normalized, out RaycastHit hit, direction.magnitude, collisionMask))
        {
            desiredPosition = hit.point - direction.normalized * collisionRadius;
        }

        // Apply final position and rotation
        cameraTransform.position = desiredPosition;
        cameraTransform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
