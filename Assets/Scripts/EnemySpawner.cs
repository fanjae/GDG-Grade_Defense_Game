using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Enemy Position")]
    [SerializeField] private Transform spawnPoint;

    // 현재 웨이브에서 살아있는 적의 수
    private int aliveEnemyCount; 

    public IEnumerator SpawnWave(WaveData waveData)
    {
        // 웨이브 시작할 때 생존 적 수를 0으로 재설정
        aliveEnemyCount = 0; 

        if (spawnPoint == null) yield break;
        if (waveData == null || waveData.spawnInfos == null) yield break;

        // 웨이브에 등록된 몬스터 그룹을 순서대로 스폰한다.
        foreach (EnemySpawnInfo spawnInfo in waveData.spawnInfos)
        {
            if (spawnInfo.enemyPrefab == null) continue;

            WaitForSeconds wait = new WaitForSeconds(spawnInfo.interval);

            // 설정된 수만큼 몬스터를 생성
            for(int i = 0; i < spawnInfo.count; i++)
            {
                GameObject enemyObj = Instantiate(spawnInfo.enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                // 생성된 Enemy의 제거 이벤트 구독
                if(enemyObj.TryGetComponent(out Enemy enemy))
                {
                    aliveEnemyCount++;
                    enemy.onRemoved += HandleEnemyRemoved;
                }

                yield return wait;
            }
        }

        // 몬스터 스폰이 끝난 뒤, 현재 웨이브의 모든 적이 제거될 때까지 대기
        yield return new WaitUntil(() => aliveEnemyCount <= 0);
    }
    private void HandleEnemyRemoved(Enemy enemy)
    {
        // 이벤트 중복 호출 방지를 위해 구독 해제 및 살아있는 적의 수 감소
        enemy.onRemoved -= HandleEnemyRemoved;
        aliveEnemyCount--;
    }
}
