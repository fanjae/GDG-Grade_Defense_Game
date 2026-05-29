using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
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

        if (distance < 0.2f )
        {
            currentWayPointIndex++;
            
            if(currentWayPointIndex >= pathManager.WayPointCount)
            {
                EndPoint();
            }
        }
    }

    public void TakeDamage( int damage )
    {
        currentHp -= damage;
        UpdateHpUI();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    //도트데미지
    public void TakeDotDamage(int dotDamage, float dotDuration, float dotInterval)
    {
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

        //1초간격으로 3초동안 때리기
        while (timer < dotDuration)
        {
            TakeDamage(dotDamage);
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
        Destroy(gameObject);
    }

    private void EndPoint()
    {
        //적이 끝까지 도달했을 때 라이프 감소?
        Destroy(gameObject);
    }

    private void UpdateHpUI()
    {
        if (hpSlider == null) return;
        hpSlider.value = (float)currentHp / maxHp;
    }
}
