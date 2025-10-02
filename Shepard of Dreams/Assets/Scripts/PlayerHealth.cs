using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        currentHealth = maxHealth;
        SpawnHealthObjects();
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

    public void TakeDamage()
    {
        if (isInvincible) return;

        if (currentHealth > 0)
        {
            currentHealth--;
            isInvincible = true;
            invincibleTimer = invincibilityTime;

            // Disable the health object instead of destroying it
            if (healthObjects[currentHealth] != null)
            {
                healthObjects[currentHealth].SetActive(false);
            }
        }
        else
        {
            Debug.Log("Player is dead!");
            // TODO: trigger some type of game over
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
