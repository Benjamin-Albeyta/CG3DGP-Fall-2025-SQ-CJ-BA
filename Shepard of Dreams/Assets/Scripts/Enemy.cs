/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 9/15/2025
  * Date Last Updated: 11/13/2025
  * Summary: Controls how enemies work, specifically getting knocked back when running into them and them damaging the player

  * Last Update: Added elements to activate a particle effect, play sound effect and rotate model upon attack plus chromatic abberation when hit
  */

/*
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float pushBackForce = 20f;  // How hard the enemy pushes the player back
    public float spinSpeed = 720f;     // How fast the model spins (degrees per second)

    private AudioSource needleSound;
    private ParticleSystem attackEffect;
    private Transform cactuarModel;
    private EnemyPatrol patrolScript;
    private bool isSpinning = false;
    public float spinDuration = 1f;

    private void Start()
    {
        // Find child components by name
        needleSound = GetComponentsInChildren<AudioSource>(true)
            .FirstOrDefault(a => a.name == "Cactuar Needles");

        attackEffect = GetComponentsInChildren<ParticleSystem>(true)
            .FirstOrDefault(p => p.name == "Attack");

        patrolScript = GetComponent<EnemyPatrol>();

        if (needleSound == null)
            Debug.LogWarning("AudioSource 'Cactuar Needles' not found as a child.");
        if (attackEffect == null)
            Debug.LogWarning("ParticleSystem 'Attack' not found as a child.");
        if (patrolScript == null)
            Debug.LogWarning("No EnemyPatrol script found on this GameObject.");

    }

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

            //Play sound
            if (needleSound != null)
            {
                needleSound.Play();
            }

            //Activate particle effect
            if (attackEffect != null)
            {
                attackEffect.Play();
            }

            //Start spinning the model
            if (!isSpinning)
            {
                StartCoroutine(SpinAndPause());
            }
        }
    }

    private IEnumerator SpinAndPause()
    {
        isSpinning = true;

        // Pause patrol if it exists
        if (patrolScript != null)
            patrolScript.Pause();

        float elapsed = 0f;
        while (elapsed < spinDuration)
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Resume patrol after spin
        if (patrolScript != null)
            patrolScript.Resume();

        isSpinning = false;
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Required for URP post-processing

public class Enemy : MonoBehaviour
{
    [Header("Combat Settings")]
    public float pushBackForce = 20f;
    public float spinSpeed = 720f;
    public float spinDuration = 1f;

    [Header("Post-Processing Effect")]
    public Volume playerVolume;          // Assign the player's Volume here
    public float chromaticMax = 1f;      // Peak intensity when hit
    public float chromaticLerpSpeed = 5f;
    public float chromaticFadeSpeed = 2f;

    private AudioSource needleSound;
    private ParticleSystem attackEffect;
    private EnemyPatrol patrolScript;
    private bool isSpinning = false;

    private ChromaticAberration chromatic;

    private void Start()
    {
        // --- Find child components by name ---
        needleSound = GetComponentsInChildren<AudioSource>(true)
            .FirstOrDefault(a => a.name == "Cactuar Needles");

        attackEffect = GetComponentsInChildren<ParticleSystem>(true)
            .FirstOrDefault(p => p.name == "Attack");

        patrolScript = GetComponent<EnemyPatrol>();

        // --- Get Chromatic Aberration from Volume ---
        if (playerVolume != null)
        {
            playerVolume.profile.TryGet(out chromatic);
        }

        if (needleSound == null)
            Debug.LogWarning("AudioSource 'Cactuar Needles' not found as a child.");
        if (attackEffect == null)
            Debug.LogWarning("ParticleSystem 'Attack' not found as a child.");
        if (patrolScript == null)
            Debug.LogWarning("No EnemyPatrol script found on this GameObject.");
        if (chromatic == null)
            Debug.LogWarning("Chromatic Aberration override not found in assigned Volume.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // --- Deal damage ---
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }

            // --- Push player back ---
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDir = (collision.transform.position - transform.position).normalized;
                pushDir.y = 0; // Keep horizontal push
                playerRb.AddForce(pushDir * pushBackForce, ForceMode.Impulse);
            }

            // --- Play sound ---
            if (needleSound != null)
                needleSound.Play();

            // --- Activate particle effect ---
            if (attackEffect != null)
                attackEffect.Play();

            // --- Spin and pause ---
            if (!isSpinning)
                StartCoroutine(SpinAndPause());

            // --- Trigger Chromatic Aberration hit feedback ---
            if (chromatic != null)
                StartCoroutine(ChromaticHitEffect());
        }
    }

    private IEnumerator SpinAndPause()
    {
        isSpinning = true;

        if (patrolScript != null)
            patrolScript.Pause();

        float elapsed = 0f;
        while (elapsed < spinDuration)
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (patrolScript != null)
            patrolScript.Resume();

        isSpinning = false;
    }

    private IEnumerator ChromaticHitEffect()
    {

        // Lerp up
        while (chromatic.intensity.value < chromaticMax - 0.01f)
        {
            chromatic.intensity.value = Mathf.Lerp(chromatic.intensity.value, chromaticMax, Time.deltaTime * chromaticLerpSpeed);
            yield return null;
        }

        // Hold briefly
        yield return new WaitForSeconds(0.1f);

        // Lerp down
        while (chromatic.intensity.value > 0.01f)
        {
            chromatic.intensity.value = Mathf.Lerp(chromatic.intensity.value, 0f, Time.deltaTime * chromaticFadeSpeed);
            yield return null;
        }

        chromatic.intensity.value = 0f; // ensure clean reset
    }
}
