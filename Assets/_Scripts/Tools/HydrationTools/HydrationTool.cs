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
}


