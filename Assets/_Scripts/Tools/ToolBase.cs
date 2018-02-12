using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBase : MonoBehaviour
{
    public ToolName ToolName;
    public bool NeedsToBeSanitized;
    public bool CanBeDropped = true;
    public GameObject DirtyMesh;
    [Header("Only if it needs to be sanitized")]
    public int UsesBeforeDirty;

    public bool IsDirty { get; private set; }

    private int CurrentUses;

    public void ToolUsed()
    {
        if(NeedsToBeSanitized)
        {
            CurrentUses++;
            if (CurrentUses >= UsesBeforeDirty)
            {
                IsDirty = true;
                DirtyMesh?.SetActive(true);
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

    NoTool = -1,
}
