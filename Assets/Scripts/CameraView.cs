using System;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    [Header("카메라 위치")]
    [SerializeField]
    private Vector3[] cameraViewPos;

    [Header("카메라 회전")]
    [SerializeField]
    private Vector3[] cameraViewRot;

    int index = 0;

    private void Awake()
    {
        transform.position = cameraViewPos[index];
        transform.rotation = Quaternion.Euler(cameraViewRot[index]);
    }

    private void Update()
    {
        // 왼쪽으로 카메라 시점 변경
        if(Input.GetKeyDown(KeyCode.Q))
        {
            index--;

            if(index < 0)
            {
                index = cameraViewPos.Length - 1;
            }

            transform.position = cameraViewPos[index];
            transform.rotation = Quaternion.Euler(cameraViewRot[index]);
        }

        // 오른쪽으로 카메라 시점 변경
        if (Input.GetKeyDown(KeyCode.E))
        {
            index++;

            if (index > cameraViewPos.Length - 1)
            {
                index = 0;
            }

            transform.position = cameraViewPos[index];
            transform.rotation = Quaternion.Euler(cameraViewRot[index]);
        }
    }
}
