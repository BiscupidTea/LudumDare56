using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private EconomyManager _economyManager;
    [Header("Parameters")]
    [SerializeField] private List<TowerButton> _towerButtons;
    [SerializeField] private List<Spawner> _spawnerPoints;

    private Dictionary<BaseTower, Spawner> _towerAndSpawnerDic = new();

    private Spawner _currentSelectedSpawner;

    public event Action<BaseTower> OnTowerSpawned;

    private void OnEnable()
    {
        for (int i = 0; i < _towerButtons.Count; i++)
        {
            _towerButtons[i].TowerToSpawn += HandleTowerToSpawn;
        }
        for (int i = 0; i < _spawnerPoints.Count; i++)
        {
            _spawnerPoints[i].SpawnedClicked += HandleSpawnPoint;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _towerButtons.Count; i++)
        {
            _towerButtons[i].TowerToSpawn -= HandleTowerToSpawn;
        }
        for (int i = 0; i < _spawnerPoints.Count; i++)
        {
            _spawnerPoints[i].SpawnedClicked -= HandleSpawnPoint;
        }
        foreach (var item in _towerAndSpawnerDic.Keys)
        {
            item.OnDeleteTower -= DeleteTower;
        }
    }

    private void HandleSpawnPoint(Spawner spawnPoint)
    {
        _currentSelectedSpawner = spawnPoint;
    }

    private void HandleTowerToSpawn(BaseTower baseTower)
    {
        if (!_currentSelectedSpawner)
            return;

        if(_currentSelectedSpawner.IsEmpty() && _economyManager.RemoveGold((int)baseTower.GetPrice()))
        {
            BaseTower temp;
            temp = _currentSelectedSpawner.SpawnTower(baseTower);
            if (temp)
            {
                _towerAndSpawnerDic.Add(temp, _currentSelectedSpawner);
                OnTowerSpawned?.Invoke(temp);
                temp.OnDeleteTower += DeleteTower;
            }
        }
    }

    public void DeleteTower(BaseTower tower)
    {
        tower.OnDeleteTower -= DeleteTower;
        if (_towerAndSpawnerDic.ContainsKey(tower))
        {
            _towerAndSpawnerDic[tower].DeleteTower();
            _towerAndSpawnerDic.Remove(tower);
        }
    }
}
