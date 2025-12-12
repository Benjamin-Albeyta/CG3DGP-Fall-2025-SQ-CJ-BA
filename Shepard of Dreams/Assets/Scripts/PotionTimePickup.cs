/**
  * Author: Sophia Qian
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 12/5/2025
  * Summary: Special potion in Level 1.
  *          When the player touches it, it flags that platforms
  *          in Level 3 should be slowed down, then destroys itself.
  */

using UnityEngine;

public class PotionTimePickup : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;

        // Mark that the player has collected the special potion
        GlobalGameState.hasSlowPlatforms = true;

        // Optionally hide the visual mesh immediately
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // Destroy this object shortly after pickup
        Destroy(gameObject, 0.1f);
    }
}
