using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1; // Amount of health to restore

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Restore health
            playerHealth.RestoreHealth(healAmount);

            // Destroy the pickup
            Destroy(gameObject);
        }
    }
}
