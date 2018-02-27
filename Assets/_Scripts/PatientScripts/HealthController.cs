using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public bool TEST_HPS;

    public float HydrationMeter;
    public float CholeraSeverity;

    public HydrationConfig HydrationConfig;
    public CholeraConfig CholeraConfig;
    public CholeraThresholdOddsConfig ThresholdOddsConfig;
    public SanitationConfig BedSanitationConfig;

    public GameObject HydrationUIPrefab;
    public GameObject PukeParticleEffect;
    public Transform PukePosition;
    public BedManager BedManagerInstance;

    public float ConstantDehydrationSpeed;
    public float ConstantHealing;

    private PatientStatusController PatientStatusController;
    private GameObject HydrationUI;

    private void Start()
    {
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        HydrationUI = Instantiate(HydrationUIPrefab, canvas);
        HydrationUI.GetComponent<HydrationUIManager>().InitializeHydrationUI(this);
        PatientStatusController = GetComponent<PatientStatusController>();
        StartCoroutine(SickCoroutine());
        StartCoroutine(BedSanitationCheckCoroutine());
    }
    
    private IEnumerator BedSanitationCheckCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            var inBed = BedManagerInstance?.Beds.SingleOrDefault(a => a.PatientInBed == gameObject);            
            if(inBed != null)
            {
                var severityIncrease = BedSanitationConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= inBed.GetComponent<BedStation>().DirtyMeter)?.CholeraSeverityIncreasePerSecond ?? 0;
                CholeraSeverity += severityIncrease;
                CholeraSeverity = Mathf.Clamp(CholeraSeverity, 0, 100);
            }
        }
    }

    private IEnumerator SickCoroutine()
    {
        while(true)
        {
            float odds = ThresholdOddsConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= CholeraSeverity)?.OddsOfExcretion ?? 0.0f;
            if(UnityEngine.Random.Range(0,100) < odds)
            {
                Excrete();
                yield return new WaitForSeconds(CholeraConfig.ExcreteCooldown);
            }
            else
            {
                yield return new WaitForSeconds(CholeraConfig.CholeraCheckRate);
            }
        }
    }



    private void Excrete()
    {
        ReduceHydration();
        ReduceCholeraSeverity();
        MakeBedDirty();
        StartPukingAnimation();
        Debug.Log($"I'M PUKING!");
    }

    private void StartPukingAnimation()
    {
        var puke = Instantiate(PukeParticleEffect, PukePosition.position, PukePosition.rotation, PukePosition);
        Destroy(puke, 3f);
    }

    private void ReduceCholeraSeverity()
    {
        CholeraSeverity -= CholeraConfig.ExcreteCholeraSeverityLoss;
    }

    private void ReduceHydration()
    {
        float randomVariance = UnityEngine.Random.Range(-CholeraConfig.ExcreteHydrationLossVariance, CholeraConfig.ExcreteHydrationLossVariance);
        float hydrationLossModifier = HydrationConfig.HydrationLowerThreshold >= HydrationMeter ? HydrationConfig.HydrationLowerThresholdModifier : 1;
        HydrationMeter -= (CholeraConfig.ExcreteHydrationLoss + randomVariance) * hydrationLossModifier;
    }

    private void Update()
    {
        //if (TEST_HPS && CholeraSeverity > 0)
        //{
        //    CholeraSeverity -= .5f * Time.deltaTime;
        //}

        if (!PatientStatusController.IsHealed && CholeraSeverity <= 0)
        {
            PatientStatusController.IsHealed = true;
        }

        if(!PatientStatusController.IsDead && !PatientStatusController.IsHealed)
        {
            HydrationMeter -= ConstantDehydrationSpeed * Time.deltaTime;
            CholeraSeverity -= ConstantHealing * Time.deltaTime;

            if (!PatientStatusController.IsDead && HydrationMeter <= 0)
            {
                PatientStatusController.Death();
            }
        }
    }
    private void MakeBedDirty()
    {
        var beds = BedManagerInstance?.Beds;

        var patientInBed = BedManagerInstance?.Beds.SingleOrDefault(a => a.PatientInBed == gameObject);        

        if (patientInBed != null)
        {
            patientInBed.BedStation.IncreaseDirtyMeter(CholeraConfig.ExcreteBedDirtyIncrease);
        }
        else
        {
            print("<color=magenta> puked but was not in bed </color>");
        }
    }
}