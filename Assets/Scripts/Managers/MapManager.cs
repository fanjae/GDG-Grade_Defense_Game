using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    BuildTile,  // 타워 설치 가능한 타일
    Road        // 몬스터가 지나다닐 길
}

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset mapFile;  // Excel CSV 파일 가져오기

    // 맵 데이터 저장할 딕셔너리
    public Dictionary<int, List<MapType>> MapDatas { get; private set; } = new Dictionary<int, List<MapType>>();

    private void Awake()
    {
        MapLoad();  // Awake에서 맵 로드하기
    }

    private void MapLoad()
    {
        string mapData = mapFile.text;  // 맵 파일을 문자열로 받기

        string[] row = mapData.Split("\r\n");   // \r\n 기준으로 mapData 나누기

        for (int i = 0; i < row.Length; i++)    // for문 돌려서 딕셔너리에 저장하기
        {
            if (row[i] == "")   // 빈칸이면 넘어감
            {
                continue;
            }

            string[] col = row[i].Split(",");   // CSV 파일은 ','로 구분되기 때문에 ',' 로 나눠 문자열로 저장하기

            List<MapType> mapTypes = new List<MapType>();   // value가 리스트이므로 리스트를 생성하고

            for (int j = 0; j < col.Length; j++)    // for문 돌려서 리스트에 값을 저장하기
            {
                if (int.TryParse(col[j], out int mapEnumType))
                {
                    mapTypes.Add((MapType)mapEnumType); // enum에 있는 MapType으로 리스트에 저장
                }
            }

            MapDatas.Add(i, mapTypes);  // 최종적으로 딕셔너리에 저장
        }
    }

}
