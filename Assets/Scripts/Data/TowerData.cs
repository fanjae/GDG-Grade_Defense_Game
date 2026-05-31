using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/Tower Data")]
public class TowerData : ScriptableObject
{
    [Header("기본 정보")]
    public string[] towerNames;

    [Header("UI")]
    public Sprite[] icons;

    [Header("일반 프리팹")]
    public GameObject[] towerPrefabs;

    [Header("고스트 프리팹")]
    public GameObject[] towerGhostPrefabs;
}