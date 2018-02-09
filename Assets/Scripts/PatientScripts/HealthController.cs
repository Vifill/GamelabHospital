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

    public float ConstantDehydrationSpeed;

    private PatientStatusController PatientStatusController;

    private void Start()
    {
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
        float randomVariance = UnityEngine.Random.Range(-CholeraConfig.ExcreteHydrationLossVariance, CholeraConfig.ExcreteHydrationLossVariance);
        HydrationMeter -= CholeraConfig.ExcreteHydrationLoss + randomVariance;

        Debug.Log($"I'M PUKING!");
    }

    private void Update()
    {
        if(!PatientStatusController.IsDead)
        {
            HydrationMeter -= ConstantDehydrationSpeed * Time.deltaTime;

            if (!PatientStatusController.IsDead && HydrationMeter <= 0)
            {
                PatientStatusController.Death();
            }
        }
    }
}
