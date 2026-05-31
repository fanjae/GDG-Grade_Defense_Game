using System.Collections;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Destroy time Setting")] //타워 Destroy 쿨타임
    [SerializeField] protected float DestroyInterval = 3.0f;
    [Header("Fainting Range")] //보스 스킬 범위
    [SerializeField] private float attackRange = 5.0f;
    [Tooltip("타워 레이어 설정")]
    [SerializeField] protected LayerMask towerLayer;

    public Transform centerPoint;
    private WaitForSeconds bossWait;
    private WaitForSeconds bossCooldown;
    private float bossInterval = 0.5f;

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

            Collider[] towers = Physics.OverlapSphere(centerPoint.position, attackRange, towerLayer);

            float nearestDistanceB = float.MaxValue;
            Tower nearestTower = null;

            foreach (Collider towerCollider in towers)
            {
                if (towerCollider.TryGetComponent(out Tower tower))
                {
                    float distanceB = Vector3.Distance(centerPoint.position, tower.transform.position);
                    if (distanceB < nearestDistanceB)
                    {
                        nearestDistanceB = distanceB;
                        nearestTower = tower;
                    }
                }
            }
            if(nearestTower != null)
            {
                Destroy(nearestTower.gameObject);
            }
            moveSpeed = originSpeed; //원래대로
            yield return bossCooldown;
        }
    }

    //조건 true일 시

    //이속 0
    //정면 내 가장 가까운 Tower Destroy
    //이속 원래대로
    //Co destroy 쿨 만큼 중단
}
    //기절: (1회)체력 nn% 이하로 떨어질 시 - 원형 범위 내 Tower n초 기절

    //조건: 체력 nn회 이하일 시 & 스킬 조건이 true일 시
    //이속 0
    //범위 내 Tower 의 공격속도를 n초 간 0
    //이속 원래대로