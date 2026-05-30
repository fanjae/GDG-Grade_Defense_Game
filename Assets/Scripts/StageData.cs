using System;
using UnityEngine;

[Serializable]
public class StageData // 스테이지 데이터(웨이브 정보를 가짐)
{
    [SerializeField] private WaveData[] waves;
    public WaveData[] Waves => waves;
}