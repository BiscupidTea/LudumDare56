using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerAreaUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image attackRadius;
    [SerializeField] private float animationSpeed = 1f;

    [SerializeField] private float minAlpha = 0f;

    private Color startColor;
    private Color endColor = new Color(1f, 1f, 1f, 1f);

    private Coroutine currentAnimation;

    private void Awake()
    {
        BaseTower baseTower = GetComponentInParent<BaseTower>();

        startColor = attackRadius.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, minAlpha);
        attackRadius.color = endColor;

        attackRadius.GetComponent<RectTransform>().sizeDelta = new Vector2(baseTower.towerSo.attackRadius * 2, baseTower.towerSo.attackRadius * 2);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(AnimateAppearance(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(AnimateAppearance(false));
    }

    private IEnumerator AnimateAppearance(bool fadeIn)
    {
        float t = 0f;
        Color start = fadeIn ? endColor : startColor;
        Color end = fadeIn ? startColor : endColor;

        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;
            Color currentColor = Color.Lerp(start, end, t);
            attackRadius.color = currentColor;
            yield return null;
        }

        currentAnimation = null;
    }
}
