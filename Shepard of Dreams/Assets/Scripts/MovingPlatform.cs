/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 10/11/2025
  * Date Last Updated: 10/11/2025
  * Summary: Responsible for moving a platform that this script is attached too, two objects pointA and pointB make up the path that the platform moves between
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
    }

    void FixedUpdate()
    {
        //If not at point A or point B then move towards the other point
        if (!pointA || !pointB) return;

        Vector3 newPosition = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector3.Distance(rb.position, target) < 0.05f)
        {
            movingToB = !movingToB;
            target = movingToB ? pointB.position : pointA.position;
        }
    }
}
