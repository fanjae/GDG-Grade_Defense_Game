using System.Collections;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Stage Waves"]
    [SerializeField] private StageData[] stages = new StageData[5];

    [Header("Build UI")]
    [SerializeField] private GameObject buildUI; // 빌드 UI

    private int currentStageIndex;
    private int currentWaveIndex;

    // Build UI의 버튼이 눌렀는지 확인하는 값
    private bool isNextWaveButtonClicked;

    private IEnumerator Start()
    {
        if (buildUI != null)
            buildUI.SetActive(false);

        currentStageIndex = 0;

        // 등록된 스테이지를 순서대로 실행
        while (currentStageIndex < stages.Length)
        {
            StageData currentStage = stages[currentStageIndex];

            // 현재 스테이지의 모든 웨이브 진행
            yield return StartCoroutine(PlayStage(currentStage));

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
        while (currentWaveIndex < stageData.Waves.Length)
        {
            WaveData currentWave = stageData.Waves[currentWaveIndex];

            // 현재 웨이브의 모든 적 제거 대기
            yield return StartCoroutine(enemySpawner.SpawnWave(currentWave));

            currentWaveIndex++;

            // 다음 웨이브가 남아있으면 Build UI 버튼 입력 대기
            if (currentWaveIndex < stageData.Waves.Length)
            {
                yield return StartCoroutine(WaitBuildUI());
            }
        }

        Debug.Log($"Stage {currentStageIndex + 1} Clear");
    }

    private IEnumerator WaitBuildUI()
    {
        // 버튼 클릭 상태 초기화 
        isNextWaveButtonClicked = false;

        // Build UI 활성화
        if (buildUI != null)
            buildUI.SetActive(true);

        // 버튼 입력 대기
        yield return new WaitUntil(() => isNextWaveButtonClicked);

        // 버튼이 눌리면 Build UI 비활성화        
        if (buildUI != null)
            buildUI.SetActive(false);
    }

    // 다음 웨이브 시작 버튼 OnClick에 연결할 메서드
    public void OnClickNextWaveButton()
    {
        isNextWaveButtonClicked = true;
    }
}