/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/29/2025
  * Date Last Updated: 11/29/2025
  * Summary: For being attached to the object collected by the player
  */


using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    public AlternatingObjectSets alternator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alternator.StartAlternating();
            Destroy(gameObject);
        }
    }
}
