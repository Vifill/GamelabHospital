using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedStation : Actionable 
{
    public float DirtyMeter;
    private BedController BedController;

    public ToolName RequiredTool;

    // UI Stuff
    public GameObject DirtyBarPrefab;
    private GameObject DirtyBarInstance;
    private Camera Cam;
    private float BarMaxWidth;
    private float BarHeight;
    private Image BarFill;


    protected override void Initialize()
    {
        BedController = GetComponent<BedController>();

        //UI Stuff
        if (DirtyBarPrefab != null)
        Cam = FindObjectOfType<Camera>();
        DirtyBarInstance = Instantiate(DirtyBarPrefab, FindObjectOfType<Canvas>().transform);
        DirtyBarInstance.transform.position = Cam.WorldToScreenPoint(transform.position);
        BarFill = DirtyBarInstance.transform.GetChild(0).GetComponent<Image>();
        BarMaxWidth = BarFill.rectTransform.rect.width;
        BarHeight = BarFill.rectTransform.rect.height;
        UpdateDirtyUI();
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
        //UI Stuff
        UpdateDirtyUI();
    }

    public void SetClean()
    {
        DirtyMeter = 0;
        //UI Stuff
        UpdateDirtyUI();
    }

    private void UpdateDirtyUI()
    {
        BarFill.rectTransform.sizeDelta = new Vector2(((BarMaxWidth / 100) * DirtyMeter), BarHeight);
    }
}
