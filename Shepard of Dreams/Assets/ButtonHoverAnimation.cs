/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/30/2025
  * Date Last Updated: 11/30/2025
  * Summary: Controls additional animation on the buttons for UI
  */

using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleAmount = 1.1f;
    public float moveAmount = 5f;
    public float duration = 0.2f;

    private RectTransform rect;
    private Vector3 originalScale;
    private Vector2 originalPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
        originalPos = rect.anchoredPosition;
    }

    void OnDisable()
    {
        if (rect != null)
            rect.DOKill();  // Stop tweens if object becomes inactive
    }

    void OnDestroy()
    {
        if (rect != null)
            rect.DOKill();  // Stop tweens if object is destroyed
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rect == null) return;

        rect.DOScale(originalScale * scaleAmount, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);  // Allows animation while paused

        rect.DOAnchorPos(originalPos + new Vector2(0f, moveAmount), duration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);  // Allows animation while paused
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (rect == null) return;

        rect.DOScale(originalScale, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        rect.DOAnchorPos(originalPos, duration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }
}


