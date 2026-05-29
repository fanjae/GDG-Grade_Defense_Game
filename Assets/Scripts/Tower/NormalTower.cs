using UnityEngine;

public class NormalTower : NormalTowerBase
{
    private void Reset()
    {
        attackInterval = 0.5f; 
        damage = 1;
        attackRange = 5.0f;
        bulletSpeed = 15.0f; 
    }
}
