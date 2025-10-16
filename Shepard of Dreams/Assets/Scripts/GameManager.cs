/**
  * Author: Sophia Qian, Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 9/20/2025
  * Date Last Updated: 10/15/2025
  * Summary: Manages the game state and is responsible for resetting the level if the player dies and moving to the next level upon the player reaching the goal
  */

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [Tooltip("If true, when the player completes the last level, it restarts from scene 0.")]
    public bool loopToFirstScene = true;

    private void Awake()
    {
        // Singleton pattern (but reset each scene)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CompleteLevel()
    {
        Debug.Log("[Game] Level complete!");
        LoadNextScene();
    }

    public void ResetLevel()
    {
        Debug.Log("[Game] Reset level");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void PlayerDied()
    {
        Debug.Log("[Game] Player died");
        ResetLevel();
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // If we're at the last scene
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            if (loopToFirstScene)
            {
                Debug.Log("[Game] Last level reached. Looping to first scene.");
                nextSceneIndex = 0;
            }
            else
            {
                Debug.Log("[Game] Last level reached. Staying here.");
                return;
            }
        }

        Debug.Log($"[Game] Loading next scene: {nextSceneIndex}");
        SceneManager.LoadScene(nextSceneIndex);
    }
}
