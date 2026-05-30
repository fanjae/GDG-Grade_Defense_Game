using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Enemy Position")]
    [SerializeField] private Transform spawnPoint;

    // 현재 웨이브에서 살아있는 적의 수
    private int aliveEnemyCount;
    private int runningSpawnGroupCount; // Spawn Group

    public IEnumerator SpawnWave(WaveData waveData)
    {
        aliveEnemyCount = 0;
        runningSpawnGroupCount = 0;

        if (spawnPoint == null) yield break;
        if (waveData == null) yield break;
        if (waveData.SpawnInfos == null || waveData.SpawnInfos.Length == 0) yield break;

        // 웨이브에 등록된 몬스터 그룹 동시 스폰
        foreach (EnemySpawnInfo spawnInfo in waveData.SpawnInfos)
        {
            if (spawnInfo.EnemyPrefab == null) continue;

            runningSpawnGroupCount++;
            StartCoroutine(SpawnGroup(spawnInfo));
        }

        // 모든 스폰 그룹의 스폰이 끝날 때까지 대기
        yield return new WaitUntil(() => runningSpawnGroupCount <= 0);

        // 스폰된 모든 적이 제거될 때까지 대기
        yield return new WaitUntil(() => aliveEnemyCount <= 0);
    }

    private IEnumerator SpawnGroup(EnemySpawnInfo spawnInfo)
    {
        // 해당 몬스터 그룹 스폰 간격
        WaitForSeconds wait = new WaitForSeconds(spawnInfo.Interval);

        // 설정된 수만큼 몬스터 생성
        for (int i = 0; i < spawnInfo.Count; i++)
        {
            Enemy enemy = Instantiate(spawnInfo.EnemyPrefab,spawnPoint.position,spawnPoint.rotation);

            // 생성된 적 count 계산 및 제거 이벤트 구독
            aliveEnemyCount++;
            enemy.onRemoved += HandleEnemyRemoved;

            yield return wait;
        }

        // 몬스터 그룹 스폰 끝
        runningSpawnGroupCount--;
    }

    // 제거된 적에 대한 이벤트 해제 및 생존 적 수 감소
    private void HandleEnemyRemoved(Enemy enemy)
    {
        enemy.onRemoved -= HandleEnemyRemoved;
        aliveEnemyCount--;
    }
}
