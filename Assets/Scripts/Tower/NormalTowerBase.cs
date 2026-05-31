using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NormalTowerBase : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("회전하고 총알이 나갈 위치")]
    [SerializeField] private Transform towerHead;
    [Tooltip("총알 생성 위치")]
    [SerializeField] protected Transform firePoint;
    [Tooltip("총알 프리펩")]
    [SerializeField] protected GameObject bulletPrefab;

    [Header("공격 세팅")] 
    [Tooltip("공격 사거리")]
    [SerializeField] protected float attackRange = 5.0f;
    [Tooltip("공격 간격 작을 수록 빠른 공격")]
    [SerializeField] protected float attackInterval = 0.8f;
    [Tooltip("한발당 데미지")]
    [SerializeField] protected int damage = 1;
    [Tooltip("총알 발사 속도 ")]
    [SerializeField] protected float bulletSpeed = 10.0f;

    [Tooltip("적 판별 레이어 설정")]
    [SerializeField] protected LayerMask enemyLayer;

    [Header("Cooldown UI")]
    [SerializeField] private TowerCooldownUI cooldownUI;
    
    protected Enemy CurrentEnemy;
    protected WaitForSeconds AttackWait;

    protected virtual void Awake()
    {
        AttackWait = new WaitForSeconds(attackInterval);
        if (towerHead == null) towerHead = transform;

        cooldownUI?.SetCooldown(1.0f);
    }

    protected virtual void Start()
    {
        StartCoroutine(AttackCo());
    }

    protected virtual void Update()
    {
        FindTarget();
        RotatetTarget();
    }

    protected virtual void FindTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(
            towerHead.position,
            attackRange,
            enemyLayer
            );
        float nearestDistance = float.MaxValue;
        Enemy nearestEnemy = null;
        foreach (Collider enemyCollider in enemies)
        {
            if (enemyCollider.TryGetComponent(out Enemy enermy))
            {
                float distance = Vector3.Distance(towerHead.position, enermy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enermy;
                }
            }
        }
        CurrentEnemy = nearestEnemy;
    }

    protected virtual void RotatetTarget()
    {
        if (CurrentEnemy == null) return;
        
        Vector3 direction = CurrentEnemy.transform.position - towerHead.position;
        direction.y = 0;
        
        if(direction == Vector3.zero) return;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        
        towerHead.rotation = Quaternion.Slerp(towerHead.rotation,lookRotation,Time.deltaTime * 5.0f);
    }

    protected virtual IEnumerator AttackCo() 
    {
        // 공격 전용 코루틴 
        while (true)
        {
            if (CurrentEnemy == null) // 공격 대상이 없음.
            {
                cooldownUI?.SetCooldown(1.0f); // 쿨타임 꽉참
                yield return null; // 다음 프레임까지 대기
                continue; // while 처음으로 이동
            }

            Fire();

            float timer = 0.0f;
            cooldownUI?.SetCooldown(0.0f); // 발사 이후 시점 비율은 0%

            while (timer < attackInterval) // 공격 쿨타임 동안 UI 갱신
            {
                timer += Time.deltaTime; // 프레임 시간 만큼 누적

                cooldownUI?.SetCooldown(timer / attackInterval);

                yield return null;
            }
            cooldownUI?.SetCooldown(1.0f); // 다시 발사 가능 상태로 원복.
        }
    }

    protected virtual void Fire()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bulletObj.TryGetComponent(out NormalBaseBullet bullet))
        {
            bullet.Initialize(CurrentEnemy, damage, bulletSpeed);
        }
    }
    public void ApplyBuff(int damageAdd, float intervalSub)
    {
        damage += damageAdd;
        attackInterval -= intervalSub;
        attackInterval = Mathf.Max(0.1f, attackInterval); // 최소 공격 간격 제한
        AttackWait = new WaitForSeconds(attackInterval); // 대기 시간 갱신
    }

    public void RemoveBuff(int damageSub, float intervalAdd)
    {
        damage -= damageSub;
        attackInterval += intervalAdd;
        AttackWait = new WaitForSeconds(attackInterval); // 대기 시간 갱신
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
