
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WaveController _waveController;
    [SerializeField] private OlimpicTemple _olimpicTemple;

    /// <summary>
    /// This event sent true if the player wins the game, in other case send false.
    /// </summary>
    public event Action<bool> GameEnd;

    private void OnEnable()
    {
        _waveController.FinishedWaves += HandleWavesAreOver;
        _olimpicTemple.OnOlimpicDeath += HandleOlimpicTempleDeath;
    }

    private void OnDisable()
    {
        _waveController.FinishedWaves -= HandleWavesAreOver;
        _olimpicTemple.OnOlimpicDeath -= HandleOlimpicTempleDeath;
    }

    private void Awake()
    {
        Validate();
    }

    private void HandleWavesAreOver()
    {
        GameEnd?.Invoke(true);
        Debug.Log("Ganaste");
    }

    private void HandleOlimpicTempleDeath()
    {
        GameEnd?.Invoke(false);
        Debug.Log("Perdiste");
    }

    private void Validate()
    {
        if (!_waveController)
        {
            Debug.LogError($"{name}: WaveController is null");
            enabled = false;
            return;
        }
        if (!_olimpicTemple)
        {
            Debug.LogError($"{name}: OlimpicTemple is null");
            enabled = false;
            return;
        }
    }
}
