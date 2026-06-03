using System;
using UnityEngine;

[Serializable]
public class EnemySpawnInfo // 적스폰 정보
{
    [Header("Enemy Info")]
    [SerializeField] private Enemy enemyPrefab;

    [Header("Spawn Setting")]
    [SerializeField] private int count = 5;
    [SerializeField] private float interval = 1.0f;
    [SerializeField] private float startDelay;


    public Enemy EnemyPrefab => enemyPrefab;
    public int Count => count;
    public float Interval => interval;

    public float StartDelay => startDelay;
}