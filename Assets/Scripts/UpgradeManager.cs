using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private EconomyManager _economy;
    [SerializeField] private TowerSpawnManager _spawnManager;
    [SerializeField] private GameObject _upgradeAndDeletePanel;
    [SerializeField] private UpgradeButton _upgradeButton;
    [SerializeField] private Button _deleteButton;

    private List<BaseTower> _towersList = new();

    private BaseTower _currentSelectedTower;

    private void OnEnable()
    {
        _spawnManager.OnTowerSpawned += HandleSpawnTower;
        _upgradeButton.OnTryToUpgrade += TryToUpgradeTower;
    }

    private void OnDisable()
    {
        _spawnManager.OnTowerSpawned -= HandleSpawnTower;
        _upgradeButton.OnTryToUpgrade -= TryToUpgradeTower;
        for (int i = 0; i < _towersList.Count; i++)
        {
            _towersList[i].OnSelectedTower -= HandleSelectedTower;
        }
    }

    private void Awake()
    {
        _upgradeButton.gameObject.SetActive(false);
    }

    private void HandleSpawnTower(BaseTower tower)
    {
        tower.OnSelectedTower += HandleSelectedTower;
        _towersList.Add(tower);
    }

    private void HandleSelectedTower(BaseTower tower)
    {
        _currentSelectedTower = tower;
        _upgradeButton.HandleTower(tower);
        _upgradeButton.gameObject.SetActive(true);
    }

    private void TryToUpgradeTower(BaseTower tower)
    {
        if (_economy.RemoveGold((int)tower.currentUpgradePrice))
        {
            tower.Upgrade();
        }
    }
}
