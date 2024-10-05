using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalPanelU : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _gameManager.GameEnd += HandleEndGame;
    }

    private void OnDisable()
    {
        _gameManager.GameEnd -= HandleEndGame;
    }

    private void Awake()
    {
        _panel.SetActive(false);
    }

    private void HandleEndGame(bool isWin)
    {
        _panel.SetActive(true);
        if (isWin)
            _text.text = "YOU WIN";
        else
            _text.text = "YOU LOST";
    }
}
