
using System.Collections;
using UnityEngine;

public class DotTower : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("회전하고 총알이 나갈 위치")]
    [SerializeField] private Transform towerHead;
    [Tooltip("총알 생성 위치")]
    [SerializeField] private Transform firePoint;
    [Tooltip("총알 프리펩")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("공격 세팅")]
    [Tooltip("공격 사거리")]
    [SerializeField] protected float attackRange = 5.0f;
    [Tooltip("공격 간겨 작을 수록 빠른 공격")]
    [SerializeField] protected float attackInterval = 1.5f;
    [Tooltip("한발당 데미지")]
    [SerializeField] protected int damage = 2;
    [Tooltip("총알 발사 속도 ")]
    [SerializeField] protected float bulletSpeed = 10.0f;

    [Tooltip("적 판별 레이어 설정")]
    [SerializeField] protected LayerMask enemyLayer;

    protected Enemy currentEnermy;
    protected WaitForSeconds attackWait;

    protected virtual void Awake()
    {
        attackWait = new WaitForSeconds(attackInterval);
        if (towerHead == null) towerHead = transform;
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
        currentEnermy = nearestEnemy;
    }

    protected virtual void RotatetTarget()
    {
        if (currentEnermy == null) return;

        Vector3 direction = currentEnermy.transform.position - towerHead.position;
        direction.y = 0;

        if (direction == Vector3.zero) return;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        towerHead.rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    protected virtual IEnumerator AttackCo()
    {
        while (true)
        {
            if (currentEnermy != null)
            {
                Fire();
            }

            yield return attackWait;
        }
    }

    protected virtual void Fire()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bulletObj.TryGetComponent(out DotBullet bullet))
        {
            bullet.Initialize(currentEnermy, damage, bulletSpeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
