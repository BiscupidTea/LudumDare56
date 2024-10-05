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

        RotateTowardsTarget();
    }

    public void SetTarget(Transform _target, int _damage, float _speed)
    {
        if (_target == null) return;

        target = _target;
        damage = _damage;
        speed = _speed;
    }

    public void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;
    }

    private void OnCollisionEnter2D(Collision2D targetCollision)
    {
        if (targetCollision.transform.TryGetComponent<IHealth>(out var hitTarget))
        {
            hitTarget.TakeDamage(damage);
            Deactivate();
        }
    }

    private void Deactivate()
    {
        OnDeactivated?.Invoke(this);
        gameObject.SetActive(false);
    }
}
