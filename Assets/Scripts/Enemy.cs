using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 몬스터가 사망하거나 끝점에 도달해서 제거 되는 것을 외부에 알려주기 위한 Event
    public event Action<Enemy> onRemoved;

    [Header("Move Setting")]
    [SerializeField] private float moveSpeed = 2.5f;
    [Header("Hp Setting")]
    [SerializeField] private int maxHp = 3;
    [Header("Score Setting")]
    [SerializeField] private int scoreValue = 10;

    [Header("Hp UI")]
    [SerializeField] private Slider hpSlider;

    private int currentHp;
    private int currentWayPointIndex;
    private PathManager pathManager;

    private Coroutine dotDamageCo;

    // 제거된 Enemy인지 확인(이벤트 중복처리 방지)
    private bool isRemoved;

    private void Start()
    {
        currentHp = maxHp; // 현재 체력을 최대 체력으로 초기화
        currentWayPointIndex = 0;
        pathManager = FindAnyObjectByType<PathManager>();
        UpdateHpUI();
    }

    private void Update()
    {
        MovePath();
    }
    private void MovePath()
    {
        if (pathManager == null) return;

        Transform targetWayPoint = pathManager.GetWayPoint(currentWayPointIndex);

        if (targetWayPoint == null) return;

        Vector3 dir = targetWayPoint.position - transform.position;

        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        transform.LookAt(targetWayPoint);

        float distance = Vector3.Distance(transform.position, targetWayPoint.position);

        // 목표 웨이포인트 도착하면 다음 웨이포인트로 이동
        if (distance < 0.2f )
        {
            currentWayPointIndex++;
            
            // 마지막 웨이포인트 도달시 끝점 도달(적 사망 처리)
            if(currentWayPointIndex >= pathManager.WayPointCount)
            {
                EndPoint();
            }
        }
    }

    public void TakeDamage( int damage )
    {
        if (isRemoved) return; 

        currentHp -= damage;
        UpdateHpUI();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    //도트 데미지 적용
    public void TakeDotDamage(int dotDamage, float dotDuration, float dotInterval)
    {
        // 제거된 적이 추가 데미지 받지 않게 처리
        if (isRemoved) return; 

        if (dotDamageCo != null)
        {
            //이미 도트데미지를 맞고있다면 기존 도트데미지 중단
            StopCoroutine(dotDamageCo);
        }
        //새 도트데미지 시작
        dotDamageCo = StartCoroutine((DotDamageCo(dotDamage, dotDuration, dotInterval)));
    }

    IEnumerator DotDamageCo(int dotDamage, float dotDuration, float dotInterval)
    {
        float timer = 0.0f;

        // dotInterval 간격으로 dotDuration 동안 데미지 적용.
        while (timer < dotDuration)
        {
            TakeDamage(dotDamage);

            // 도트 데미지로 사망시 코루틴 종료.
            if (isRemoved) yield break;

            yield return new WaitForSeconds(dotInterval);
            timer += dotInterval;
        }
        dotDamageCo = null;
    }

    private void Die()
    {
        //적이 죽었을 때 점수를 올리고 파괴
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        RemoveEnemy();
    }

    private void EndPoint()
    {
        //적이 끝까지 도달했을 때 라이프 감소?
        Destroy(gameObject);
    }

    private void RemoveEnemy()
    {
        if (isRemoved) return;

        isRemoved = true;

        onRemoved?.Invoke(this);
        Destroy(gameObject);
    }

    private void UpdateHpUI()
    {
        if (hpSlider == null) return;
        hpSlider.value = (float)currentHp / maxHp;
    }
}
