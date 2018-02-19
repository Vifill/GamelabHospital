﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float HydrationMeter;
    public float CholeraSeverity;

    public HydrationConfig HydrationConfig;
    public CholeraConfig CholeraConfig;
    public CholeraThresholdOddsConfig ThresholdOddsConfig;
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
    }

    private IEnumerator SickCoroutine()
    {
        while(true)
        {
            float odds = ThresholdOddsConfig.ListOfThresholds.Last(a => a.ThresholdOfActivation <= CholeraSeverity).OddsOfExcretion;
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
        if(!PatientStatusController.IsDead)
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
        BedController patientBed = null;

        int size = beds?.Count ?? 0;
        for (int i = 0; i < size; i++)
        {
            if (beds[i].PatientInBed == gameObject)
            {
                patientBed = beds[i];
            }
        }

        if (patientBed != null)
        {
            patientBed.BedStation.IncreaseDirtyMeter(20);
        }
        else
        {
            print("<color=magenta> puked but was not in bed </color>");
        }
    }
}