
using System.Collections;
using UnityEngine;

public class SplashTower : NormalTowerBase
{
    protected override void Fire()
    {
        //SplashBullet 날리기
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bulletObj.TryGetComponent(out SplashBullet bullet))
        {
            bullet.Initialize(CurrentEnemy, damage, bulletSpeed);
        }
    }
}
