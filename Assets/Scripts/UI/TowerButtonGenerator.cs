using UnityEngine;

public class TowerButtonGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] towerPrefabs;
    public GameObject[] TowerPrefabs { get; }

    [SerializeField]
    private GameObject towerButton;

    private void Awake()
    {
        LoadTowerButtonObject();
    }

    private void LoadTowerButtonObject()
    {
        // 타워 프리팹이 없으면 반환
        if (towerPrefabs.Length == 0) return;

        // 타워 프리팹 개수만큼 버튼 오브젝트 생성
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            GameObject towerButtonObject = Instantiate(towerButton, transform);

            // 그 버튼 오브젝트가 가질 타워 프리팹 정보를 세팅함
            if(towerButtonObject.TryGetComponent(out TowerBtnUI towerButtonUI))
            {
                towerButtonUI.SetTowerPrefab(towerPrefabs[i]);
            }
        }
    }
}
