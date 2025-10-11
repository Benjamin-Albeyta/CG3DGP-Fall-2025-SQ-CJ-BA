/**
  * Author: Benjamin Albeyta
  * Date Created: 10/8/2025
  * Date Last Updated: 10/11/2025
  * Summary: Creates a shadow object to show the player where they are landing
  */


using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShadowCaster : MonoBehaviour
{
    [Header("References")]
    public Transform player;        // The player transform
    public GameObject shadowPrefab; // Shadow prefab (like a small circle or blob)
    
    [Header("Settings")]
    public float maxDistance = 50f; // Max distance to check for ground
    public LayerMask groundLayer;   // Layer of the ground

    private GameObject shadowInstance;

    void Start()
    {
        if (shadowPrefab != null)
        {
            shadowInstance = Instantiate(shadowPrefab);
        }
    }

    void Update()
    {
        if (player == null || shadowInstance == null) return;

        RaycastHit hit;
        Vector3 origin = player.position + Vector3.up * 0.1f; // Slight offset above player

        // Cast ray downwards
        if (Physics.Raycast(origin, Vector3.down, out hit, maxDistance, groundLayer))
        {
            shadowInstance.SetActive(true);
            shadowInstance.transform.position = hit.point + Vector3.up * 0.01f; // Avoid z-fighting
            shadowInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            shadowInstance.SetActive(false); // Hide if no ground below
        }
    }
}
