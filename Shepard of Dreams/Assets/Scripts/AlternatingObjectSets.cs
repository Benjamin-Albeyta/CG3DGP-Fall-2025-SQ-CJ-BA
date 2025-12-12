/**
  * Author: Benjamin Albeyta, Sophia Qian
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/29/2025
  * Date Last Updated: 12/12/2025
  * Summary: Controls a system that when activated causes objects to appear and disappear on a timer, add to an empty game object and have the platforms added to the lists inside the inspector.

  * Update: Made it compatable with the variable speeds
  */

using System.Collections;
using UnityEngine;

public class AlternatingObjectSets : MonoBehaviour
{
    [Header("Object Sets")]
    public GameObject[] setA;
    public GameObject[] setB;

    [Header("Timing")]
    [Tooltip("Time each set stays visible before swapping.")]
    public float visibleDuration = 4f;

    [Tooltip("How long before a set appears its sound should play.")]
    public float preAppearSoundDelay = 0.75f;

    [Header("Flashing")]
    [Tooltip("Minimum flash speed at start (seconds per toggle).")]
    public float minFlashInterval = 0.3f;

    [Tooltip("Maximum flash speed at end (seconds per toggle).")]
    public float maxFlashInterval = 0.05f;

    [Header("Sound")]
    public AudioSource poofSound;
    public AudioSource poofSound2;

    [Header("Difficulty Adjustment")]
    [Tooltip("Multiplier applied to timings when the slow-platform potion has been collected.")]
    public float slowMultiplier = 1.5f;

    private bool effectStarted = false;
    private bool showingA = true;

    // Cached base values
    private float baseVisibleDuration;
    private float baseMinFlashInterval;
    private float baseMaxFlashInterval;

    private void Awake()
    {
        // Cache base values
        baseVisibleDuration = visibleDuration;
        baseMinFlashInterval = minFlashInterval;
        baseMaxFlashInterval = maxFlashInterval;
    }

    private void FixedUpdate()
    {
        // Update speed dynamically
        if (GlobalGameState.hasSlowPlatforms)
        {
            ApplySpeedSettings(true);
        }
        else
        {
            ApplySpeedSettings(false);
        }
    }

    private void ApplySpeedSettings(bool slowActive)
    {
        if (slowActive)
        {
            visibleDuration = baseVisibleDuration * slowMultiplier;
            minFlashInterval = baseMinFlashInterval * slowMultiplier;
            maxFlashInterval = baseMaxFlashInterval * slowMultiplier;
        }
        else
        {
            visibleDuration = baseVisibleDuration;
            minFlashInterval = baseMinFlashInterval;
            maxFlashInterval = baseMaxFlashInterval;
        }
    }

    public void StartAlternating()
    {
        if (!effectStarted)
        {
            effectStarted = true;
            StartCoroutine(AlternateSets());
        }
    }

    private IEnumerator AlternateSets()
    {
        while (true)
        {
            GameObject[] nextSet = showingA ? setA : setB;
            GameObject[] prevSet = showingA ? setB : setA;

            // Disable colliders on next set before flashing
            SetCollidersEnabled(nextSet, false);

            // Play pre-appear sound
            if (poofSound != null)
                poofSound.Play();

            // Start flashing
            Coroutine flashCoroutine = StartCoroutine(FlashObjectsLerp(nextSet, preAppearSoundDelay));

            // Wait for preAppearSoundDelay dynamically
            float elapsed = 0f;
            while (elapsed < preAppearSoundDelay)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Stop flashing and fully show next set
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            SetActive(nextSet, true);
            SetCollidersEnabled(nextSet, true); // enable colliders now

            // Play appear sound
            if (poofSound2 != null)
                poofSound2.Play();

            // Hide previous set
            SetActive(prevSet, false);
            SetCollidersEnabled(prevSet, false);

            // Remain visible dynamically
            elapsed = 0f;
            while (elapsed < visibleDuration)
            {
                elapsed += Time.deltaTime;
                yield return null; // check each frame if visibleDuration changes
            }

            showingA = !showingA;
        }
    }

    private IEnumerator FlashObjectsLerp(GameObject[] objects, float duration)
    {
        float elapsed = 0f;
        bool visible = false;

        // Colliders stay DISABLED during flashing
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float interval = Mathf.Lerp(minFlashInterval, maxFlashInterval, t);

            visible = !visible;
            SetActive(objects, visible);

            // Wait dynamically so interval adjusts if potion collected
            float waitElapsed = 0f;
            while (waitElapsed < interval)
            {
                waitElapsed += Time.deltaTime;
                yield return null;
            }

            elapsed += interval;
        }
    }

    private void SetActive(GameObject[] objects, bool state)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }

    private void SetCollidersEnabled(GameObject[] objects, bool state)
    {
        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;

            foreach (Collider c in obj.GetComponentsInChildren<Collider>())
                c.enabled = state;

            foreach (Collider2D c2 in obj.GetComponentsInChildren<Collider2D>())
                c2.enabled = state;
        }
    }
}
