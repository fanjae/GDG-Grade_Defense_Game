using UnityEngine;

public class SmallEnemy : Enemy
{
    protected override void Reset()
    {
        moveSpeed = 3.0f;
        maxHp = 5;
        coinValue = 2;
    }
}
