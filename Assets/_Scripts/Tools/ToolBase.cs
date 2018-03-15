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
    private Level3TutorialScreenController TutorialScreenController;

    private void Start()
    {
        TutorialScreenController = FindObjectOfType<Level3TutorialScreenController>();
        if (DirtyMesh != null)
        {
            DirtyMesh?.SetActive(false);
        }
    }

    public void ToolUsed()
    {
        if(NeedsToBeSanitized)
        {
            CurrentUsesBeforeDirty++;
            if (CurrentUsesBeforeDirty >= UsesBeforeDirty)
            {
                if(ToolName == ToolName.Bucket && TutorialScreenController != null)
                {
                    TutorialScreenController.DisplayBucketDirtyScreen();
                }

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
    Water,
    IVBag,

    NoTool = -1,
}
