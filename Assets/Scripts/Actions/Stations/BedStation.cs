using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedStation : Actionable 
{
    //public bool IsDirty { private set; get; }
    public bool IsDirty;
    private BedController BedController;

    protected override void Initialize()
    {
        BedController = GetComponent<BedController>();
        IsDirty = false;
    }

    private void Update()
    {
        if (BedController.HasPatient)
        {
            IsActionActive = false;
        }
    }

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        if (IsDirty && !BedController.IsReserved)
        {
            return true;
        }
        else if (!IsDirty)
        {
            return false;
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
    }

    public void SetClean()
    {
        IsDirty = false;
    }
}
