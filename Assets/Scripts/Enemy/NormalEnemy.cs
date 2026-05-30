using UnityEngine;

public class NormalEnemy : Enemy
{
    private void Reset()
    {
        moveSpeed = 2.0f;
        maxHp = 20;
        coinValue = 10;
    }
}
