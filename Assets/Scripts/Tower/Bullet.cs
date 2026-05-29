using System;
using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3.0f;

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
            enermy.TakeDamage(damage);
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
        this.damage = damage;
        this.moveSpeed = moveSpeed;
    }
}
