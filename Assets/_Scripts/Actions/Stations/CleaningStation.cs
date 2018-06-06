using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningStation : Actionable
{
    public List<ToolName> CleanableTools;

    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
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

    public override void OnStartAction(GameObject pObjectActioning)
    {
        var toolController = pObjectActioning.GetComponent<ToolController>();

        if (toolController.GetCurrentToolName() == ToolName.Bucket)
        {
            pObjectActioning.GetComponentInChildren<Animator>().SetBool(Constants.AnimationParameters.CharacterCleanBucket, true);
        }
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        base.OnFinishedAction(pObjectActioning);

        pObjectActioning.GetComponentInChildren<Animator>().SetBool(Constants.AnimationParameters.CharacterCleanBucket, false);
        pObjectActioning.GetComponent<ToolController>().GetToolBase().CleanTool();
    }

}
