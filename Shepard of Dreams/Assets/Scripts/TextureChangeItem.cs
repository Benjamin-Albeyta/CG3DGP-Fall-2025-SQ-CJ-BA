/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 10/14/2025
  * Date Last Updated: 10/14/2025
  * Summary: Creates an item which when collected triggers a change in some textures in the level (tempoary but an implementation of a view change effect)
  */

using UnityEngine;

public class TextureChangeItem : MonoBehaviour
{
    [Tooltip("The object whose texture will change when collected.")]
    public TextureChanger target;

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger when the player touches it
        if (other.CompareTag("Player"))
        {
            if (target != null)
            {
                target.ChangeTexture();
            }

            // Destroy the item after collection
            Destroy(gameObject);
        }
    }
}
