using System.Collections;
using UnityEngine;

public class StealthEnemy : Enemy
{
    [Header("은신 설정")]
    [Tooltip("모습이 보이는 시간")]
    [SerializeField] private float visibleTime = 4.0f;
    [Tooltip("은신하는 시간")]
    [SerializeField] private float stealthTime = 2.0f;

    [Header("레이어 이름 설정")]
    [SerializeField] private string enemyLayerName = "Enemy";
    [SerializeField] private string stealthLayerName = "Default";

    private int enemyLayer;
    private int stealthLayer;

    private Renderer enemyRenderer;

    protected void Start()
    {
        enemyRenderer = GetComponent<Renderer>();

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
            enemyRenderer.enabled = true;

            yield return new WaitForSeconds(visibleTime);

            //은신 상태
            gameObject.layer = stealthLayer;
            enemyRenderer.enabled = false;

            yield return new WaitForSeconds(stealthTime);
        }
    }
}
