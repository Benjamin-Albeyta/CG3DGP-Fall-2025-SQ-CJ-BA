/**
  * Author: Benjamin Albeyta
  * Date Created: 9/15/2025
  * Date Last Updated: 10/11/2025
  * Summary: Controls how enemies work, specifically getting knocked back when running into them and them damaging the player
  */

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float pushBackForce = 5f;  // How hard the enemy pushes the player back

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get PlayerHealth component
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }

            // Push player back
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDir = (collision.gameObject.transform.position - transform.position).normalized;
                pushDir.y = 0; // Keep push horizontal
                playerRb.AddForce(pushDir * pushBackForce, ForceMode.Impulse);
            }
        }
    }
}
