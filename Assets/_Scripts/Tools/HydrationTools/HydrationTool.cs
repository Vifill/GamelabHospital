﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrationTool : ToolBase
{
    public float HydrationReplenished;
    public float TimeItTakes;
    public float ActionTime;

    private float Counter;

    public void UpdateTool(HealthController pHealthCtrl)
    {
        Counter += Time.deltaTime;

        if (Counter >= TimeItTakes)
        {
            pHealthCtrl.HydrationMeter += (HydrationReplenished / TimeItTakes) * Time.deltaTime;
        }
    }
}