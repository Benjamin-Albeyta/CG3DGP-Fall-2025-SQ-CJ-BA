/**
  * Author: Sophia Qian
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Summary: Adds simple chase behavior on top of EnemyPatrol.
  * The enemy will patrol between point A and B,
  * but if the player enters a detection range,
  * the enemy pauses patrol and chases the player.
  */

using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Player")]
    public Transform player;              // Assign player here or it will try to find one by tag "Player"

    [Header("Chase Settings")]
    public float detectionRange = 6f;     // Distance at which the enemy starts chasing
    public float stopRange = 1.5f;        // Distance at which the enemy stops moving closer
    public float chaseSpeed = 3f;         // Move speed while chasing
    public float turnSpeed = 10f;         // How quickly the enemy rotates to face the player

    [Header("Integration")]
    public bool disablePatrolWhenChasing = true;

    private EnemyPatrol patrolScript;
    public bool IsChasing { get; private set; } = false;

    private void Awake()
    {
        // Get patrol script on the same GameObject
        patrolScript = GetComponent<EnemyPatrol>();

        // If player is not manually assigned, try to find by tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("EnemyChase: No player assigned and no GameObject with tag 'Player' found.");
            }
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Decide whether we are in chasing mode
        if (distance <= detectionRange)
        {
            // Enter chasing mode
            if (!IsChasing)
            {
                IsChasing = true;
                if (disablePatrolWhenChasing && patrolScript != null)
                {
                    patrolScript.Pause();
                }
            }

            // Move toward player if still outside stop range
            if (distance > stopRange)
            {
                Vector3 direction = player.position - transform.position;
                direction.y = 0f; // Keep movement in horizontal plane

                if (direction.sqrMagnitude > 0.001f)
                {
                    direction.Normalize();
                    transform.position += direction * chaseSpeed * Time.deltaTime;

                    // Smoothly rotate to face player
                    Quaternion targetRot = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRot,
                        turnSpeed * Time.deltaTime
                    );
                }
            }
        }
        else
        {
            // Exit chasing mode, return to patrol
            if (IsChasing)
            {
                IsChasing = false;
                if (disablePatrolWhenChasing && patrolScript != null)
                {
                    patrolScript.Resume();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection and stop ranges in Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
}
