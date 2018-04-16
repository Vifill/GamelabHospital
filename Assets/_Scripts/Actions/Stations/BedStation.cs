using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedStation : Actionable 
{
    public float DirtyMeter;
    private BedController BedController;
    private GameController GC;

    public ToolName RequiredTool;

    // UI Stuff
    public GameObject DirtyBarPrefab;
    private GameObject DirtyBarInstance;
    private Camera Cam;
    private Image BarFill;
    public Transform BucketUIPos;
    private LevelManager LevelManager;


    protected override void Initialize()
    {
        LevelManager = FindObjectOfType<LevelManager>();
        BedController = GetComponent<BedController>();
        GC = FindObjectOfType<GameController>();

        //UI Stuff
        Cam = FindObjectOfType<Camera>();

        if (DirtyBarPrefab != null && GC.ShouldSpawnBucketUI())
        {
            DirtyBarInstance = Instantiate(DirtyBarPrefab, FindObjectOfType<Canvas>().transform);
            BarFill = DirtyBarInstance.transform.GetChild(0).GetComponent<Image>();
            UpdateDirtyUI();
        } 
    }

    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
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
        LevelManager.AddPoints(50, transform.position);
    }

    public void IncreaseDirtyMeter(float pValue)
    {
        DirtyMeter = Mathf.Clamp(DirtyMeter += pValue, 0, 100);
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
        if (DirtyBarInstance != null)
        {
            BarFill.GetComponent<UIFillAmount>().FillAmount = DirtyMeter / 100;
        }
    }

    private void Update()
    {
        if (DirtyBarInstance != null)
        {
            DirtyBarInstance.transform.position = Cam.WorldToScreenPoint(BucketUIPos.position);
        }
    }

    //public Vector3 GetWorldPositionOnPlane(Vector3 pScreenPosition, float z)
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(pScreenPosition);
    //    Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, z));
    //    float distance;
    //    plane.Raycast(ray, out distance);
    //    return ray.GetPoint(distance);
    //}
}
