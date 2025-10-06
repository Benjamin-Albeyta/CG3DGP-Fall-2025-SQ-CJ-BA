using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody rb;
    private PlayerMovement playerMovement; // To check jumpHeld

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        // Apply gravity mods
        if (rb.velocity.y < 0)
        {
            // Faster fall
            rb.AddForce(Vector3.down * (Physics.gravity.y * (fallMultiplier - 1)) * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        else if (rb.velocity.y > 0 && playerMovement != null && !playerMovement.IsJumpHeld())
        {
            // Short hop when releasing jump early
            rb.AddForce(Vector3.down * (Physics.gravity.y * (lowJumpMultiplier - 1)) * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
}
