using UnityEngine;

public class PoisonBullet : Bullet
{
    [Header("Attributes")]
    [SerializeField] private DamageArea damageAreaPrefab;
    [SerializeField] private float effectRadius = 3f;

    protected override void OnCollisionEnter2D(Collision2D targetCollision)
    {
        if (targetCollision.transform == target)
        {
            if (targetCollision.transform.TryGetComponent<IHealth>(out var hitTarget))
            {
                hitTarget.TakeDamage(damage);

                Instantiate(damageAreaPrefab, transform.position, Quaternion.identity)
                    .GetComponent<DamageArea>()
                    .Initialize(effectRadius, damage);

                Deactivate();
            }
        }
    }
}
