using UnityEngine;

public class PathManager : MonoBehaviour
{
    [Header("WayPoints List")]
    [SerializeField] private Transform[] waypoints;

    // 현재 등록된 웨이포인트 개수를 외부에서 읽기 위한 프로퍼티
    public int WayPointCount
    { get { return waypoints.Length; } }

    // 에너미가 현재 목표지점을 가져올 때 사용
    public Transform GetWayPoint(int index)
    {
        if(index < 0 || index >= waypoints.Length)
        {
            return null;
        }
        return waypoints[index];
    }
}
