using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseEnemy : MonoBehaviour, IHealth<BaseEnemy>
{
    [Header("Enemy Data")] [SerializeField]
    public BaseEnemySO enemySo;

    private SpriteRenderer _view;
    private float _currentLifePoints;
    private List<Transform> pathPoints;
    private int currentPoint = 0;

    private bool _canMove = false;

    private float distanceToReachPoint = 0.4f;

    public event Action<float> OnEnemyChangeLife;
    public event Action<BaseEnemy> OnEnemyDeath;

    private void Awake()
    {
        _view = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!_canMove)
            return;
        
        transform.position = Vector2.MoveTowards(transform.position, pathPoints[currentPoint + 1].position, enemySo.speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, pathPoints[currentPoint + 1].position) <= distanceToReachPoint)
        {
            currentPoint++;

            if (currentPoint >= pathPoints.Count - 1)
            {
                enabled = false;
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

    public string GetName()
    {
        return enemySo.enemyName;
    }

    public void Revive()
    {
        transform.position = pathPoints[0].position;
        currentPoint = 0;
        _currentLifePoints = enemySo.maxLife;
        OnEnemyChangeLife?.Invoke(_currentLifePoints / enemySo.maxLife);
        gameObject.SetActive(true);
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
}