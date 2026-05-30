using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SummonerTower : NormalTowerBase
{
    [Header("Summoner Settings")]
    [Tooltip("동시에 존재할 수 있는 미니 포탑의 최대 수")]
    [SerializeField] private int maxTurretCount = 3;
    [Tooltip("미니 포탑의 지속 시간")]
    [SerializeField] private float turretLifetime = 10.0f;
    
    private List<MiniTurret> activeTurrets = new List<MiniTurret>();

    protected override void Update()
    {
        base.Update();
        // 리스트에서 파괴된(null이 된) 미니 포탑을 제거하여 개수를 관리합니다.
        activeTurrets.RemoveAll(t => t == null);
    }

    protected override void Fire()
    {
        // 현재 활성화된 미니 포탑 수가 최대치보다 작을 때만 소환합니다.
        if (activeTurrets.Count < maxTurretCount)
        {
            Vector3 setPos = CurrentEnemy.transform.position + new Vector3(Random.Range(-1,1), 0, Random.Range(-1,1));
            if (bulletPrefab == null)
            {
                Debug.LogWarning("SummonerTower: bulletPrefab(미니 포탑 프리펩)이 설정되지 않았습니다.");
                return;
            }

            GameObject turretObj = Instantiate(bulletPrefab, setPos, firePoint.rotation);
            
            if (turretObj.TryGetComponent(out MiniTurret miniTurret))
            {
                // 모체인 SummonerTower의 파라미터를 미니 포탑에 전달합니다.
                miniTurret.InitializeTurret(damage, attackRange, attackInterval, bulletSpeed, turretLifetime);
                activeTurrets.Add(miniTurret);
            }
            else
            {
                Debug.LogWarning("SummonerTower: 생성된 프리펩에 MiniTurret 컴포넌트가 없습니다.");
            }
        }
    }
}
