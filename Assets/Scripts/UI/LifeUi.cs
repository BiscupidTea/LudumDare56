using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUi : MonoBehaviour
{
    [SerializeField] private Image _lifeFillBar;
    [SerializeField] private GameObject _objectHealth;

    private IHealth _health;

    private void OnEnable()
    {
        _health.SuscribeLifeChange(HandleLifeChange);
    }

    private void OnDisable()
    {
        _health.UnsuscribeLifeChange(HandleLifeChange);
    }

    private void Awake()
    {
        if (!_objectHealth)
        {
            Debug.LogError($"{name}: ObjectHealth is null");
            enabled = false;
            return;
        }

        if (_objectHealth.TryGetComponent<IHealth>(out IHealth hp))
        {
            _health = hp;
        }
        else
        {
            Debug.LogError($"{name}: ObjectHealth don´t have IHealth");
            enabled = false;
            return;
        }
    }

    private void HandleLifeChange(float life)
    {
        _lifeFillBar.fillAmount = life;
    }
}
