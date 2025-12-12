/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/29/2025
  * Date Last Updated: 11/29/2025
  * Summary: Controlls a system that when activated causes objects to appear and disappear on a timer, add to an empty game object and have the platforms added to the lists inside the inspector.
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

    private void Start()
    {
        // If the player collected the special potion earlier, slow down the alternating behavior.
        if (GlobalGameState.hasSlowPlatforms)
        {
            visibleDuration *= slowMultiplier;
            minFlashInterval *= slowMultiplier;
            maxFlashInterval *= slowMultiplier;
        }
    }

    // Call this when the potion is collected
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

            // Play pre-appear sound
            if (poofSound != null)
                poofSound.Play();

            // Start flashing coroutine
            Coroutine flashCoroutine = StartCoroutine(FlashObjectsLerp(nextSet, preAppearSoundDelay));

            // Wait for pre-appear delay
            yield return new WaitForSeconds(preAppearSoundDelay);

            // Stop flashing and ensure fully visible
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);
            SetActive(nextSet, true);

            // Play appear sound
            if (poofSound2 != null)
                poofSound2.Play();

            // Hide previous set
            SetActive(prevSet, false);

            // Wait until next cycle
            yield return new WaitForSeconds(visibleDuration);

            showingA = !showingA;
        }
    }

    private IEnumerator FlashObjectsLerp(GameObject[] objects, float duration)
    {
        float elapsed = 0f;
        bool visible = false;

        while (elapsed < duration)
        {
            // Compute normalized progress (0 → 1)
            float t = elapsed / duration;

            // Lerp flash interval from min to max
            float interval = Mathf.Lerp(minFlashInterval, maxFlashInterval, t);

            // Toggle visibility
            visible = !visible;
            SetActive(objects, visible);

            // Wait for current interval
            elapsed += interval;
            yield return new WaitForSeconds(interval);
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
}
