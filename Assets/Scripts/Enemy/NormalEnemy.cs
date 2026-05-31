using UnityEngine;

public class NormalEnemy : Enemy
{
    protected override void Reset()
    {
        moveSpeed = 2.0f;
        maxHp = 20;
        coinValue = 10;
    }
}
