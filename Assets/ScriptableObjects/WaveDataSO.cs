using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveDataSo", menuName = "Create New WaveData")]
public class WaveDataSO : ScriptableObject
{
    public List<BaseEnemySO> enemyToSpawn;
}
