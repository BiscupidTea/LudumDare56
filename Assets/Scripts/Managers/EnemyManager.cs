using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private WaveController waveController;
    [SerializeField] private int enemyCount;
    [SerializeField] private float timeBetweenEnemies = 0.5f;
    [SerializeField] private BaseEnemy enemyPrefab;

    [SerializeField] private Transform SpawnPosition;
    
    [SerializeField] private List<Transform> path;

    private List<BaseEnemy> _activeEnemies = new();
    private Dictionary<string, List<BaseEnemy>> _poolEnemies = new();

    public event Action AllEnemiesDeath;

    private void OnEnable()
    {
        waveController.onWaveStart += StartWave;
    }

    private void OnDisable()
    {
        waveController.onWaveStart -= StartWave;
    }

    private void Awake()
    {
        if (!waveController)
        {
            Debug.LogError($"{name}: Wave controller is null");
            enabled = false;
            return;
        }
    }

    private void StartWave(List<BaseEnemySO> enemies)
    {
        StartCoroutine(SpawnEnemies(enemies));
    }

    private IEnumerator SpawnEnemies(List<BaseEnemySO> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            BaseEnemy baseEnemyComponent = PoolCheck(enemies[i]);
            _activeEnemies.Add(baseEnemyComponent);
            baseEnemyComponent.SuscribeAction(HandleEnemyDeath);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    private BaseEnemy NewEnemy(BaseEnemySO so)
    {
        BaseEnemy baseEnemyComponent = Instantiate(enemyPrefab, SpawnPosition.position, Quaternion.identity, transform);
        baseEnemyComponent.SetSO(so);
        baseEnemyComponent.SetNewPath(path);
        return baseEnemyComponent;
    }

    private BaseEnemy PoolCheck(BaseEnemySO so)
    {
        if (_poolEnemies.ContainsKey(so.enemyName))
        {
            if (_poolEnemies[so.enemyName].Count <= 0)
                return NewEnemy(so);
            else
            {
                BaseEnemy temp = _poolEnemies[so.enemyName][0];
                _poolEnemies[so.enemyName].Remove(temp);
                temp.Revive();
                return temp;
            }
        }
        else
        {
            return NewEnemy(so);
        }
    }

    private void HandleEnemyDeath(BaseEnemy enemy)
    {
        BaseEnemy temp = enemy;
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
        }

        if(_poolEnemies.ContainsKey(enemy.GetName()))
            {
            _poolEnemies[enemy.GetName()].Add(enemy);
            _activeEnemies.Remove(enemy);
            enemy.Unsuscribe(HandleEnemyDeath);
        }
    }
}