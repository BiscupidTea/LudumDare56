using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _detailPanel;
    [SerializeField] private TMP_Text _text;

    private BaseTower _tower;
    private Button _button;

    public event Action<BaseTower> OnTryToUpgrade;

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
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);
        _detailPanel.SetActive(false);
    }

    private void HandleClick()
    {
        OnTryToUpgrade?.Invoke(_tower);
    }

    public void HandleTower(BaseTower tower)
    {
        _tower = tower;
        _text.text = _tower.currentUpgradePrice.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _detailPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _detailPanel.SetActive(false);
    }
}
