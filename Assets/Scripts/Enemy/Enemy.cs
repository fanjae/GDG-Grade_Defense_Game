using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 몬스터가 사망하거나 끝점에 도달해서 제거 되는 것을 외부에 알려주기 위한 Event
    public event Action<Enemy> onRemoved;

    [Header("Move Setting")]
    [SerializeField] protected float moveSpeed;
    [Header("Hp Setting")]
    [SerializeField] protected int maxHp;
    [Header("Gold Setting")]
    [SerializeField] protected int coinValue;

    [Header("Hp UI")]
    [SerializeField] protected Slider hpSlider;

    private int currentHp;
    private int currentWayPointIndex;
    private PathManager pathManager;

    //슬로우용 원본 스피드 저장
    protected float originSpeed;
    public int slowCount;

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
        if (distance < 0.2f)
        {
            currentWayPointIndex++;

            // 마지막 웨이포인트 도달시 끝점 도달(적 사망 처리)
            if (currentWayPointIndex >= pathManager.WayPointCount)
            {
                EndPoint();
            }
        }
    }

    public void TakeDamage(int damage)
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

    //슬로우
    public void GetSlow(float amount)
    {
        originSpeed = moveSpeed;
        moveSpeed = moveSpeed / 100 * amount;
        slowCount++;
    }
    public void DispelSlow()
    {
        slowCount--;
        if (slowCount == 0)
        { moveSpeed = originSpeed; }
    }

    private void Die()
    {
        //적이 죽었을 때 코인을 지급하고 파괴
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoin(coinValue);
        }

        RemoveEnemy();
    }

    private void EndPoint()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreaseLife(1);
        }

        RemoveEnemy();
    }

    private void RemoveEnemy()
    {
        // 중복 제거 방지
        if (isRemoved) return;

        isRemoved = true;

        // Enemy 제거를 알림(Spawner에게 통보)
        onRemoved?.Invoke(this);

        Destroy(gameObject);
    }

    private void UpdateHpUI()
    {
        if (hpSlider == null) return;
        hpSlider.value = (float)currentHp / maxHp;
    }
}
