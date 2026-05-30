using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private MapManager mapManager;  // 같은 인스펙터 창에 있어서 MapManager를 가져오기 위한 변수

    [SerializeField]
    private GameObject[] mapObjects;    // 프리펩에 있는 BulidTile, Road 오브젝트를 넣어줌

    [SerializeField]
    private Vector3 startPos = Vector3.zero;    // 시작위치를 지정해주면 그 위치 기준으로 오브젝트가 나옴

    private void Awake()
    {
        mapManager = GetComponent<MapManager>();
    }

    private void Start()
    {
        GenerateMap();  // 맵 생성하기
    }

    private void GenerateMap()
    {
        for (int i = 0; i < mapManager.MapDatas.Count; i++) // 딕셔너리에 있던 키의 개수만큼 for문을 돌림
        {
            List<MapType> mapTypes = mapManager.MapDatas[i];    // 딕셔너리에 담아둔 리스트를 가져옴

            for (int j = 0; j < mapTypes.Count; j++)    // for문으로 리스트에 있는 값을 하나씩 가져옴
            {
                float posY = 0.0f;

                if (mapTypes[j] == MapType.BuildTile)   // 높낮이를 다르게 하기위해 구분함
                {
                    posY = 0.2f;
                }
                if (mapTypes[j] == MapType.Road)
                {
                    posY = 0.5f;
                }

                Vector3 pos = startPos + new Vector3((j * 2), posY, (i * -2));  // 2, -2는 큐브라서 위치를 잡기 위한것임
                Instantiate(mapObjects[(int)mapTypes[j]], pos, Quaternion.identity, transform); // MapData에 맞는 오브젝트를 해당 위치에 설치함
            }
        }
    }

}
