/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 12/10/2025
  * Date Last Updated: 12/10/2025
  * Summary: Handles death of the player
  */

using UnityEngine;
using System.Collections;

public class PlayerDeathHandler : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public ThirdPersonCamera cameraFollow;
    public GameObject deathUI;
    public AudioSource deathSFX;
    public AudioSource deathSong;
    public AudioSource levelMusic;

    void Awake()
    {
        deathUI.SetActive(false);
    }

    public void RunDeathSequence()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        playerMovement.enabled = false;
        cameraFollow.enabled = false;

        levelMusic.Stop();
        deathSFX.Play();
        deathSong.Play();

        yield return new WaitForSeconds(0.2f);

        deathUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
