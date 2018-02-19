using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBase : MonoBehaviour
{
    public ToolName ToolName;
    public bool NeedsToBeSanitized;
    public bool CanBeDropped = true;
    public GameObject DirtyMesh;
    public bool IsUsedUpAfterUse;
    [Header("Only if it needs to be sanitized")]
    public int UsesBeforeDirty;
    public bool IsDirty { get; private set; }

    private int CurrentUsesBeforeDirty;

    public void ToolUsed()
    {
        if(NeedsToBeSanitized)
        {
            CurrentUsesBeforeDirty++;
            if (CurrentUsesBeforeDirty >= UsesBeforeDirty)
            {
                IsDirty = true;
                if(DirtyMesh != null)
                {
                    DirtyMesh?.SetActive(true);
                }
            }
        }
    }

    public void CleanTool()
    {
        IsDirty = false;
        DirtyMesh?.SetActive(false);
    }
}

public enum ToolName
{
    Bandage,
    Saw,
    AnaestheticSyringe,
    VaccinationSyringe,
	Bucket,
    Water,
    IVBag,

    NoTool = -1,
}
