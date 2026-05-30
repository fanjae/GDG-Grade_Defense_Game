using UnityEngine;

public class DotTower : NormalTowerBase
{
    protected override void Fire()
    {
        //DotBullet 날리기
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bulletObj.TryGetComponent(out DotBullet bullet))
        {
            bullet.Initialize(CurrentEnemy, damage, bulletSpeed);
        }
    }
}
