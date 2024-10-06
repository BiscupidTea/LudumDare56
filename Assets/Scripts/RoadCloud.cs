using System.Collections;
using UnityEngine;

public class RoadCloud : MonoBehaviour
{
    [SerializeField] private SpriteRenderer poisonSprite;

    [SerializeField] private float animationSpeed = 1f;

    private float minAlpha = 0f;
    private Color startColor;
    private Color endColor = new Color(1f, 1f, 1f, 1f);

    private bool isPoisonous = false;

    private Coroutine currentAnimation;

    private void Awake()
    {
        startColor = poisonSprite.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, minAlpha);
        poisonSprite.color = endColor;
    }

    public void StartPoisonEffect()
    {
        if (isPoisonous) return;
        isPoisonous = true;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        StartCoroutine(AnimatePoisonSprite(isPoisonous));
    }

    public void StopPoisonEffect()
    {
        if (!isPoisonous) return;
        isPoisonous = false;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        StartCoroutine(AnimatePoisonSprite(isPoisonous));
    }

    private IEnumerator AnimatePoisonSprite(bool fadeIn)
    {
        float t = 0f;
        Color start = fadeIn ? endColor : startColor;
        Color end = fadeIn ? startColor : endColor;

        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;
            Color currentColor = Color.Lerp(start, end, t);
            poisonSprite.color = currentColor;
            yield return null;
        }

        currentAnimation = null;
    }
}
