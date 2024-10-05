using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private WaveController waveController;
    [SerializeField] private int enemyCount;
    [SerializeField] private float timeBetweenEnemies = 0.5f;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform SpawnPosition;
    
    [SerializeField] private List<Transform> path;

    private void OnEnable()
    {
        waveController.onWaveStart += StartWave;
    }

    private void OnDisable()
    {
        waveController.onWaveStart -= StartWave;
    }

    private void Awake()
    {
        if (!waveController)
        {
            Debug.LogError($"{name}: Wave controller is null");
            enabled = false;
            return;
        }
    }

    private void StartWave(List<BaseEnemySO> enemies)
    {
        StartCoroutine(SpawnEnemies(enemies));
    }

    private IEnumerator SpawnEnemies(List<BaseEnemySO> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, SpawnPosition.position, Quaternion.identity, transform);
            SpriteRenderer spriteRender = newEnemy.AddComponent<SpriteRenderer>();
            spriteRender.sprite = enemies[i].asset;

            BaseEnemy baseEnemyComponent = newEnemy.GetComponent<BaseEnemy>();
            baseEnemyComponent.enemySo = enemies[i];
            baseEnemyComponent.SetNewPath(path);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }
}