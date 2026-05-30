using System.Collections.Generic;
using UnityEngine;

public class BuffTower : NormalTowerBase
{
    [Header("버프 설정")]
    [SerializeField] private LayerMask towerLayer; // 타워들이 속한 레이어 
    [SerializeField] private float checkInterval = 0.5f; // 감지 주기
    
    [Header("버프 수치")]
    [SerializeField] private int buffDamage = 5;
    [SerializeField] private float buffIntervalReduction = 0.2f;

    private List<NormalTowerBase> targetsInRange = new List<NormalTowerBase>();
    private float lastCheckTime;

    protected override void Update()
    {
        
        if (Time.time - lastCheckTime >= checkInterval)
        {
            UpdateBuffs();
            lastCheckTime = Time.time;
        }
    }

    private void UpdateBuffs()
    {
        // 1. 범위 내의 모든 콜라이더 검출
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, towerLayer);
        List<NormalTowerBase> towersInCurrentRange = new List<NormalTowerBase>();

        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out NormalTowerBase targetTower))
            {
                // 자기 자신은 제외
                if (targetTower == this) continue;
                
                towersInCurrentRange.Add(targetTower);

                // 새롭게 범위에 들어온 타워라면 버프 적용
                if (!targetsInRange.Contains(targetTower))
                {
                    targetTower.ApplyBuff(buffDamage, buffIntervalReduction);
                    targetsInRange.Add(targetTower);
                    Debug.Log($"{targetTower.name}에게 버프 적용");
                }
            }
        }

        // 2. 범위를 벗어난 타워 처리
        for (int i = targetsInRange.Count - 1; i >= 0; i--)
        {
            NormalTowerBase t = targetsInRange[i];
            if (t == null || !towersInCurrentRange.Contains(t))
            {
                if (t != null) t.RemoveBuff(buffDamage, buffIntervalReduction);
                targetsInRange.RemoveAt(i);
                Debug.Log("버프 해제");
            }
        }
    }

    // 버프 타워가 파괴될 때 적용 중이던 버프 모두 해제
    private void OnDestroy()
    {
        foreach (var t in targetsInRange)
        {
            if (t != null) t.RemoveBuff(buffDamage, buffIntervalReduction);
        }
    }

    // 기존 공격 로직이 작동하지 않도록 Fire()를 비워둡니다.
    protected override void Fire() { }
}