/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 10/11/2025
  * Date Last Updated: 10/30/2025
  * Summary: Responsible for moving a platform that this script is attached too, two objects pointA and pointB make up the path that the platform moves between

  * Recent Update: Changed so that it properly carries momentum of player when on the platform
  */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement Settings")]
    public float speed = 2f;
    public bool startAtA = true;

    private Rigidbody rb;
    private Vector3 target;
    private bool movingToB;


    // Player momentum shifting
    private Vector3 lastPosition;
    private Vector3 platformDelta;
    private Rigidbody playerRb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // important: prevent unwanted physics interactions

        if (pointA == null || pointB == null)
        {
            Debug.LogError("MovingPlatform requires both PointA and PointB set.");
            enabled = false;
            return;
        }

        //Starts moving
        transform.position = startAtA ? pointA.position : pointB.position;
        movingToB = startAtA;
        target = movingToB ? pointB.position : pointA.position;

        lastPosition = transform.position;
    }

    void FixedUpdate()
    {

        if (!pointA || !pointB) return;

        // Move platform
        Vector3 newPosition = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        platformDelta = newPosition - rb.position;
        rb.MovePosition(newPosition);

        // Carry player if on platform
        if (playerRb != null)
        {
            playerRb.MovePosition(playerRb.position + platformDelta);
        }

        // Swap target when reached
        if (Vector3.Distance(rb.position, target) < 0.05f)
        {
            movingToB = !movingToB;
            target = movingToB ? pointB.position : pointA.position;
        }

        lastPosition = rb.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detect if player stands on top
        if (collision.rigidbody != null && collision.gameObject.CompareTag("Player"))
        {
            //make player part of the moving platform
            Debug.Log("player on moving platform");
            playerRb = collision.rigidbody;

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody != null && collision.rigidbody == playerRb)
        {
            Debug.Log("player off moving platform");
            playerRb = null;
        }
    }
}
