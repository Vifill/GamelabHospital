using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Levels/LevelConfig")]
public class LevelConfig : ScriptableObject 
{
    public int LevelNumber;
    public StarConfig StarConfig;
    public float LevelTimeSecs;
}
[System.Serializable]
public class StarConfig
{
    public int PointsForBronze;
    public int PointsForSilver;
    public int PointsForGold;

    public int GetStar(int pPoints)
    {
        int starCount = 0;
        if(pPoints >= PointsForGold)
        {
            starCount = 3;
        }
        else if (pPoints >= PointsForSilver)
        {
            starCount = 2;
        }
        else if (pPoints >= PointsForBronze)
        {
            starCount = 1;
        }
        return starCount;
    }
}

