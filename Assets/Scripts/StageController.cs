using System.Collections;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Stage Waves")]
    [SerializeField] private StageData[] stages = new StageData[5];

    [Header("Build UI")]
    [SerializeField] private GameObject buildUI; // 빌드 UI

    [Header("TowerBuilder")]
    [SerializeField] private TowerBuilder towerBuilder;

    [Header("Spawn Point")]
    [SerializeField] private GameObject spawnPointImage; // 몬스터 스폰 지점

    private int currentStageIndex;
    private int currentWaveIndex;

    // Build UI의 버튼이 눌렀는지 확인하는 값
    private bool isNextWaveButtonClicked;

    private IEnumerator Start()
    {
        if (buildUI != null)
            buildUI.SetActive(false);

        if (towerBuilder != null) // 타워 빌더도 웨이브 도중에 지을 수 없음
            towerBuilder.CanBuild = false;

        if (spawnPointImage != null)
            spawnPointImage.SetActive(false);

        currentStageIndex = 0;

        // 게임 시작 직후, 첫 웨이브 전에 타워 설치
        yield return StartCoroutine(EnterBuildTime());

        // 등록된 스테이지를 순서대로 실행
        while (currentStageIndex < stages.Length && !GameManager.Instance.IsGameOver)
        {
            StageData currentStage = stages[currentStageIndex];

            // 현재 스테이지의 모든 웨이브 진행
            yield return StartCoroutine(PlayStage(currentStage));

            if (GameManager.Instance.IsGameOver) 
                yield break;

            currentStageIndex++;
        }

        Debug.Log("All Stages Clear");
    }

    private IEnumerator PlayStage(StageData stageData)
    {
        if (stageData == null || stageData.Waves == null)
            yield break;

        currentWaveIndex = 0;

        // 현재 스테이지 안의 웨이브를 순서대로 실행
        while (currentWaveIndex < stageData.Waves.Length && !GameManager.Instance.IsGameOver)
        {
            WaveData currentWave = stageData.Waves[currentWaveIndex];

            // 현재 웨이브의 모든 적 제거 대기
            yield return StartCoroutine(enemySpawner.SpawnWave(currentWave));

            currentWaveIndex++;

            // 다음 웨이브 시작 전, 타워 건설 시간 진입.
            if (currentWaveIndex < stageData.Waves.Length && !GameManager.Instance.IsGameOver)
            {
                yield return StartCoroutine(EnterBuildTime());
            }
        }

        Debug.Log($"Stage {currentStageIndex + 1} Clear");
    }

    private IEnumerator EnterBuildTime()
    {
        // 버튼 클릭 상태 초기화 
        isNextWaveButtonClicked = false;

        // Build UI 활성화
        if (buildUI != null)
            buildUI.SetActive(true);

        // 타워 지을땐 몬스터 스폰 지점 보이게 설정
        if (spawnPointImage != null)
            spawnPointImage.SetActive(true);

        // 타워 빌더 지을 수 있게 변경
        if (towerBuilder != null)
            towerBuilder.CanBuild = true;

        // 버튼 입력 대기
        yield return new WaitUntil(() => isNextWaveButtonClicked);

        if (towerBuilder != null)
            towerBuilder.CanBuild = false;

        // 버튼이 눌리면 Build UI 비활성화        
        if (buildUI != null)
            buildUI.SetActive(false);

        // 웨이브 시작하면 다시 스폰 지점 보이는 것 다시 비활성화 
        if (spawnPointImage != null)
            spawnPointImage.SetActive(false);

    }

    // 다음 웨이브 시작 버튼 OnClick에 연결할 메서드
    public void OnClickNextWaveButton()
    {
        isNextWaveButtonClicked = true;
    }
}