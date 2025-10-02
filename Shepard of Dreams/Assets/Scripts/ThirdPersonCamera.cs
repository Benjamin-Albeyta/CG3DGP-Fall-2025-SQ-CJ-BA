/*using UnityEngine;

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
*/

/*using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                 // The player
    public Vector3 offset = new Vector3(0, 3, -6); // Default offset from player

    [Header("Rotation")]
    public float rotationSpeed = 120f;       // Sensitivity in degrees/second
    public float pitchMin = -30f;            // Minimum vertical angle
    public float pitchMax = 60f;             // Maximum vertical angle

    [Header("Camera Collision")]
    public float collisionRadius = 0.3f;     // Sphere radius for collision check
    public LayerMask collisionMask;          // Layers to consider as obstacles

    private float yaw;                       // Horizontal rotation
    private float pitch;                     // Vertical rotation
    private Vector2 lookInput;               // Input from the mouse/stick
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

        // Apply input to yaw/pitch
        yaw += lookInput.x * rotationSpeed * Time.deltaTime;
        pitch -= lookInput.y * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Desired camera position
        Vector3 desiredPosition = target.position + rotation * offset;

        // Camera collision
        RaycastHit hit;
        Vector3 direction = desiredPosition - target.position;
        if (Physics.SphereCast(target.position, collisionRadius, direction.normalized,
            out hit, direction.magnitude, collisionMask))
        {
            desiredPosition = hit.point - direction.normalized * collisionRadius;
        }

        // Apply position & rotation
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    // Called by the Input System (bind this action to "Look" in your Input Actions asset)
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
*/
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);

    [Header("Rotation")]
    public float sensitivity = 120f;  // degrees per second
    public float pitchMin = -30f;
    public float pitchMax = 70f;
    public float rotationSmoothTime = 0.05f;

    [Header("Camera Collision")]
    public float collisionRadius = 0.3f;
    public LayerMask collisionMask;

    private Vector2 lookInput;
    private float yaw;
    private float pitch;
    private Vector3 currentOffset;
    private Vector3 currentRotationVelocity;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("ThirdPersonCamera: No target assigned.");
            enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentOffset = offset;
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Apply input to rotation
        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Smooth rotation
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

        // Desired camera position
        Vector3 desiredPosition = target.position + targetRotation * offset;

        // Camera collision
        RaycastHit hit;
        Vector3 direction = desiredPosition - target.position;
        if (Physics.SphereCast(target.position, collisionRadius, direction.normalized, out hit, direction.magnitude, collisionMask))
        {
            desiredPosition = hit.point - direction.normalized * collisionRadius;
        }

        // Apply position & rotation
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    // Input System callback
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}
