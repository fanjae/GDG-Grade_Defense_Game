using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtnUI : MonoBehaviour
{
    private Image towerImage;
    private Button towerBtn;
    private TextMeshProUGUI towerName;

    private GameObject towerPrefab;
    private GameObject towerGhostPrefab;

    private void Awake()
    {
        // towerButton UI 오브젝트의 Button 컴포넌트 가져옴
        towerBtn = GetComponent<Button>();

        towerImage = GetComponent<Image>();
        towerName = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        // 타워 버튼을 눌렀을 때 누른 버튼에 맞는 타워 프리팹을 TowerBuilder에 전달하는 기능
        towerBtn.onClick.AddListener(OnClickTowerBtn);
    }

    private void OnClickTowerBtn()
    {
        // 고스트 프리팹 생성하기
        GameObject towerGhostObj = Instantiate(towerGhostPrefab, Vector3.zero, Quaternion.identity);
    
        // 고스트 프리팹 스크립트에서 실제 프리팹 데이터 저장하기
        if(towerGhostObj.TryGetComponent(out TowerGhost towerGhost))
        {
            towerGhost.SetTowerBuilderPrefabInfo(towerPrefab);
        }
        else
        {
            print("고스트 오브젝트 실제 프리팹 데이터 못넣음");
        }
    }

    public void SetTowerPrefab(GameObject towerPrefabObject, GameObject towerGhostPrefabObject, Sprite sprite, string name)
    {
        // TowerData에 있는 정보를 받아와서 저장
        towerPrefab = towerPrefabObject;
        towerGhostPrefab = towerGhostPrefabObject;
        towerImage.sprite = sprite;
        towerName.text = name;  
    }
}