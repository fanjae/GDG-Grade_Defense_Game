using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform towerHead;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPreb;

    [Header("Attack Setting")]
    [SerializeField] private float attackRange = 5.0f;
    [SerializeField] private float attackInterval = 0.8f;

    [SerializeField] private LayerMask enemyLayer;

    private Enemy currentTarget;
    private WaitForSeconds attackWait;

    private Transform myTransform;

    private void Start()
    {
        StartCoroutine(AttackCo());
    }

    private void Awake()
    {
        attackWait = new WaitForSeconds(attackInterval);
        myTransform = transform;
    }
    private void Update()
    {
        FindTarget();
        RotateToTarget();
    }
    
    private void FindTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(
            myTransform.position, attackRange, enemyLayer);
        float nearestDistance = float.MaxValue;
        Enemy nearestEnemy = null;
        foreach(Collider enemyCollider in enemies)
        {
            if(enemyCollider.TryGetComponent(out Enemy enemy))
            {
                float distance = Vector3.Distance(myTransform.position, enemy.transform.position);
                if(distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        currentTarget = nearestEnemy;
    }

    private void RotateToTarget()
    {
        if (currentTarget == null) return;

        Vector3 dir = currentTarget.transform.position - towerHead.position;
        dir.y = 0.0f;

        if (dir == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        towerHead.rotation = Quaternion.Slerp(towerHead.rotation, targetRot, Time.deltaTime * 8.0f);
    }

    IEnumerator AttackCo()
    {
        while (true)
        {
            if(currentTarget!=null)
            {
                Fire();
            }
            yield return attackWait;
        }
    }

    private void Fire()
    {
        GameObject bulletObj = Instantiate(bulletPreb, firePoint.position, firePoint.rotation);

        if (bulletObj.TryGetComponent(out Bullet bullet))
        {
            bullet.SetTarget(currentTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
