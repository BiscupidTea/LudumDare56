using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int enemyCount;
    [SerializeField] private BaseEnemySO enemy;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform SpawnPosition;
    
    [SerializeField] private List<Transform> path;

    private void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab ,SpawnPosition.position, Quaternion.identity, transform);
            SpriteRenderer spriteRender = newEnemy.AddComponent<SpriteRenderer>();
            spriteRender.sprite = enemy.asset;
            
             BaseEnemy baseEnemyComponent = newEnemy.GetComponent<BaseEnemy>();
             baseEnemyComponent.enemySo = enemy;
             baseEnemyComponent.SetNewPath(path);
        }
    }
}