/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 10/4/2025
  * Date Last Updated: 11/13/2025
  * Summary: Responsible for creating the items which can restore a players health when collected

  * Upadated: Added sound effect upon collection
  */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public AudioSource healthSound;
    public int healAmount = 1; // Amount of health to restore

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Restore health
            playerHealth.RestoreHealth(healAmount);

            if (healthSound != null)
                healthSound.Play();

            // Disable the pickup’s visuals/collider immediately
            GetComponent<Collider>().enabled = false;
        
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = false;
            

            // Destroy after the sound finishes
            Destroy(gameObject, healthSound.clip.length);
        

            /*
            healthSound.Play();

            // Destroy the pickup
            Destroy(gameObject); */
        }
    }
}
