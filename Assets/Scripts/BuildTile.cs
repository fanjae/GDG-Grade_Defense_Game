using UnityEngine;

public class BuildTile : MonoBehaviour
{
    private bool isBuild;
    public bool CanBuild()
    {
        if(isBuild==false)
        {
            return true;
        }
        return false;
    }
    public void SetBuild()
    {
        isBuild = true;
    }
}
