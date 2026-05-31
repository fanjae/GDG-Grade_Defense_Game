using UnityEngine;

public class ArmorEnemy : Enemy
{
    private int armorValue = 3;
    protected override void Reset()
    {
        moveSpeed = 1.5f;
        maxHp = 50;
        coinValue = 20;
    }
    public override void TakeDamage(int damage)
    {
        if (armorValue > damage)
        {
            return;
        }
        base.TakeDamage(damage);
    }
}