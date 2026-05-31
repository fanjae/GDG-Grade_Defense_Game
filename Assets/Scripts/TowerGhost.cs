using UnityEngine;

public class TowerGhost : MonoBehaviour
{
    private Camera cam;
    private TowerBuilder towerBuilder;
    private void Awake()
    {
        // TowerBuilder 오브젝트 하이어라키 창에서 찾아서 TowerBuilder 스크립트 컴포넌트를 가져옴
        towerBuilder = GameObject.Find("TowerBuilder").GetComponent<TowerBuilder>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("BuildTile")))
        {
            Collider hitCollider = hit.collider;

            Vector3 hitColiderCenterPos = hitCollider.transform.position;
            hitColiderCenterPos.y = 1.2f;

            transform.position = hitColiderCenterPos;

            if (Input.GetMouseButtonDown(0))
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetTowerBuilderPrefabInfo(GameObject towerPrefab)
    {
        // TowerBulider에 타워 프리팹 저장
        towerBuilder.TowerPrefab = towerPrefab;
    }
}