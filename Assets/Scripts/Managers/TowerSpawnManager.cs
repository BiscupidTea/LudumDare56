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

    private Spawner _currentSelectedSpawner;

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
            _currentSelectedSpawner.SpawnTower(baseTower);
    }
}
