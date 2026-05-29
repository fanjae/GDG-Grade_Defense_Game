using UnityEngine;

public class StrongDamageTower : NormalTowerBase
{
    private void Reset()
    {
        attackInterval = 1.5f; 
        damage = 5;            
        attackRange = 5.0f;
        bulletSpeed = 8.0f;    
    }
}
