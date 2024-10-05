using System;
using System.Collections;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Start parameters")]
    [SerializeField] private float _initialGold = 100;
    [SerializeField] private float _goldPerSecond = 1;
    [SerializeField] private int _multipleOfTheNumberToSend = 5;

    private float _currentGold;

    public Action<int> RefreshGoldEvent;
    public Action<int> CheckTowersGold;

    private void OnEnable()
    {
        _currentGold = _initialGold;
    }

    private void Awake()
    {
        HandleRefreshGold();
    }

    private void Update()
    {
        if ((int) _currentGold % _multipleOfTheNumberToSend == 0)
        {
            CheckTowersGold?.Invoke((int)_currentGold);
        }
    }

    private void HandleRefreshGold()
    {
        RefreshGoldEvent?.Invoke((int)_currentGold);
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
        RefreshGoldEvent?.Invoke((int)_currentGold);
    }

    public bool RemoveGold(int gold) 
    {
        if(_currentGold - gold < 0)
            return false;

        _currentGold -= gold;
        RefreshGoldEvent?.Invoke((int)_currentGold);
        return true;
    }
}
