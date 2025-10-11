/**
  * Author: Benjamin Albeyta
  * Date Created: 9/20/2025
  * Date Last Updated: 10/11/2025
  * Summary: Controls a moving platform that moves between specified locations (determined by game objects)
  */

/*
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement")]
    public Transform pointA;           // First waypoint
    public Transform pointB;           // Second waypoint
    public float moveSpeed = 3f;       // Movement speed
    public float waitTime = 1f;        // Time to wait at each point

    [Header("Options")]
    public bool loop = true;           // Should it move back and forth?
    public bool carryPlayer = true;    // Should player stick to platform?

    private Vector3 targetPos;
    private bool movingToB = true;
    private float waitTimer = 0f;

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("MovingPlatform requires both pointA and pointB assigned.");
            enabled = false;
            return;
        }

        transform.position = pointA.position;
        targetPos = pointB.position;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Move toward current target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // When reached the target point
        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                // Swap direction
                movingToB = !movingToB;
                targetPos = movingToB ? pointB.position : pointA.position;
                waitTimer = 0f;

                // If not looping, stop after reaching B
                if (!loop && !movingToB)
                    enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.1f);
            Gizmos.DrawWireSphere(pointB.position, 0.1f);
        }
    }

    // --- Optional: let player move with platform ---
    private void OnCollisionEnter(Collision collision)
    {
        if (carryPlayer && collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (carryPlayer && collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 3f;
    public float waitTime = 1f;

    [Header("Options")]
    public bool loop = true;
    public bool carryPlayer = true;

    private Vector3 targetPos;
    private bool movingToB = true;
    private float waitTimer = 0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Important for moving platforms!
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (pointA == null || pointB == null)
        {
            Debug.LogError("MovingPlatform requires both pointA and pointB assigned.");
            enabled = false;
            return;
        }

        transform.position = pointA.position;
        targetPos = pointB.position;
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= waitTime)
            {
                movingToB = !movingToB;
                targetPos = movingToB ? pointB.position : pointA.position;
                waitTimer = 0f;

                if (!loop && !movingToB)
                    enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (carryPlayer && collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (carryPlayer && collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.1f);
            Gizmos.DrawWireSphere(pointB.position, 0.1f);
        }
    }
}
