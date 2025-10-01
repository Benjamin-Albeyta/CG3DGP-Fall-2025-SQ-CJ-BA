using UnityEngine;
using Cinemachine;

namespace Platformer {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour {
        [Header("References")]
        [SerializeField] Rigidbody rb;
        [SerializeField] Animator animator;
        [SerializeField] CinemachineFreeLook freeLookVCam;
        [SerializeField] InputReader input;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float gravityMultiplier = 3f;

        [Header("Dash Settings")]
        [SerializeField] float dashForce = 10f;
        [SerializeField] float dashCooldown = 2f;

        [Header("Attack Settings")]
        //[SerializeField] float attackCooldown = 0.5f;
        //[SerializeField] float attackDistance = 1f;
        //[SerializeField] int attackDamage = 10;

        static readonly int Speed = Animator.StringToHash("Speed");

        Transform mainCam;
        Vector3 movement;
        float currentSpeed;
        float velocityRef;

        bool isJumping;
        bool isGrounded;
        bool canDash = true;
        bool isDashing;
        //bool canAttack = true;

        void Awake() {
            mainCam = Camera.main.transform;
            rb.freezeRotation = true;

            if (freeLookVCam != null) {
                freeLookVCam.Follow = transform;
                freeLookVCam.LookAt = transform;
            }
        }

        void OnEnable() {
            input.Jump += OnJump;
            input.Dash += OnDash;
            input.Attack += OnAttack;
        }

        void OnDisable() {
            input.Jump -= OnJump;
            input.Dash -= OnDash;
            input.Attack -= OnAttack;
        }

        void Update() {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            UpdateAnimator();
        }

        void FixedUpdate() {
            HandleMovement();
            HandleGravity();
        }

        void UpdateAnimator() {
            animator.SetFloat(Speed, currentSpeed);
        }

        void HandleMovement() {
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;

            if (isDashing) {
                rb.velocity = adjustedDirection.normalized * dashForce;
                return;
            }

            if (adjustedDirection.magnitude > 0.1f) {
                HandleRotation(adjustedDirection);
                Vector3 velocity = adjustedDirection.normalized * moveSpeed;
                rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
                SmoothSpeed(adjustedDirection.magnitude);
            } else {
                SmoothSpeed(0f);
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
        }

        void HandleRotation(Vector3 adjustedDirection) {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        void SmoothSpeed(float value) {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocityRef, smoothTime);
        }

        void HandleGravity() {
            if (!isGrounded && !isJumping) {
                rb.velocity += Vector3.up * (Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime);
            }
        }

        void OnJump(bool performed) {
            if (performed && isGrounded && !isJumping) {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                isJumping = true;
            }
        }

        void OnDash(bool performed) {
            if (performed && canDash) {
                StartCoroutine(DashRoutine());
            }
        }

        void OnAttack() {
            //if (canAttack) StartCoroutine(AttackRoutine());
            //except no, need to work on making this work as like an interact button instead
        }

        System.Collections.IEnumerator DashRoutine() {
            isDashing = true;
            canDash = false;
            yield return new WaitForSeconds(0.2f); // short dash time
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        //System.Collections.IEnumerator AttackRoutine() {
            /*
            canAttack = false;
            Vector3 attackPos = transform.position + transform.forward;
            Collider[] hitEnemies = Physics.OverlapSphere(attackPos, attackDistance);

            foreach (var enemy in hitEnemies) {
                if (enemy.CompareTag("Enemy")) {
                    var health = enemy.GetComponent<Health>();
                    if (health != null) {
                        health.TakeDamage(attackDamage);
                    }
                }
            }

            yield return new WaitForSeconds(attackCooldown);
            canAttack = true; */
        //}

        void OnCollisionEnter(Collision collision) {
            if (collision.contacts.Length > 0) {
                var normal = collision.contacts[0].normal;
                if (Vector3.Dot(normal, Vector3.up) > 0.5f) {
                    isGrounded = true;
                    isJumping = false;
                }
            }
        }

        void OnCollisionExit(Collision collision) {
            isGrounded = false;
        }
    }
}
