/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 10/14/2025
  * Date Last Updated: 10/14/2025
  * Summary: Changes textures of a given object in the level
  */

using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    [Tooltip("The renderer whose texture will change.")]
    public Renderer targetRenderer;

    [Tooltip("The new material or texture to apply when triggered.")]
    public Material newMaterial;

    [Tooltip("The object to show when triggered (e.g., the moving plane).")]
    public GameObject objectToShow;

    [Tooltip("Hide objectToShow at start (Play) for safety.")]
    public bool hideAtStart = true;

    private Material originalMaterial;

    private void Awake()
    {
        if (hideAtStart && objectToShow != null)
        {
            objectToShow.SetActive(false);

        }
    }


    //When executed activate the moving platform
    public void ChangeTexture()
    {
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);
        }
    }




    //When executed resets textures back to original state 
    public void ResetTexture()
    {
        if (objectToShow != null)
        {
            objectToShow.SetActive(false);
        }
    }
}