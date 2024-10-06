using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [Header("Tower Data")]
    [SerializeField] public BaseTowerSO towerSo;

    [Header("References")]
    [SerializeField] protected Transform aimPoint;
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] private LayerMask enemyMask;

    protected BaseEnemy target;

    private void Awake()
    {
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

        if (!CheckIfTargetInRange())
        {
            HandleTargetDeath(target);
            return;
        }

        Shoot(target.transform);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, towerSo.attackRadius, transform.right, 0f, enemyMask);

        BaseEnemy potentialTarget = null;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.TryGetComponent<BaseEnemy>(out var enemyTarget))
            {
                if (!enemyTarget.gameObject.activeSelf) continue;

                if (potentialTarget == null || enemyTarget.ID < potentialTarget.ID)
                    potentialTarget = enemyTarget;
            }
        }

        if (potentialTarget != null)
        {
            target = potentialTarget;
            target.OnEnemyDeath += HandleTargetDeath;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, towerSo.rotationSpeed * Time.deltaTime);
    }

    private bool CheckIfTargetInRange()
    {
        return Vector2.Distance(target.transform.position, transform.position) <= towerSo.attackRadius;
    }

    protected virtual void Shoot(Transform _lastKnownTarget) { }

    public float GetPrice()
    {
        return towerSo.price;
    }

    private void HandleTargetDeath(BaseEnemy enemy)
    {
        target.OnEnemyDeath -= HandleTargetDeath;
        target = null;
    }
}