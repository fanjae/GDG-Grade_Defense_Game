using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private MapManager mapManager;

    [SerializeField]
    private GameObject[] mapObjects;

    [SerializeField]
    private Vector3 startPos = Vector3.zero;

    private void Awake()
    {
        mapManager = GetComponent<MapManager>();
    }

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int i = 0; i < mapManager.MapDatas.Count; i++)
        {
            List<MapType> mapTypes = mapManager.MapDatas[i];

            for (int j = 0; j < mapTypes.Count; j++)
            {
                float posY = 0.0f;

                if (mapTypes[j] == MapType.BuildTile)
                {
                    posY = 0.2f;
                }
                if (mapTypes[j] == MapType.Road)
                {
                    posY = 0.5f;
                }

                Vector3 pos = startPos + new Vector3((j * 2), posY, (i * -2));
                Instantiate(mapObjects[(int)mapTypes[j]], pos, Quaternion.identity, transform);
            }
        }
    }

}
