using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningStation : Actionable
{
    public List<ToolName> CleanableTools;

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        //return CleanableTools.Contains(pCurrentTool);
        var toolbase = pObjectActioning.GetComponent<ToolController>().GetToolBase();
        if (toolbase != null && toolbase.IsDirty && CleanableTools.Contains(pCurrentTool) && IsActionActive)
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
        pObjectActioning.GetComponent<ToolController>().GetToolBase().CleanTool();
    }

}
