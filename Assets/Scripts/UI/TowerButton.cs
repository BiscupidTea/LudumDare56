using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TowerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    [SerializeField] private BaseTower _towerPrefab;
    [SerializeField] private GameObject _detailPanel;
    [SerializeField] private TMP_Text _text;

    public event Action<BaseTower> TowerToSpawn;

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    private void Awake()
    {
        Validate();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);
        _text.text = _towerPrefab.GetPrice().ToString();
        _detailPanel.SetActive(false);
    }

    private void HandleClick()
    {
        TowerToSpawn?.Invoke(_towerPrefab);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _detailPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _detailPanel.SetActive(false);
    }

    private void Validate()
    {
        if (!_towerPrefab)
        {
            Debug.LogError($"{name}: Tower prefab is null");
            enabled = false;
            return;
        }
        if (!_detailPanel)
        {
            Debug.LogError($"{name}: Panel is null");
            enabled = false;
            return;
        }
        if (!_text)
        {
            Debug.LogError($"{name}: Text is null");
            enabled = false;
            return;
        }
    }
}
