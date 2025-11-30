/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/14/2025
  * Date Last Updated: 11/30/2025
  * Summary: Resets current level

  * Recent Change: Fixed an issue where restarting level resulted in all music going away
  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
        // Call this from a UI Button
    public void OnResetLevel()
    {
        AudioListener.pause = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

}
