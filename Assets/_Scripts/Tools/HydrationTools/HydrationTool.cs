using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrationTool : ToolBase
{
    public HydrationModel HydrationModel;
}

[System.Serializable]
public class HydrationModel
{
    [Header("Hydration Tool Parameters")]
    public float HydrationReplenished;
    public float TimeItTakes;
    public float ActionTime;
    public GameObject DisplayPrefab;
    public AudioClip HydrationSound;

    public override bool Equals(object obj)
    {
        var modelToCompare = (HydrationModel)obj;
        bool comparer = DisplayPrefab == modelToCompare.DisplayPrefab;
        comparer &= HydrationReplenished == modelToCompare.HydrationReplenished;
        comparer &= TimeItTakes == modelToCompare.TimeItTakes;
        comparer &= ActionTime == modelToCompare.ActionTime;
        comparer &= HydrationSound == modelToCompare.HydrationSound;
        return comparer;
    }

    public override int GetHashCode()
    {
        return DisplayPrefab?.GetHashCode() ?? 1;
    }
}


