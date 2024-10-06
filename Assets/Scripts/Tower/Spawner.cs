using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Spawner : MonoBehaviour, ISelectable
{
    private bool _isOcupaid;
    private BaseTower _towerSpawned;
    private SpriteRenderer _spriteRenderer;
    private Color _color;

    public event Action<Spawner> SpawnedClicked;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _color = _spriteRenderer.color;
    }

    public bool IsEmpty()
    {
        return !_isOcupaid;
    }

    public BaseTower SpawnTower(BaseTower baseTower)
    {
        if (_isOcupaid)
            return null;

        _towerSpawned = Instantiate(baseTower,transform.position,Quaternion.identity);
        _isOcupaid = true;
        _spriteRenderer.color = _color;
        return _towerSpawned;
    }

    public void ChangeColor(bool isObserved)
    {
        if (_isOcupaid)
            return;

        if (isObserved)
        {
            _spriteRenderer.color = Color.red;
            SpawnedClicked?.Invoke(this);
        }
        else
        {
            _spriteRenderer.color = _color;
        }
    }
}