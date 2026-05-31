using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private LayerMask buildTowerlayer;

    public GameObject TowerPrefab { get; set; }

    private Camera mainCam;

    private Renderer hitRenderer;
    private Color originColor;

    public bool CanBuild { get; set; } // 타워 지을 수 있는 상태 판단

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!CanBuild) // 웨이브 도중에는 비활성화 처리 (지을 수 없음)
            return ;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (TowerPrefab != null && Input.GetMouseButtonDown(0))
        {
            BuildTower();
        }

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, buildTowerlayer))
        {
            if (hit.collider.TryGetComponent(out Renderer renderer))
            {
                if (hitRenderer != null && hitRenderer != renderer)
                {
                    hitRenderer.material.color = originColor;
                }

                if (hitRenderer != renderer)
                {
                    hitRenderer = renderer;
                    originColor = renderer.material.color;
                    hitRenderer.material.color = Color.orange;
                }
                return;
            }
        }

        if (hitRenderer != null)
        {
            hitRenderer.material.color = originColor;

            hitRenderer = null;
        }
    }
    
    private void BuildTower()
    {
        if (mainCam == null) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, buildTowerlayer))
        {
            if(hit.collider.TryGetComponent(out BuildTile buildTile))
            {
                if(buildTile.CanBuild()==false)
                {
                    return;
                }

                Vector3 buildPosition = hit.collider.transform.position;
                buildPosition.y = 1.2f;

                Instantiate(TowerPrefab, buildPosition, Quaternion.identity);
                TowerPrefab = null;

                buildTile.SetBuild();
            }
        }
    }
}
