using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3.0f;

    private Enemy target;
    private Rigidbody rb;
    private Transform targetTransform;
    private Transform myTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myTransform = transform;
    }
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if(target == null || targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 targetPos = targetTransform.position + Vector3.up * 0.5f;
        Vector3 dir = targetPos - myTransform.position;

        rb.linearVelocity = dir.normalized * moveSpeed;
        myTransform.forward = dir.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
        if(target!=null)
        {
            targetTransform = target.transform;
        }
    }
}
