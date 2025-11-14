/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/13/2025
  * Date Last Updated: 11/13/2025
  * Summary: Script for controlling sound effects for walking on the ground 
  */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEvents : MonoBehaviour
{

    [Header("Footstep Settings")]
    public AudioSource audioSource;           // The AudioSource to play from

    public AudioClip[] footstepClips;         // Array of footstep sounds

    public void FootstepSound()
    {
        if (footstepClips == null || footstepClips.Length == 0)
        {
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource assigned!");
            return;
        }

        // Pick a random clip
        int index = Random.Range(0, footstepClips.Length);
        AudioClip chosenClip = footstepClips[index];

        // Play the sound
        audioSource.PlayOneShot(chosenClip);

        // Debug feedback
        Debug.Log("Footstep sound played: " + chosenClip.name);
    }
}
