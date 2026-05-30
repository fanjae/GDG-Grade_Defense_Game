using UnityEngine;

public class BigEnemy : Enemy
{
    private void Reset()
    {
        moveSpeed = 1.0f;
        maxHp = 200;
        coinValue = 100;
    }
}
