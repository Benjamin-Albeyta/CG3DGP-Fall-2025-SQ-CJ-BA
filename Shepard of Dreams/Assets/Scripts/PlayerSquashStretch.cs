/**
  * Author: Benjamin Albeyta
  * Date Created: 10/12/2025
  * Date Last Updated: 10/14/2025
  * Summary: Applies squash and stretch to player model (still needs to be worked on / refined / implemented in terms of the dash, plus currently the squash brings off the ground and that needs to change) 
  */


using UnityEngine;

public class PlayerSquashStretch : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign the empty VisualParent object whose pivot is at the feet of the player.")]
    public Transform visualParent;

    [Header("General Settings")]
    [Tooltip("How quickly the scale returns to normal after deformation.")]
    public float returnSpeed = 8f;

    [Header("Vertical (Y-Axis) Settings")]
    [Tooltip("How much to squash vertically (Y). 1 = no change.")]
    public float verticalSquashAmount = 0.8f;
    [Tooltip("How much to stretch vertically (Y). 1 = no change.")]
    public float verticalStretchAmount = 1.2f;

    [Header("Horizontal (X/Z Axis) Settings")]
    [Tooltip("How much to squash horizontally (X/Z). 1 = no change.")]
    public float horizontalSquashAmount = 0.8f;
    [Tooltip("How much to stretch horizontally (X/Z). 1 = no change.")]
    public float horizontalStretchAmount = 1.2f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private void Awake()
    {
        if (visualParent == null)
        {
            Debug.LogError("PlayerSquashStretch: 'VisualParent' is not assigned! Please assign it in the Inspector.");
            return;
        }

        originalScale = visualParent.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        // Smoothly interpolate the visual scale back toward target
        visualParent.localScale = Vector3.Lerp(visualParent.localScale, targetScale, Time.deltaTime * returnSpeed);
    }

    /// <summary>
    /// Compress vertically and expand horizontally (used for land impacts).
    /// </summary>
    public void SquashVertical()
    {
        targetScale = new Vector3(
            originalScale.x * horizontalStretchAmount,
            originalScale.y * verticalSquashAmount,
            originalScale.z * horizontalStretchAmount
        );
    }

    /// <summary>
    /// Stretch vertically and compress horizontally (used for jumps).
    /// </summary>
    public void StretchVertical()
    {
        targetScale = new Vector3(
            originalScale.x * horizontalSquashAmount,
            originalScale.y * verticalStretchAmount,
            originalScale.z * horizontalSquashAmount
        );
    }

    /// <summary>
    /// Reset to original scale.
    /// </summary>
    public void ResetScale()
    {
        targetScale = originalScale;
    }
}
