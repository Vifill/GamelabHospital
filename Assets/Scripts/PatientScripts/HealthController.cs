using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float CholeraSeverity;
    public float HydrationMeter;

    public CholeraThresholdOddsConfig ThresholdOddsConfig;

    public float DehydrationSpeed;

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
            yield return new WaitForSeconds(1);
            float odds = ThresholdOddsConfig.ListOfThresholds.First(a => a.ThresholdOfActivation <= CholeraSeverity).OddsOfExcretion;

        }
    }

    private void Update()
    {
        if(!PatientStatusController.IsDead)
        {
            HydrationMeter -= DehydrationSpeed * Time.deltaTime;

            if (!PatientStatusController.IsDead && HydrationMeter <= 0)
            {
                PatientStatusController.Death();
            }
        }
    }
}
