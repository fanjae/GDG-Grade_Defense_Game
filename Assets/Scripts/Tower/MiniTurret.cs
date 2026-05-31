using UnityEngine;

public class MiniTurret : NormalTowerBase
{
    [Header("Mini Turret Lifetime")]
    [SerializeField] private float lifetime = 10.0f;

    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, lifetime);
    }

    //미니 포탑 생성시 서머너 타워에서 데미지를 받아 오게 설정
    public void InitializeTurret(int damage, float range, float interval, float bSpeed, float life)
    {
        this.damage = damage;
        this.attackRange = range;
        this.attackInterval = interval;
        this.bulletSpeed = bSpeed;
        this.lifetime = life;
        

    }
}
