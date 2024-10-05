using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private List<PauseButton> _pauseButtons;

    private void OnEnable()
    {
        for (int i = 0; i < _pauseButtons.Count; i++)
        {
            _pauseButtons[i].IsPaused += HandlePause;
        }    
    }

    private void OnDisable()
    {
        for (int i = 0; i < _pauseButtons.Count; i++)
        {
            _pauseButtons[i].IsPaused -= HandlePause;
        }
    }

    private void Awake()
    {
        _panel.SetActive(false);
    }

    private void HandlePause(bool pause)
    {
        _panel.SetActive(pause);
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}