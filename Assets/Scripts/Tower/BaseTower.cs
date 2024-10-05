using UnityEditor;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [Header("Tower Data")]
    [SerializeField] protected BaseTowerSO towerSo;

    [Header("References")]
    [SerializeField] protected Transform rotationPoint;
    [SerializeField] protected Transform aimPoint;

    [SerializeField] protected Bullet bulletPrefab;

    protected Transform target;

    private void Awake()
    {
        if (!rotationPoint)
        {
            Debug.LogError($"{name}: Rotation point is null");
            enabled = false;
            return;
        }

        if (!aimPoint)
        {
            Debug.LogError($"{name}: Aim point is null");
            enabled = false;
            return;
        }

        if (!bulletPrefab)
        {
            Debug.LogError($"{name}: Bullet prefab is null");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();
        if (!CheckIfTargetInRange()) target = null;
        else Shoot();
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, towerSo.attackRadius, transform.right, 0f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.TryGetComponent<IHealth>(out var targetHealth))
            {
                target = hit.transform;
                return;
            }
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        rotationPoint.rotation = Quaternion.RotateTowards(rotationPoint.rotation, targetRotation, towerSo.rotationSpeed * Time.deltaTime);
    }

    private bool CheckIfTargetInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= towerSo.attackRadius;
    }

    protected virtual void Shoot() { }

    public float GetPrice()
    {
        return towerSo.price;
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, towerSo.attackRadius);
    }
}