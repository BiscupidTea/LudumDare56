using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseEnemy : MonoBehaviour, IHealth<BaseEnemy>
{
    [Header("Enemy Data")] [SerializeField]
    public BaseEnemySO enemySo;

    [SerializeField] private Transform rotationPoint;
    [SerializeField] private float rotationSpeed = 5;

    private SpriteRenderer _view;
    private float _currentLifePoints;
    private List<Transform> pathPoints;
    private int currentPoint = 0;
    private OlimpicTemple _temple;

    private bool _canMove = false;

    private float distanceToReachPoint = 0.4f;

    public event Action<float> OnEnemyChangeLife;
    public event Action<BaseEnemy> OnEnemyDeath;

    public int ID { get; set; }

    private void Awake()
    {
        _view = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!_canMove)
            return;
        
        transform.position = Vector2.MoveTowards(transform.position, pathPoints[currentPoint + 1].position, enemySo.speed * Time.deltaTime);
        RotateTowardsTarget();

        if (Vector2.Distance(transform.position, pathPoints[currentPoint + 1].position) <= distanceToReachPoint)
        {
            currentPoint++;

            if (currentPoint >= pathPoints.Count - 1)
            {
                AttackTemple();
            }
        }
    }

    public void SetSO(BaseEnemySO so)
    {
        enemySo = so;
        _view.sprite = so.asset;
        _currentLifePoints = so.maxLife;
        _canMove = true;
    }

    public void SetTemple(OlimpicTemple temple)
    {
        _temple = temple;
    }

    public string GetName()
    {
        return enemySo.enemyName;
    }

    public void Revive()
    {
        transform.position = pathPoints[0].position;
        currentPoint = 0;
        _currentLifePoints = enemySo.maxLife;
        _canMove = true;
        gameObject.SetActive(true);
        OnEnemyChangeLife?.Invoke(_currentLifePoints / enemySo.maxLife);
    }

    public void SetNewPath(List<Transform> newPath)
    {
        pathPoints = newPath;
    }

    public void TakeDamage(int damage)
    {
        _currentLifePoints -= damage;
        if (_currentLifePoints <= 0)
        {
            Dead();
        }
        OnEnemyChangeLife?.Invoke(_currentLifePoints / enemySo.maxLife);
    }

    public void Dead()
    {
        OnEnemyDeath?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void SuscribeActionDeath(Action action)
    {
        throw new NotImplementedException();
    }

    public void UnsuscribeDeath(Action action)
    {
        throw new NotImplementedException();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceToReachPoint);
    }

    public void SuscribeLifeChange(Action<float> action)
    {
        OnEnemyChangeLife += action;
    }

    public void UnsuscribeLifeChange(Action<float> action)
    {
        OnEnemyChangeLife -= action;
    }

    public void SuscribeAction(Action<BaseEnemy> action)
    {
        OnEnemyDeath += action;
    }

    public void Unsuscribe(Action<BaseEnemy> action)
    {
        OnEnemyDeath -= action;
    }

    private void AttackTemple()
    {
        _canMove = false;
        _temple.TakeDamage((int)enemySo.damage);
        Dead();
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(pathPoints[currentPoint + 1].position.y - transform.position.y, pathPoints[currentPoint + 1].position.x - transform.position.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        rotationPoint.rotation = Quaternion.RotateTowards(rotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}