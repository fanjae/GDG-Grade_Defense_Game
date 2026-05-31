using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    private float bossInterval = 0.5f; //보스 스킬 캐스팅 간격

    [Header("Destroy time Setting")] //타워 Destroy 쿨타임
    [SerializeField] private float DestroyInterval = 3.0f;
    [Header("TowerStun time Setting")] //타워의 Stun 시간
    [SerializeField] private float StunTime = 3.0f;

    [Header("Fainting Range")] //보스 스킬 범위
    [SerializeField] private float attackRange = 5.0f;
    [Tooltip("타워 레이어 설정")]
    [SerializeField] private LayerMask towerLayer;

    private List<NormalTowerBase> targetsInRange = new List<NormalTowerBase>();

    private WaitForSeconds bossWait;
    private WaitForSeconds bossCooldown;
    private bool isStun = true; //스턴 발동 조건

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHp <= maxHp / 2 && isStun == true) //현재 체력이 maxHp의 절반일 시(1회)
        {
            isStun = false; //1회만 발동
            StopCoroutine(DestroyCo());
            StartCoroutine(TowerStun());
        }
    }

    protected override void Reset()
    {
        moveSpeed = 2.0f;
        maxHp = 2000;
        coinValue = 1000;
    }
    protected void Awake()
    {
        bossWait = new WaitForSeconds(bossInterval);
        bossCooldown = new WaitForSeconds(DestroyInterval);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DestroyCo());
    }
    
    private IEnumerator DestroyCo()
    {
        //파괴: (n초마다)이동 중단 - 범위 내 가장 가까운 타워 삭제 - 쿨타임 대기 (이후 반복)
        while (true)
        {
            moveSpeed = 0f; //이동 중단
            yield return bossWait;

            Collider[] towers = Physics.OverlapSphere(transform.position, attackRange, towerLayer);

            float nearestDistance = float.MaxValue;
            NormalTowerBase nearestTower = null;

            foreach (Collider towerCollider in towers)
            {
                if (towerCollider.TryGetComponent(out NormalTowerBase tower))
                {
                    float distance = Vector3.Distance(transform.position, tower.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestTower = tower;
                    }
                }
            }
            if (nearestTower != null)
            {
                Destroy(nearestTower.gameObject);
            }
            yield return bossWait;
            moveSpeed = originSpeed; //원래대로
            yield return bossCooldown;
        }
    }

    //기절: 체력 50% 이하로 떨어질 시 - 원형 범위 내 Tower n초 기절
    private IEnumerator TowerStun()
    {
        moveSpeed = 0f; //이동 중단
        yield return bossWait;

        Collider[] towers = Physics.OverlapSphere(transform.position, attackRange, towerLayer);
        List<NormalTowerBase> towersInCurrentRange = new List<NormalTowerBase>();
        //범위 내 Tower들의 공격속도를 n초 간 0으로
        foreach (var col in towers)
        {
            if (col.TryGetComponent(out NormalTowerBase tower))
            {
                towersInCurrentRange.Add(tower);
            }
        }
        yield return bossWait;
        moveSpeed = originSpeed; //원래대로
        StartCoroutine(DestroyCo());
    }
}