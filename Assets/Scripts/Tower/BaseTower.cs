using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseTower : MonoBehaviour, ISelectable
{
    [Header("Tower Data")]
    [SerializeField] protected BaseTowerSO towerSoP;

    [Header("References")]
    [SerializeField] protected Transform rotationPoint;
    [SerializeField] protected Transform aimPoint;

    [SerializeField] protected Bullet bulletPrefab;

    [SerializeField] private LayerMask enemyMask;

    [SerializeField] private GameObject attackRangeUI;

    protected float p_currentUpgradePrice;
    private SpriteRenderer _spriteRenderer;
    private Color _color;

    protected BaseEnemy target;

    public BaseTowerSO towerSo { get { return towerSoP; } }

    public float currentUpgradePrice { get { return p_currentUpgradePrice; } }

    public event Action<BaseTower> OnSelectedTower;
    public event Action OnDie = delegate{ };
    public event Action<BaseTower> OnDeleteTower;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _color = _spriteRenderer.color;
        p_currentUpgradePrice = towerSoP.upgradePrice;
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

        if (!CheckIfTargetInRange())
        {
            HandleTargetDeath(target);
            return;
        }

        Shoot(target.transform);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, towerSoP.attackRadius, transform.right, 0f, enemyMask);

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
        rotationPoint.rotation = Quaternion.RotateTowards(rotationPoint.rotation, targetRotation, towerSoP.rotationSpeed * Time.deltaTime);
    }

    private bool CheckIfTargetInRange()
    {
        return Vector2.Distance(target.transform.position, transform.position) <= towerSoP.attackRadius;
    }

    protected virtual void Shoot(Transform _lastKnownTarget) { }

    public float GetPrice()
    {
        return towerSoP.price;
    }

    private void HandleTargetDeath(BaseEnemy enemy)
    {
        target.OnEnemyDeath -= HandleTargetDeath;
        target = null;
    }

    public void ChangeColor(bool isSelected)
    {
        if (isSelected)
        {
            _spriteRenderer.color = Color.red;
            OnSelectedTower?.Invoke(this);
        }
        else
        {
            OnSelectedTower?.Invoke(null);
            _spriteRenderer.color = _color;
        }
    }

    public virtual void Upgrade() { }

    public virtual void DeleteTower()
    {
        OnDie.Invoke();
        OnDeleteTower?.Invoke(this);
        OnSelectedTower?.Invoke(null);
        StartCoroutine(WaitingForDie());
    }

    protected IEnumerator WaitingForDie()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void SuscribeChangeColor(Action  action)
    {
        OnDie += action;
    }

    public void UnsuscribeChangeColor(Action action)
    {
        StartCoroutine(WaitingUnsuscribe(action));
    }

    private IEnumerator WaitingUnsuscribe(Action action)
    {
        yield return new WaitForSeconds(0.5f);
        OnDie -= action;
    }
}