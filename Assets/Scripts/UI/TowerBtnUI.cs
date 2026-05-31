using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtnUI : MonoBehaviour
{
    private Image towerImage;
    private Button towerBtn;
    private TextMeshProUGUI towerName;

    private GameObject towerPrefab;

    private TowerBuilder towerBuilder;
    
    private void Awake()
    {
        // TowerBuilder 오브젝트 하이어라키 창에서 찾아서 TowerBuilder 스크립트 컴포넌트를 가져옴
        towerBuilder = GameObject.Find("TowerBuilder").GetComponent<TowerBuilder>();
        
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
        // TowerBulider에 타워 프리팹 저장
        towerBuilder.TowerPrefab = towerPrefab;
    }

    public void SetTowerPrefab(GameObject towerPrefabObject, Sprite sprite, string name)
    {
        // TowerData에 있는 정보를 받아와서 저장
        towerPrefab = towerPrefabObject;
        towerImage.sprite = sprite;
        towerName.text = name;  
    }
}