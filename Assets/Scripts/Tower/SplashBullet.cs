using UnityEngine;

public class SplashBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float lifetime = 3.0f;

    [Tooltip("범위 데미지")]
    [SerializeField] private int splashDamage = 2;

    private Enemy _target;
    private Rigidbody _rb;
    private Transform _targetTransform;
    private Transform _myTransform;

    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _myTransform = transform;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (_target == null || _targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPos = _targetTransform.position + Vector3.up * 0.5f;
        Vector3 dir = targetPos - _myTransform.position;
        _rb.linearVelocity = dir.normalized * moveSpeed;
        _myTransform.forward = dir.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enermy))
        {
            TakeSplashDamage(enermy.transform.position, splashDamage);
            Destroy(gameObject);
        }
    }

    void TakeSplashDamage(Vector3 enemyPos, int splashDamage)
    {
        Collider[] hits = Physics.OverlapSphere(enemyPos, 3f, enemyLayer);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(splashDamage);
            }
        }
    }

    public void Initialize(Enemy target, int damage, float moveSpeed)
    {
        _target = target;
        if (target != null)
        {
            _targetTransform = target.transform;
        }
        this.splashDamage = damage;
        this.moveSpeed = moveSpeed;
    }
}
