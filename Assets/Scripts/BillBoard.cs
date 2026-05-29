using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera targetCamera;
    private void Start()
    {
        targetCamera = Camera.main;
    }

    private void Update()
    {
        if (targetCamera == null) return;
        transform.rotation = Quaternion.LookRotation(
            targetCamera.transform.forward,
            targetCamera.transform.up);
    }

}
