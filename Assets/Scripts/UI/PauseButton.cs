using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PauseButton : MonoBehaviour
{
    [SerializeField] private bool _isPaused;

    private Button _button;

    public Action<bool> IsPaused;

    private void OnEnable()
    {
        _button.onClick.AddListener(SendClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SendClick);
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SendClick);
    }

    private void SendClick()
    {
        IsPaused?.Invoke(_isPaused);
    }
}
