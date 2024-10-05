using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Start parameters")]
    [SerializeField] private float _initialGold = 100;
    [SerializeField] private float _goldPerSecond = 1;

    private float _currentGold;

    public Action<int> refreshGoldEvent;

    private void OnEnable()
    {
        _currentGold = _initialGold;
    }

    private void Awake()
    {
        HandleRefreshGold();
    }

    private void HandleRefreshGold()
    {
        refreshGoldEvent?.Invoke((int)_currentGold);
        StartCoroutine(RechargeGold());
    }

    private IEnumerator RechargeGold()
    {
        yield return new WaitForSeconds(1);
        _currentGold += _goldPerSecond;
        HandleRefreshGold();
    }

    public void AddGold(int gold)
    {
        _currentGold += gold;
        refreshGoldEvent?.Invoke((int)_currentGold);
    }

    public void RemoveGold(int gold) 
    {
        _currentGold -= gold;
        refreshGoldEvent?.Invoke((int)_currentGold);
    }
}
