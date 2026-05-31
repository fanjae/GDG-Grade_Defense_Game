using System.Collections;
using UnityEngine;

public class StealthEnemy : Enemy
{
    [Header("은신 설정")]
    [Tooltip("모습이 보이는 시간")]
    [SerializeField] private float visibleTime = 4.0f;
    [Tooltip("은신하는 시간")]
    [SerializeField] private float stealthTime = 2.0f;

    [Header("레이어 설정")]
    [SerializeField] private string enemyLayerName = "Enemy";
    [SerializeField] private string stealthLayerName = "Default";

    //투명도 알파값
    [SerializeField] private float stealthAlpha = 0.5f;

    private Renderer enemyRenderer;

    private int enemyLayer;
    private int stealthLayer;

    protected override void Reset()
    {
        moveSpeed = 4.0f;
        maxHp = 10;
        coinValue = 5;
    }
    protected override void Start()
    {
        base.Start();
        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        stealthLayer = LayerMask.NameToLayer(stealthLayerName);

        StartCoroutine(StealthRoutine());
    }

    private IEnumerator StealthRoutine()
    {
        while (true)
        {
            //비은신 상태
            gameObject.layer = enemyLayer;
            SetAlpha(1.0f);

            yield return new WaitForSeconds(visibleTime);

            //은신 상태
            gameObject.layer = stealthLayer;
            SetAlpha(stealthAlpha);

            yield return new WaitForSeconds(stealthTime);
        }
    }
    protected void SetAlpha(float alphaValue)
    {
        Color color = enemyRenderer.material.color;
        color.a = alphaValue;
        enemyRenderer.material.color = color;
    }
}
