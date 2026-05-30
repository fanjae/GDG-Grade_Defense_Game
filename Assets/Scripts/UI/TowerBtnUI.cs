using UnityEngine;
using UnityEngine.UI;

public class TowerBtnUI : MonoBehaviour
{
    private Button towerBtn;
    private GameObject towerPrefab;
    private TowerBuilder towerBuilder;

    private void Awake()
    {
        // TowerBuilder 오브젝트 하이어라키 창에서 찾아서 TowerBuilder 스크립트 컴포넌트를 가져옴
        towerBuilder = GameObject.Find("TowerBuilder").GetComponent<TowerBuilder>();
        
        // towerButton UI 오브젝트의 Button 컴포넌트 가져옴
        towerBtn = GetComponent<Button>();
    }

    private void Start()
    {
        // 타워 버튼을 눌렀을 때 누른 버튼에 맞는 타워 프리팹을 TowerBuilder에 전달하는 기능
        towerBtn.onClick.AddListener(OnClickTowerBtn);
    }

    private void OnClickTowerBtn()
    {
        // TowerBulider에 타워 프리팹 저장
        towerBuilder.TowerPrefab = towerPrefab;
    }

    public void SetTowerPrefab(GameObject towerPrefabObject)
    {
        // TowerButtonenerator 스크립트에서 받은 프리펩을 towerPrefab 변수에 저장
        towerPrefab = towerPrefabObject;
    }
}