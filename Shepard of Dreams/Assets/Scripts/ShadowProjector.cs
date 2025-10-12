/**
  * Author: Benjamin Albeyta
  * Date Created: 10/4/2025
  * Date Last Updated: 10/11/2025
  * Summary: Generates and updates the position of a dropshadow showing the player where they will land after jumps
  */

/*
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
        //Creates instance of the shadow
        if (shadowPrefab != null)
        {
            shadowInstance = Instantiate(shadowPrefab);
        }
    }

    void Update()
    {
        //Updates position of shadow based on if there is ground under player
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
}*/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShadowCaster : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The player or object casting the shadow.")]
    public Transform player;

    [Tooltip("Prefab of the shadow (e.g., blob or decal).")]
    public GameObject shadowPrefab;

    [Header("Settings")]
    [Tooltip("Maximum distance to check for ground below the player.")]
    public float maxDistance = 50f;

    [Tooltip("Which layers count as 'ground'.")]
    public LayerMask groundLayer;

    [Header("Shadow Transform Controls")]
    [Tooltip("Offset of the shadow from the ground hit point (helps avoid z-fighting or floating).")]
    public Vector3 positionOffset = new Vector3(0, 0.01f, 0);

    [Tooltip("Base scale of the shadow.")]
    public Vector3 baseScale = Vector3.one;

    [Tooltip("Should shadow size change with height?")]
    public bool scaleWithHeight = true;

    [Tooltip("How much the shadow shrinks as the player rises.")]
    public float heightScaleFactor = 0.05f;

    private GameObject shadowInstance;

    void Start()
    {
        // Create instance of the shadow prefab
        if (shadowPrefab != null)
        {
            shadowInstance = Instantiate(shadowPrefab);
            shadowInstance.transform.localScale = baseScale;
        }
    }

    void Update()
    {
        if (player == null || shadowInstance == null) return;

        RaycastHit hit;
        Vector3 origin = player.position + Vector3.up * 0.1f;

        // Cast ray downward
        if (Physics.Raycast(origin, Vector3.down, out hit, maxDistance, groundLayer))
        {
            shadowInstance.SetActive(true);

            // --- POSITION ---
            shadowInstance.transform.position = hit.point + positionOffset;

            // --- ROTATION ---
            shadowInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // --- SCALE ---
            if (scaleWithHeight)
            {
                // Shrinks as the player gets higher above ground
                float distance = Vector3.Distance(player.position, hit.point);
                float scaleFactor = Mathf.Max(0.1f, 1f - distance * heightScaleFactor);
                shadowInstance.transform.localScale = baseScale * scaleFactor;
            }
            else
            {
                shadowInstance.transform.localScale = baseScale;
            }
        }
        else
        {
            shadowInstance.SetActive(false);
        }
    }
}

