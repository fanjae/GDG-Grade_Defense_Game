using System.Collections.Generic;
using UnityEngine;

public class DebuffTower : NormalTowerBase
{
    [Header("디버프 설정")]
    [SerializeField] private LayerMask EnemyLayer; // 몬스터 레이어 
    [SerializeField] private float checkInterval = 0.5f; // 감지 주기

    [Header("디버프 수치")]
    [SerializeField] private float debuffSpeed = 50.0f; //감소시킬 이동속도 (50% 감소)

    private List<Enemy> targetsInRange = new List<Enemy>();
    private float lastCheckTime;

    protected override void Update()
    {
        if (Time.time - lastCheckTime >= checkInterval)
        {
            UpdateDeBuffs();
            lastCheckTime = Time.time;
        }
    }

    private void UpdateDeBuffs()
    {
        // 1. 범위 내의 모든 콜라이더 검출
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, EnemyLayer);
        List<Enemy> enemysInCurrentRange = new List<Enemy>();

        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out Enemy targetEnemy))
            {
                enemysInCurrentRange.Add(targetEnemy);

                // 새롭게 범위에 들어온 몬스터라면 디버프 적용
                if (!targetsInRange.Contains(targetEnemy))
                {
                    targetEnemy.GetSlow(debuffSpeed);
                    targetsInRange.Add(targetEnemy);
                    Debug.Log($"{targetEnemy.name}에게 디버프 적용");
                }
            }
        }

        // 2. 범위를 벗어난 몬스터 처리
        for (int i = targetsInRange.Count - 1; i >= 0; i--)
        {
            Enemy t = targetsInRange[i];
            if (t == null || !enemysInCurrentRange.Contains(t))
            {
                if (t != null) t.DispelSlow();
                targetsInRange.RemoveAt(i);
                Debug.Log("디버프 해제");
            }
        }
    }

    // 기존 공격 로직이 작동하지 않도록 Fire()를 비워둡니다.
    protected override void Fire() { }
}
