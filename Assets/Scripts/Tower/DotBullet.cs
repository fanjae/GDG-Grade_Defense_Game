using UnityEngine;

public class DotBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float lifetime = 3.0f;

    [Tooltip("도트데미지")]
    [SerializeField] private int dotDamage = 2;
    [Tooltip("도트 지속시간")]
    [SerializeField] private float dotDuration = 3.0f;
    [Tooltip("도트 데미지 간격")]
    [SerializeField] private float dotInterval = 1.0f;

    private Enemy _target;
    private Rigidbody _rb;
    private Transform _targetTransform;
    private Transform _myTransform;

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
            //3초동안 1초간격으로 2데미지 
            enermy.TakeDotDamage(dotDamage, dotDuration, dotInterval);   
            Destroy(gameObject);
        }
    }

    public void Initialize(Enemy target, int damage, float moveSpeed)
    {
        _target = target;
        if (target != null)
        {
            _targetTransform = target.transform;
        }
        this.dotDamage = damage;
        this.moveSpeed = moveSpeed;
    }
}
