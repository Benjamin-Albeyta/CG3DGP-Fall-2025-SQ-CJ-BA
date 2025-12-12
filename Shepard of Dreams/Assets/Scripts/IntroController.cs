/**
  * Author: Sophia Qian
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 12/12/2025
  * Date Last Updated: 12/12/2025
  * Summary: Controls the intro screen shown between the main menu and Level 1.
  *          Displays the story text, then automatically (or on key press) 
  *          transitions to the first gameplay scene.
  */

using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [Header("Next Scene")]
    [Tooltip("Name of the first gameplay scene (e.g. 'Level 1')")]
    public string nextSceneName = "Level 1";

    [Header("Timing")]
    [Tooltip("Seconds before automatically loading the next scene.")]
    public float autoLoadDelay = 5f;

    [Tooltip("Allow the player to skip the intro using any key.")]
    public bool allowSkipWithAnyKey = true;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // Auto-load next scene after delay
        if (timer >= autoLoadDelay)
        {
            LoadNextScene();
        }

        // Allow skipping the intro
        if (allowSkipWithAnyKey && Input.anyKeyDown)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // Prevent triggering multiple loads
        if (SceneManager.GetActiveScene().name == nextSceneName)
            return;

        SceneManager.LoadScene(nextSceneName);
    }
}
