using UnityEngine;

public class TowerButtonGenerator : MonoBehaviour
{
    [SerializeField]
    private TowerData towerData;

    [SerializeField]
    private GameObject towerButton;

    private void Awake()
    {
        LoadTowerButtonObject();
    }

    private void LoadTowerButtonObject()
    {
        // 타워 프리팹이 없으면 반환
        if (towerData.towerPrefabs.Length == 0) return;

        // 타워 프리팹 개수만큼 버튼 오브젝트 생성
        for (int i = 0; i < towerData.towerPrefabs.Length; i++)
        {
            GameObject towerButtonObject = Instantiate(towerButton, transform);

            // 그 버튼 오브젝트가 가질 타워 프리팹 정보를 세팅함
            if(towerButtonObject.TryGetComponent(out TowerBtnUI towerButtonUI))
            {
                towerButtonUI.SetTowerPrefab(towerData.towerPrefabs[i], towerData.towerGhostPrefabs[i], towerData.icons[i], towerData.towerNames[i]);
            }
        }
    }
}