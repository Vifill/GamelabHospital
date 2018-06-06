using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public SanitationThresholdConfig BedSanitationThresholds;

    private BedController BedController;
    //private GameController GC;

    public ToolName RequiredTool;

    // UI Stuff
    public GameObject DirtyBarPrefab;
    public GameObject DirtyBarInstance;
    private Camera Cam;
    public Image BarFill;

    //private Image BarFill;
    private Animator DirtyBarAnimator;

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
        //BedController = GetComponent<BedController>();
        //GC = FindObjectOfType<GameController>();
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
        DirtyBarUIPositionBed.transform.SetParent(UISpawner.instance.UIDictionary[UIHierarchy.PatientUI].transform);
        DirtyBarUIPositionBed.localScale = new Vector3(1, 1, 1);
        DirtyBarUIPositionBed.position = Cam.WorldToScreenPoint(DirtyBarWorldBedPosition.position);

        DirtyBarInstance = Instantiate(DirtyBarPrefab, DirtyBarUIPositionBed);
        DirtyBarInstance.transform.localPosition = Vector3.zero;

        BarFill = DirtyBarInstance.transform.Find("MeterFill").GetComponent<Image>();
        BarFill.fillAmount = 0;
        DirtyBarAnimator = DirtyBarInstance.GetComponent<Animator>();

        var thresholdLine = BarFill.transform.GetChild(0).GetComponent<RectTransform>();
        var xPos = BedSanitationThresholds.ListOfThresholds.FirstOrDefault().ThresholdOfActivation - 1;
        thresholdLine.anchoredPosition = new Vector2(xPos, thresholdLine.anchoredPosition.y);
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

    public override void OnStartAction(GameObject pObjectActioning)
    {
        pObjectActioning.GetComponentInChildren<Animator>().SetBool(Constants.AnimationParameters.CharacterCleanBed, true);
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        base.OnFinishedAction(pObjectActioning);
        pObjectActioning.GetComponentInChildren<Animator>().SetBool(Constants.AnimationParameters.CharacterCleanBed, false);
        SetClean();
        LevelManager.AddPoints(50, transform.position);
    }

    private void SetDirtyBarWarning(bool pValue)
    {
        DirtyBarAnimator.SetBool(Constants.AnimationParameters.IsPulsatingUI, pValue);
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

            if (BarFill.fillAmount >= 0.80)
            {
                SetDirtyBarWarning(true);
            }
            else
            {
                SetDirtyBarWarning(false);
            }
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

        while ((DirtyBarInstance.transform.position - pTargetTransform.position).sqrMagnitude > 1)
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
