using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedStation : Actionable 
{
    public float DirtyMeter;
    private BedController BedController;

    public ToolName RequiredTool;

    protected override void Initialize()
    {
        BedController = GetComponent<BedController>();
    }

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        if (DirtyMeter > 0 && pCurrentTool == RequiredTool)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        SetClean();
    }

    public void IncreaseDirtyMeter(float pValue)
    {
        DirtyMeter += pValue;
        Mathf.Clamp(DirtyMeter, 0, 100);
    }

    public void SetClean()
    {
        DirtyMeter = 0;
    }
}
