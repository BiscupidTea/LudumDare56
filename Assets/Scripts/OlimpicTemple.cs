using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OlimpicTemple : MonoBehaviour, IHealth
{
    [SerializeField] private float _maxHealth;

    private float _currentHealth;

    public event Action<float> OnHealthChange;
    public event Action OnOlimpicDeath;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void Dead()
    {
        OnOlimpicDeath?.Invoke();
    }

    public void SuscribeActionDeath(Action action)
    {
        OnOlimpicDeath += action;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            OnHealthChange?.Invoke(0);
            Dead();
        }

        OnHealthChange?.Invoke(_currentHealth / _maxHealth);
    }

    public void UnsuscribeDeath(Action action)
    {
        OnOlimpicDeath -= action;
    }

    public void SuscribeLifeChange(Action<float> action)
    {
        OnHealthChange += action;
    }

    public void UnsuscribeLifeChange(Action<float> action)
    {
        OnHealthChange -= action;
    }
}
