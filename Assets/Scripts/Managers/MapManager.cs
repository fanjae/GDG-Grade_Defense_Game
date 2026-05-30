using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    BuildTile,
    Road
}

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset mapFile;

    public Dictionary<int, List<MapType>> MapDatas { get; private set; } = new Dictionary<int, List<MapType>>();

    private void Awake()
    {
        MapLoad();
    }

    private void MapLoad()
    {
        string mapData = mapFile.text;

        string[] row = mapData.Split("\r\n");

        for (int i = 0; i < row.Length; i++)
        {
            if (row[i] == "")
            {
                continue;
            }

            string[] col = row[i].Split(",");

            List<MapType> mapTypes = new List<MapType>();

            for (int j = 0; j < col.Length; j++)
            {
                if (int.TryParse(col[j], out int mapEnumType))
                {
                    mapTypes.Add((MapType)mapEnumType);
                }
            }

            MapDatas.Add(i, mapTypes);
        }
    }

}
