/**
  * Author: Benjamin Albeyta
  * Date Created: 9/15/2025
  * Date Last Updated: 10/11/2025
  * Summary: Controls how enemies work, specifically gives them a movement where they go between two specified points (chosen as game objects)
  */

using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;         // First patrol point
    public Transform pointB;         // Second patrol point
    public float speed = 3f;         // Movement speed
    public float waitTime = 1f;      // Time to wait at each end

    private Transform targetPoint;   // Current target
    private float waitTimer;

    private void Start()
    {
        targetPoint = pointA; // start moving toward pointA
    }

    private void Update()
    {
        if (targetPoint == null || pointA == null || pointB == null) return;

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // If reached the target, switch after waiting
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (waitTimer <= 0f)
            {
                targetPoint = (targetPoint == pointA) ? pointB : pointA;
                waitTimer = waitTime;
            }
            else
            {
                waitTimer -= Time.deltaTime;
            }
        }

        // (Optional) Flip enemy to face movement direction
        Vector3 dir = (targetPoint.position - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            transform.forward = dir;
        }
    }
}
