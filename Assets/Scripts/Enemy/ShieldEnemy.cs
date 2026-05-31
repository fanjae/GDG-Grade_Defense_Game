using UnityEngine;

public class ShieldEnemy : Enemy
{
    private int currentShield;
    private int maxShield = 5;
    protected override void Reset()
    {
        moveSpeed = 1.5f;
        maxHp = 50;
        coinValue = 20;
        currentShield = maxShield;
    }
    public override void TakeDamage(int damage)
    {
        if(currentShield > 0)
        {
            currentShield--;
            return;
        }
        base.TakeDamage(damage);
    }
}
