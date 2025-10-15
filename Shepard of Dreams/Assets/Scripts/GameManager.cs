/**
  * Author: Sophia Qian
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 9/20/2025
  * Date Last Updated: 10/12/2025
  * Summary: Manages the game state and is responsible for resetting the level if the player dies and moving to the next level upon the player reaching the goal
  */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CompleteLevel()
    {
        Debug.Log("[Game] Level complete!");          
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
    }

    public void ResetLevel()
    {
        Debug.Log("[Game] Reset level");              
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerDied()
    {
        Debug.Log("[Game] Player died");
        ResetLevel();
    }
}