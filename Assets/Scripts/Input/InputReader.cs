using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private LayerMask _spawnTowerMask;
    [SerializeField] private int _rangeLayer;
    private ISelectable spawn;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckSpawnTowerCollision();
        }
    }

    private void CheckSpawnTowerCollision()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit)
        {
            if (hit.transform.TryGetComponent<ISelectable>(out ISelectable hitSpawnTower))
            {
                if (spawn == null)
                {
                    spawn = hitSpawnTower;
                    spawn.ChangeColor(true);
                    spawn.SuscribeChangeColor(HandleDestroySpawn);
                }
                else
                {
                    spawn.ChangeColor(false);
                    spawn.UnsuscribeChangeColor(HandleDestroySpawn);
                    spawn = hitSpawnTower;
                    spawn.ChangeColor(true);
                    spawn.SuscribeChangeColor(HandleDestroySpawn);
                }
            }
        }
        else
        {
            if (spawn != null)
            {
                spawn.ChangeColor(false);
                spawn.UnsuscribeChangeColor(HandleDestroySpawn);
            }
        }
    }

    private void HandleDestroySpawn()
    {
        spawn.UnsuscribeChangeColor(HandleDestroySpawn);
        spawn = null;
    }
}
