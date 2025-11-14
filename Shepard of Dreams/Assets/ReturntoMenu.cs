/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/14/2025
  * Date Last Updated: 11/14/2025
  * Summary: Returns to Main Menu of the Current Level
  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturntoMenu : MonoBehaviour
{
    [Header("Menu Scene Settings")]
    public string mainMenuSceneName = "MainMenu"; // change to your menu scene name

    // Call this from a UI Button
    public void OnReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
