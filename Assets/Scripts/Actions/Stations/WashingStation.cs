using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingStation : Actionable
{
    private float OriginalStartingTime;
    private GameObject ObjectActioning;

    protected override void Initialize()
    {
        OriginalStartingTime = ActionTime;
    }

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        var toolbase = pObjectActioning.GetComponent<ToolController>().GetToolBase();

        if (toolbase == null && IsActionActive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnStartAction(GameObject pObjectActioning)
    {
        ActionTime += pObjectActioning.GetComponent<SanitationController>().CurrentSanitationLevel / 100;
        ObjectActioning = pObjectActioning;
    }

    public override void OnStopAction()
    {
        ObjectActioning.GetComponent<SanitationController>().CurrentSanitationLevel = 0;
        ActionTime = OriginalStartingTime;
    }
}
