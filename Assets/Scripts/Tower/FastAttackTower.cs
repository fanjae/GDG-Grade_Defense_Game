using UnityEngine;

public class FastAttackTower : NormalTowerBase
{
    private void Reset()
    {
        attackInterval = 0.3f; 
        damage = 1;
        attackRange = 5.0f;
        bulletSpeed = 15.0f; 
    }
}
