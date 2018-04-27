﻿using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float Health;
    public float HydrationMeter { get; private set; } = 100;

    public float MaxHealthIncresePerSecond;
    public float ComboBonusAmount;
    public float ComboBonusTime;
    public float ComboRedemptionTime;

    public float MaxHydration = 100;
    public float MinHydration = 0;

    public float MaxHealth = 100;
    public float MinHealth = 0;

    [HideInInspector]
    public float HydrationClampMax;
    [HideInInspector]
    public float HydrationClampMin;
    [HideInInspector]
    public float HealthClampMax;
    [HideInInspector]
    public float HealthClampMin;


    [HideInInspector]
    public HydrationConfig HydrationConfig;
    [HideInInspector]
    public CholeraConfig CholeraConfig;
    [HideInInspector]
    public CholeraThresholdOddsConfig ThresholdOddsConfig;
    [HideInInspector]
    public SanitationThresholdConfig BedSanitationConfig;
    [HideInInspector]
    public HydrationHealingConfig HydrationHealingConfig;
    [HideInInspector]
    public SanitationThresholdConfig DoctorSanitationThresholdConfig;

    public GameObject HydrationUIPrefab;
    public GameObject PukeParticleEffectPrefab;
    public Transform PukePosition;
    
    public float ConstantDehydrationSpeed;
    public float ConstantHealing;

    public BedManager BedManagerInstance;

    private LevelManager LevelManager;
    private HydrationController HydrationController;
    private PatientStatusController PatientStatusController;
    [HideInInspector]
    public GameObject HydrationUI;
    private Transform MainCanvasTransform;

    private Coroutine CurrentCoroutineSick;
    private Animator PatientAnimator;
    private Animator PatientPrefabAnimator;

    private float HealthIncrease;

    private void Start()
    {
        StartCoroutine(GetPatientAnimator());
        LevelManager = FindObjectOfType<LevelManager>();
        HydrationClampMax = MaxHydration;
        HydrationClampMin = MinHydration;
        HealthClampMax = MaxHealth;
        HealthClampMin = MinHealth;
        MainCanvasTransform = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        SpawnHydrationUI();
        PatientStatusController = GetComponent<PatientStatusController>();
        HydrationController = GetComponent<HydrationController>();
        StartSickCoroutine();
        StartCoroutine(BedSanitationCheckCoroutine());
    }

    private IEnumerator GetPatientAnimator()
    {
        yield return new WaitForEndOfFrame();

        PatientAnimator = transform.Find(Constants.Highlightable).GetComponentInChildren<Animator>();
        PatientPrefabAnimator = GetComponent<Animator>();
    }

    private void SpawnHydrationUI()
    {
        HydrationUI = Instantiate(HydrationUIPrefab, MainCanvasTransform);
        HydrationUI.GetComponent<HydrationUIManager>().InitializeHydrationUI(this);
    }

    private void Update()
    {
        if (!LevelManager.TimeOver)
        {
            //var HealthIncrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;

            if (HealthIncrease > 0)
            {
                Health = Mathf.Clamp(Health += HealthIncrease * Time.deltaTime, HealthClampMin, HealthClampMax);
            }

            if (!PatientStatusController.IsHealed && Health >= 100)
            {
                PatientStatusController.IsHealed = true;
                HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(false);
            }

            if (!PatientStatusController.IsDead && !PatientStatusController.IsHealed)
            {
                //HydrationMeter = Mathf.Clamp(HydrationMeter -= ConstantDehydrationSpeed * Time.deltaTime, HydrationClampMin, HydrationClampMax);
                SetHydration(HydrationMeter - (ConstantDehydrationSpeed * Time.deltaTime));

                Health = Mathf.Clamp(Health += ConstantHealing * Time.deltaTime, HealthClampMin, HealthClampMax);

                if (!PatientStatusController.IsDead && HydrationMeter <= 0)
                {
                    PatientStatusController.Death();
                }
            }
        }
    }

    public Coroutine GetSickCoroutine()
    {
        return CurrentCoroutineSick;
    }

    public void StartSickCoroutine()
    {
        CurrentCoroutineSick = StartCoroutine(SickCoroutine());
    }

    private IEnumerator BedSanitationCheckCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            var inBed = BedManagerInstance?.Beds.SingleOrDefault(a => a.PatientInBed == gameObject);            
            if(inBed != null)
            {
                var healthDecrease = BedSanitationConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= inBed.GetComponent<BedStation>().DirtyMeter)?.HealthDecreasePerSecond ?? 0;
                Health = Mathf.Clamp(Health -= healthDecrease, HealthClampMin, HealthClampMax);
            }
        }
    }

    private IEnumerator SickCoroutine()
    {
        while(true)
        {
            float odds = ThresholdOddsConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= Health)?.OddsOfExcretion ?? 0.0f;
            if(UnityEngine.Random.Range(0,100) < odds && HydrationController.IsActionActive)
            {
                StartFeelingSick();
                yield return new WaitForSeconds(CholeraConfig.ExcreteCooldown);
            }
            else if (!(UnityEngine.Random.Range(0,100) < odds))
            {
                yield return new WaitForSeconds(CholeraConfig.CholeraCheckRate);
            }
        }
    }

    private void StartFeelingSick()
    {
        if (!PatientStatusController.IsHealed)
        {
            HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(true);

            Invoke("Excrete", 5);
        }
    }

    private void Excrete()
    {
        if (!PatientStatusController.IsHealed)
        {
            ReduceHydration();
            IncreaseHealthWhenExcreting();
            MakeBedDirty();
            StartPukingAnimation();
            HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(false);
            // puke animation trigger
            PatientAnimator.SetTrigger(AnimationParameters.PatientPuke);
            //PatientPrefabAnimator.SetTrigger(AnimationParameters.PatientPuke);
            Debug.Log($"I'M PUKING!");
        }
    }

    private void StartPukingAnimation()
    {
        var puke = Instantiate(PukeParticleEffectPrefab, PukePosition.position, PukePosition.rotation, PukePosition);
        Destroy(puke, 3f);
    }

    private void IncreaseHealthWhenExcreting()
    {
        Health = Mathf.Clamp(Health += CholeraConfig.ExcreteHealthIncrease, HealthClampMin, HealthClampMax);
    }

    private void ReduceHydration()
    {
        float randomVariance = UnityEngine.Random.Range(CholeraConfig.ExcreteHydrationLossVariance, CholeraConfig.ExcreteHydrationLossVariance*2);
        float hydrationLossModifier = HydrationConfig.HydrationLowerThreshold >= HydrationMeter ? HydrationConfig.HydrationLowerThresholdModifier : 1;
        HydrationMeter = Mathf.Clamp(HydrationMeter -= (CholeraConfig.ExcreteHydrationLoss + randomVariance) * hydrationLossModifier, HydrationClampMin, HydrationClampMax);
    }

    private void MakeBedDirty()
    {
        //var beds = BedManagerInstance?.Beds;
            
        var patientInBed = BedManagerInstance?.Beds.SingleOrDefault(a => a.PatientInBed == gameObject);        

        if (patientInBed != null)
        {
            patientInBed.BedStation.IncreaseDirtyMeter(CholeraConfig.ExcreteBedDirtyIncrease);
        }
        else
        {
           // print("<color=magenta> puked but was not in bed </color>");
        }
    }

    public void SetHydration (float pValue)
    {
        var threshold = HydrationHealingConfig.ListOfThresholds.LastOrDefault().ThresholdOfActivation;
        if (HydrationMeter >= threshold && pValue < threshold)
        {
            //StartCoroutine(ComboRedemptionCheck());
            StartCoroutine(ComboBonus(ComboRedemptionTime, -ComboBonusAmount));
        }
        else if (HydrationMeter < threshold && pValue >= threshold)
        {
            StartCoroutine(ComboBonus(ComboBonusTime, ComboBonusAmount));
        }
        else
        {
            HealthIncrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;
        }

        HydrationMeter = Mathf.Clamp(pValue, HydrationClampMin, HydrationClampMax);
    }

    //private IEnumerator ComboRedemptionCheck()
    //{
    //    yield return new WaitForSeconds(ComboRedemptionTime);

    //    var threshold = HydrationHealingConfig.ListOfThresholds.LastOrDefault().ThresholdOfActivation;

    //    if (HydrationMeter < threshold)
    //    {
    //        HealthIncrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;
    //    }
    //}

    private IEnumerator ComboBonus(float pTime, float pAmount)
    {
        while (true)
        {
            yield return new WaitForSeconds(pTime);
            var minHPS = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;

            if (pAmount < 0)
            {
                pTime -= 1;
            }
            else if (pAmount > 0)
            {
                pTime += 1;
            }

            HealthIncrease = Mathf.Clamp(HealthIncrease + pAmount, minHPS, MaxHealthIncresePerSecond);
            print("healthincrease is now = " + HealthIncrease);
        }
    }

    public void ForceExcretion()
    {
        Excrete();
    }
}