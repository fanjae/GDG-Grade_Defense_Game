using System;
using UnityEngine;

[Serializable]
public class WaveData // 하나의 웨이브에 등장해야할 적 스폰 정보 목록
{
    [SerializeField] private EnemySpawnInfo[] spawnInfos;

    public EnemySpawnInfo[] SpawnInfos => spawnInfos;
}