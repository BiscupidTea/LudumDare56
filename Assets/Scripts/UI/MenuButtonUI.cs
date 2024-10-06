using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image hoverSprite;
    [SerializeField] private TMP_Text hoverText;

    [SerializeField] private float animationSpeed = 5f;

    private float minAlpha = 0f;
    private Coroutine currentAnimation;

    private void OnEnable()
    {
        StartCoroutine(ResetButtonAlpha());
    }

    private IEnumerator ResetButtonAlpha()
    {
        yield return new WaitForEndOfFrame();
        SetAlpha(hoverSprite, minAlpha);
        SetAlpha(hoverText, minAlpha);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(AnimateChange(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(AnimateChange(false));
    }

    private IEnumerator AnimateChange(bool fadeIn)
    {
        float t = 0f;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, t);

            SetAlpha(hoverSprite, currentAlpha);
            SetAlpha(hoverText, currentAlpha);

            yield return null;
        }

        SetAlpha(hoverSprite, endAlpha);
        SetAlpha(hoverText, endAlpha);

        currentAnimation = null;
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
}