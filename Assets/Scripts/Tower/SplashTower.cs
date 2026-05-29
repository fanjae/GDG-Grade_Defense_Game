
using System.Collections;
using UnityEngine;

public class SplashTower : MonoBehaviour
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
    [SerializeField] private float attackRange = 5.0f;
    [Tooltip("공격 간격 작을 수록 빠른 공격")]
    [SerializeField] private float attackInterval = 2f;
    [Tooltip("한발당 데미지")]
    [SerializeField] private int damage = 5;
    [Tooltip("총알 발사 속도 ")]
    [SerializeField] private float bulletSpeed = 10.0f;

    [Tooltip("적 판별 레이어 설정")]
    [SerializeField] private LayerMask enemyLayer;

    private Enemy currentEnemy;
    private WaitForSeconds attackWait;

    private void Awake()
    {
        attackWait = new WaitForSeconds(attackInterval);
        if (towerHead == null) towerHead = transform;
    }

    private void Start()
    {
        StartCoroutine(AttackCo());
    }

    private void Update()
    {
        FindTarget();
        RotateTarget();
    }

    private void FindTarget()
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
            if (enemyCollider.TryGetComponent(out Enemy enemy))
            {
                float distance = Vector3.Distance(towerHead.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        currentEnemy = nearestEnemy;
    }

    private void RotateTarget()
    {
        if (currentEnemy == null) return;

        Vector3 direction = currentEnemy.transform.position - towerHead.position;
        direction.y = 0;

        if (direction == Vector3.zero) return;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        towerHead.rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    private IEnumerator AttackCo()
    {
        while (true)
        {
            if (currentEnemy != null)
            {
                Fire();
            }

            yield return attackWait;
        }
    }

    private void Fire()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bulletObj.TryGetComponent(out SplashBullet bullet))
        {
            bullet.Initialize(currentEnemy, damage, bulletSpeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
