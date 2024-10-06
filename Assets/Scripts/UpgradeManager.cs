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
        _deleteButton.onClick.AddListener(HandleDeleteTower);
    }

    private void OnDisable()
    {
        _spawnManager.OnTowerSpawned -= HandleSpawnTower;
        _upgradeButton.OnTryToUpgrade -= TryToUpgradeTower;
        for (int i = 0; i < _towersList.Count; i++)
        {
            _towersList[i].OnSelectedTower -= HandleSelectedTower;
        }
        _deleteButton.onClick.RemoveListener(HandleDeleteTower);
    }

    private void Awake()
    {
        _upgradeAndDeletePanel.gameObject.SetActive(false);
    }

    private void HandleSpawnTower(BaseTower tower)
    {
        tower.OnSelectedTower += HandleSelectedTower;
        _towersList.Add(tower);
    }

    private void HandleSelectedTower(BaseTower tower)
    {
        if (tower == null)
        {
            StartCoroutine(WaitingForNullSend());
            return;
        }
        StopAllCoroutines();
        _currentSelectedTower = tower;
        _upgradeButton.HandleTower(tower);
        _upgradeAndDeletePanel.gameObject.SetActive(true);
    }

    private void TryToUpgradeTower(BaseTower tower)
    {
        if (_economy.RemoveGold((int)tower.currentUpgradePrice))
        {
            tower.Upgrade();
        }
    }

    private void HandleDeleteTower()
    {
        if(_currentSelectedTower)
            _currentSelectedTower.DeleteTower();
        _currentSelectedTower = null;
    }


    private IEnumerator WaitingForNullSend()
    {
        yield return new WaitForSeconds(0.2f);
        _upgradeAndDeletePanel.gameObject.SetActive(false);
    }
}
