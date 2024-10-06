using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Transform target;
    private Rigidbody2D rb;

    protected Vector2 direction;
    private float speed;
    protected int damage;

    public event Action<Bullet> OnDeactivated = delegate { };

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    public void SetTarget(Transform _target, int _damage, float _speed)
    {
        if (_target == null) return;

        target = _target;
        damage = _damage;
        speed = _speed;

        direction = (target.position - transform.position).normalized;
        RotateTowardsTarget();
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;
    }

    protected virtual void OnCollisionEnter2D(Collision2D targetCollision)
    {
        if (targetCollision.transform == target)
        {
            if (targetCollision.transform.TryGetComponent<IHealth>(out var hitTarget))
            {
                hitTarget.TakeDamage(damage);
                Deactivate();
            }
        }
    }

    private void OnBecameInvisible()
    {
        Deactivate();
    }

    protected void Deactivate()
    {
        OnDeactivated?.Invoke(this);
        gameObject.SetActive(false);
    }
}
