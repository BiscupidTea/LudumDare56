using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private float speed;
    private int damage;

    public event Action<Bullet> OnDeactivated = delegate { };

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public void SetTarget(Transform _target, int _damage, float _speed)
    {
        if (_target == null) return;

        target = _target;
        damage = _damage;
        speed = _speed;
    }

    private void OnCollisionEnter2D(Collision2D targetCollision)
    {
        if (targetCollision.transform.TryGetComponent<IHealth>(out var hitTarget))
        {
            //hitTarget.TakeDamage(damage);
            Deactivate();
        }
    }

    private void Deactivate()
    {
        OnDeactivated?.Invoke(this);
        gameObject.SetActive(false);
    }
}
