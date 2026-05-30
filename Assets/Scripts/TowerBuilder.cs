using UnityEngine;
using UnityEngine.UIElements;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private LayerMask buildTowerlayer;

    public GameObject TowerPrefab { get; set; }

    private Camera mainCam;

    private Renderer hitRenderer;
    private Color originColor;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
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

                buildPosition.y = 0.5f;
                Instantiate(TowerPrefab, buildPosition, Quaternion.identity);

                buildTile.SetBuild();
            }
        }
    }
}
