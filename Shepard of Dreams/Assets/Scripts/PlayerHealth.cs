/**
  * Author: Benjamin Albeyta, Sophia Qian 
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 10/4/2025
  * Date Last Updated: 12/10/2025
  * Summary: Keeps track of the players health as denoted by a series of orbs surrounding the player

  * Update: Changed to use PlayerDeathHandler
  */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Health Visuals")]
    public GameObject[] healthObjects;
    public Transform player;
    public float radius = 1.5f;

    [Header("Invincibility")]
    public float invincibilityTime = 1.0f;  // seconds of invulnerability
    private bool isInvincible = false;
    private float invincibleTimer = 0f;
    [Header("Flash Settings")]
    public GameObject playerModel;          // Assign the player's visual model here
    public float flashInterval = 0.1f;

    private Renderer[] modelRenderers;      // Cached renderers to toggle visibility

    private Animator animator;
    [Header("Death")]
    public PlayerDeathHandler deathHandler;

    private void Start()
    {
        currentHealth = maxHealth;
        SpawnHealthObjects();

        //Gets the animator component
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Damage State", false);

        // Get all renderers from the player model
        if (playerModel != null)
            modelRenderers = playerModel.GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        // Countdown for invincibility
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
                isInvincible = false;
        }

        // Keep health objects floating
        for (int i = 0; i < maxHealth; i++)
        {
            if (healthObjects[i] != null && healthObjects[i].activeSelf)
            {
                float angle = (i / (float)maxHealth) * Mathf.PI * 2f + Time.time;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0.5f, Mathf.Sin(angle)) * radius;
                healthObjects[i].transform.position = player.position + offset;
            }
        }
    }

    //Processes taking damage including flashing the player and locking out movement
    public void TakeDamage()
    {
        if (isInvincible) return;


        if (currentHealth > 0)
        {
            currentHealth--;
            isInvincible = true;
            invincibleTimer = invincibilityTime;

            animator.SetBool("Damage State", true);

            // Disable the health object instead of destroying it
            if (healthObjects[currentHealth] != null)
            {
                healthObjects[currentHealth].SetActive(false);
            }

            // Start flashing coroutine
            if (playerModel != null)
                StartCoroutine(FlashDuringInvincibility());
        }
        else
        {
            Debug.Log("Player is dead!");

            if (deathHandler != null)
            {
                deathHandler.RunDeathSequence();
            }
        }

        GetComponent<PlayerMovement>().MovementLockOut(0.5f);
    }

    private IEnumerator FlashDuringInvincibility()
    {
        bool visible = true;

        while (invincibleTimer > 0f)
        {
            visible = !visible;
            SetModelVisibility(visible);
            yield return new WaitForSeconds(flashInterval);
        }

        // Ensure the model is visible at the end

        animator.SetBool("Damage State", false);
        SetModelVisibility(true);
    }

    private void SetModelVisibility(bool visible)
    {
        if (modelRenderers == null) return;
        foreach (var rend in modelRenderers)
            rend.enabled = visible;
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        // Re-enable the appropriate health objects
        for (int i = 0; i < currentHealth; i++)
        {
            if (healthObjects[i] != null && !healthObjects[i].activeSelf)
            {
                healthObjects[i].SetActive(true);
            }
        }
    }

    private void SpawnHealthObjects()
    {
        if (healthObjects != null && healthObjects.Length > 0) return;

        healthObjects = new GameObject[maxHealth];

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb.transform.localScale = Vector3.one * 0.3f;
            orb.SetActive(true);
            healthObjects[i] = orb;
        }
    }
}