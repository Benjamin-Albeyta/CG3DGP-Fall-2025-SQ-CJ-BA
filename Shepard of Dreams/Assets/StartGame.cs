/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/14/2025
  * Date Last Updated: 11/14/2025
  * Summary: For loading into the inital starting level
  */

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [Header("Scene Settings")]
    public string levelToLoad = "Level 1";

    // Call this method from a UI Button
    public void OnStartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

}
