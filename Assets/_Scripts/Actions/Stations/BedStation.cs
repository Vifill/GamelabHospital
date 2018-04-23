using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedStation : Actionable 
{
    public float DirtyMeter;
    public float MaxDirtyness = 100;
    public float MinDirtyness = 0;
    [HideInInspector]
    public float DirtynessClampMax;
    [HideInInspector]
    public float DirtynessClampMin;

    private BedController BedController;
    private GameController GC;

    public ToolName RequiredTool;

    // UI Stuff
    public GameObject DirtyBarPrefab;
    private GameObject DirtyBarInstance;
    private Camera Cam;
    private Image BarFill;
    public Transform DirtyBarWorldBedPosition;

    private Transform DirtyBarUIPositionBed;
    private Transform DirtyBarUIPositionPatient;

    //private Vector3 BucketUIPos;
    private LevelManager LevelManager;
    public float LerpSpeed;
    private GameObject Canvas;

    private Coroutine UIPositionUpdateCoroutine;
    private const string DirtyBarPosString = "DirtyBarPos";

    protected override void Initialize()
    {
        LevelManager = FindObjectOfType<LevelManager>();
        DirtynessClampMax = MaxDirtyness;
        DirtynessClampMin = MinDirtyness;
        BedController = GetComponent<BedController>();
        GC = FindObjectOfType<GameController>();
        Canvas = GameObject.Find("MainCanvas");
        
        //UI Stuff
        Cam = FindObjectOfType<Camera>();

        if (DirtyBarPrefab != null /*&& GC.ShouldSpawnBucketUI()*/)
        {
            InitializeDirtyBar();
            UpdateDirtyUI();
        }
    }

    private void InitializeDirtyBar()
    {
        DirtyBarUIPositionBed = new GameObject("DirtyBarUIPositionBed").transform;
        DirtyBarUIPositionBed.transform.SetParent(Canvas.transform);
        DirtyBarUIPositionBed.localScale = new Vector3(1, 1, 1);
        DirtyBarUIPositionBed.position = Cam.WorldToScreenPoint(DirtyBarWorldBedPosition.position);

        DirtyBarInstance = Instantiate(DirtyBarPrefab, DirtyBarUIPositionBed);
        DirtyBarInstance.transform.localPosition = Vector3.zero;

        BarFill = DirtyBarInstance.transform.GetChild(1).GetComponent<Image>();      
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
        DirtyMeter = Mathf.Clamp(DirtyMeter += pValue, DirtynessClampMin, DirtynessClampMax);
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
            BarFill.fillAmount = DirtyMeter / 100;
        }
    }
    
    private void Update()
    {
        if (DirtyBarUIPositionBed != null)
        {
            DirtyBarUIPositionBed.position = Cam.WorldToScreenPoint(DirtyBarWorldBedPosition.position);
        }
    }

    private IEnumerator LerpToUITransform(Transform pTargetTransform)
    {
        yield return null;
        var oldParent = DirtyBarInstance.transform.parent;
        DirtyBarInstance.transform.SetParent(pTargetTransform);
        //DirtyBarInstance.transform.position = oldParent.position;

        while (Vector3.Distance(DirtyBarInstance.transform.position, pTargetTransform.position) > 1)
        {
            DirtyBarInstance.transform.position = Vector3.Lerp(DirtyBarInstance.transform.position, pTargetTransform.transform.position, Time.deltaTime * LerpSpeed);
            yield return null;
        }
        yield return null;
    }

    public void LerpDirtyBarUIWhenPatientLeavesBed()
    {
        if (DirtyBarInstance != null)
        {
            DirtyBarInstance.transform.SetParent(Canvas.transform);
            StartCoroutine(LerpToUITransform(DirtyBarUIPositionBed));
        }
    }

    public void LerpDirtyBarUIWhenPatientEntersBed(GameObject pObjectToParent)
    {
        if (DirtyBarInstance != null)
        {
            DirtyBarUIPositionPatient = pObjectToParent.transform.Find(DirtyBarPosString).transform;
            StartCoroutine(LerpToUITransform(DirtyBarUIPositionPatient));
        }
    }
}
