using UnityEngine;

public class LongRangeTower : NormalTowerBase
{
    private void Reset()
    {
        attackInterval = 1.0f;
        damage = 2;
        attackRange = 10.0f;    
        bulletSpeed = 20.0f;    
    }
}
