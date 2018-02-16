﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : Actionable
{
    public float HydrationMeter;
    public float CholeraSeverity;

    public HydrationConfig HydrationConfig;
    public CholeraConfig CholeraConfig;
    public CholeraThresholdOddsConfig ThresholdOddsConfig;
    public GameObject HydrationUIPrefab;

    public float ConstantDehydrationSpeed;
    public float ConstantHealing;

    public List<ToolName> HydrationTools;

    private PatientStatusController PatientStatusController;
    private GameObject HydrationUI;
    public bool IsHydrating;
    private HydrationTool CurrentHydrationTool;

    protected override void Initialize()
    {
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        HydrationUI = Instantiate(HydrationUIPrefab, canvas);
        HydrationUI.GetComponent<HydrationUIManager>().InitializeHydrationUI(this);
        PatientStatusController = GetComponent<PatientStatusController>();
        StartCoroutine(SickCoroutine());
    }

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        if (HydrationTools.Contains(pCurrentTool))
        {
            CurrentHydrationTool = pObjectActioning.GetComponent<ToolController>().GetToolBase().GetComponent<HydrationTool>();
            ActionTime = CurrentHydrationTool.ActionTime;
            return true;
        }
        else
        {
            CurrentHydrationTool = null;
            ActionTime = 0;
            return false;
        }
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        IsHydrating = true;
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
        float hydrationLossModifier = HydrationConfig.HydrationLowerThreshold >= HydrationMeter ? HydrationConfig.HydrationLowerThresholdModifier : 1;
        HydrationMeter -= (CholeraConfig.ExcreteHydrationLoss + randomVariance) * hydrationLossModifier;

        Debug.Log($"I'M PUKING!");
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

        if (IsHydrating)
        {
            CurrentHydrationTool?.UpdateTool(this);
        }
    }
}
