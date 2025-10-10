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