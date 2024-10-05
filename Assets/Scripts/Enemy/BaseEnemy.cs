using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IHealth
{
    [Header("Enemy Data")] [SerializeField]
    public BaseEnemySO enemySo;

    private List<Transform> pathPoints;
    private int currentPoint = 0;

    private float distanceToReachPoint = 0.4f;

    private void Update()
    {
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

    public void SetNewPath(List<Transform> newPath)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceToReachPoint);
    }
}