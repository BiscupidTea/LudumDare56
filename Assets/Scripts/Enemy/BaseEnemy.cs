using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour, IHealth
{
    [Header("Enemy Data")] [SerializeField]
    private BaseEnemySO enemySo;

    public UnityAction OnDeath;

    private List<Vector2> pathPoints;
    private int currentPoint = 0;

    private void Update()
    {
        transform.position = Vector2.Lerp(pathPoints[currentPoint], pathPoints[currentPoint + 1], enemySo.speed);
    }

    public void SetNewPath(List<Vector2> newPath)
    {
        pathPoints = newPath;
    }

    public void TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }

    public void Dead()
    {
        throw new NotImplementedException();
    }

    public void SuscribeAction(Action action)
    {
        throw new NotImplementedException();
    }

    public void Unsuscribe(Action action)
    {
        throw new NotImplementedException();
    }
}