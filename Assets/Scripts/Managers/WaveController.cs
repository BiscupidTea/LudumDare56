using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private float _timeBetweenWave = 10;
    [SerializeField] private List<WaveDataSO> _waveDatas;

    private int _currentIndex;

    public event Action onCharging = delegate { };
    public event Action<float> onLoadPercentage = delegate { };
    public event Action onCharged = delegate { };

    public event Action<List<BaseEnemySO>> onWaveStart;
    public event Action FinishedWaves;

    private void Start()
    {
        _currentIndex = 0;
        //StartNewWave();
    }

    [ContextMenu("Start")]
    private void StartNewWave()
    {
        if (_currentIndex >= _waveDatas.Count)
        {
            FinishedWaves?.Invoke();
            return;
        }

        var total = _timeBetweenWave;
        onCharging?.Invoke();
        onLoadPercentage?.Invoke(0);
        StartCoroutine(WaitingNewWave(currentIndex => onLoadPercentage((float)currentIndex / 1)));
    }

    private IEnumerator WaitingNewWave(Action<float> onChargedQtyChanged)
    {
        float elapsedTime = _timeBetweenWave;

        while (elapsedTime >= 0)
        {
            elapsedTime -= Time.deltaTime;

            float percentage = Mathf.Clamp01(elapsedTime / _timeBetweenWave);
            onChargedQtyChanged?.Invoke(percentage);

            yield return new WaitForEndOfFrame();
        }

        onCharged?.Invoke();

        yield return new WaitForSeconds(1f);

        onWaveStart?.Invoke(_waveDatas[_currentIndex].enemyToSpawn);
    }
}