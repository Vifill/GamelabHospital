using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanable : Actionable
{
    private Actionable OtherActionable;

    public bool IsDirty { private set; get; }
    public ToolName RequiredTool;

    protected override void Initialize()
    {
        var actionables = GetComponents<Actionable>();

        if (actionables.Length > 1)
        {
            foreach (var actionable in actionables)
            {
                if (actionable != this)
                {
                    OtherActionable = actionable;
                }
            }
        }
    }

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        if (IsDirty && pCurrentTool == RequiredTool)
        {
            return true;
        }
        return false;
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        SetClean();
    }

    public void SetDirty()
    {
        IsDirty = true;
        if (OtherActionable != null)
        {
            OtherActionable.IsActionActive = false;
        }
        
    }

    public void SetClean()
    {
        IsDirty = false;
        if (OtherActionable != null)
        {
            OtherActionable.IsActionActive = true;
        }
    }
}
