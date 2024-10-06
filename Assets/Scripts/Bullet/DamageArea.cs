using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float poisonDuration = 5f;
    [SerializeField] private float damageCooldown = 1f;

    [SerializeField] private float animationSpeed = 1f;

    private float minScale = 0f;
    private float maxScale;

    private float minAlpha = 0f;
    private float maxAlpha;

    private Color startColor;
    private Color endColor = new Color(1f, 1f, 1f, 1f);

    private float effectRadius;

    private int poisonDamage;

    [Header("References")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask roadLayer;
    [SerializeField] private SpriteRenderer areaSprite;
    private Collider2D[] enemiesInArea;
    private Collider2D[] roadClouds;

    public void Initialize(float radius, int damage)
    {
        startColor = areaSprite.color;
        maxAlpha = startColor.a;
        endColor = new Color(startColor.r, startColor.g, startColor.b, minAlpha);
        areaSprite.color = endColor;

        maxScale = radius * 2;

        poisonDamage = damage;
        effectRadius = radius;
        StartCoroutine(AnimateAppearance(true));
        StartCoroutine(PoisonDamageSequence());
    }

    private IEnumerator PoisonDamageSequence()
    {
        roadClouds = Physics2D.OverlapCircleAll(transform.position, effectRadius, roadLayer);

        foreach (var cloud in roadClouds)
        {
            if (cloud.TryGetComponent<RoadCloud>(out var roadCloud))
            {
                roadCloud.StartPoisonEffect();
            }
        }


        yield return new WaitForSeconds(1);
        StartCoroutine(AnimateAppearance(false));

        float elapsed = 0f;
        Dictionary<Collider2D, float> enemyCooldowns = new Dictionary<Collider2D, float>();

        while (elapsed < poisonDuration)
        {
            enemiesInArea = Physics2D.OverlapCircleAll(transform.position, effectRadius, enemyLayer);

            foreach (var enemy in enemiesInArea)
            {
                if (enemy.TryGetComponent<IHealth>(out var enemyHealth))
                {
                    if (!enemyCooldowns.ContainsKey(enemy) || Time.time >= enemyCooldowns[enemy])
                    {
                        enemyHealth.TakeDamage(poisonDamage);
                        enemyCooldowns[enemy] = Time.time + damageCooldown;
                    }
                }
            }

            yield return new WaitForSeconds(damageCooldown);
            elapsed += damageCooldown;
        }

        foreach (var cloud in roadClouds)
        {
            if (cloud.TryGetComponent<RoadCloud>(out var roadCloud))
            {
                roadCloud.StopPoisonEffect();
            }
        }

        Destroy(gameObject);
    }

    private IEnumerator AnimateAppearance(bool fadeIn)
    {
        float t = 0f;

        if (fadeIn)
        {
            float startScale = minScale;
            float endScale = maxScale;

            transform.localScale = Vector3.one * startScale;
            areaSprite.color = new Color(areaSprite.color.r, areaSprite.color.g, areaSprite.color.b, maxAlpha); // Usar maxAlpha desde el inicio

            while (t < 1f)
            {
                t += Time.deltaTime * animationSpeed;

                float currentScale = Mathf.Lerp(startScale, endScale, t);
                transform.localScale = Vector3.one * currentScale;

                yield return null;
            }
        }

        else
        {
            Color startColor = new Color(areaSprite.color.r, areaSprite.color.g, areaSprite.color.b, maxAlpha);
            Color endColor = new Color(areaSprite.color.r, areaSprite.color.g, areaSprite.color.b, minAlpha);

            while (t < 1f)
            {
                t += Time.deltaTime * animationSpeed;

                Color currentColor = Color.Lerp(startColor, endColor, t);
                areaSprite.color = currentColor;

                yield return null;
            }
        }
    }
}
