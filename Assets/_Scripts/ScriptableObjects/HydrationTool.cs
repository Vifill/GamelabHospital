using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HydrationTool : ScriptableObject
{
    public float HydrationReplenished;
    public float TimeItTakes;

    protected float Counter;

    public abstract void UpdateTool(HealthController pHealthCtrl) { }
}
