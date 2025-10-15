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

    private Material originalMaterial;

    private void Awake()
    {

        //When awake get the current component
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        originalMaterial = targetRenderer.material;
    }

    //When executed changes texture
    public void ChangeTexture()
    {
        if (newMaterial != null)
        {
            targetRenderer.material = newMaterial;
        }
    }


    //When executed resets textures back to original state 
    public void ResetTexture()
    {
        targetRenderer.material = originalMaterial;
    }
}
