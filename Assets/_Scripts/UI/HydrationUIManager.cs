using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HydrationUIManager : MonoBehaviour 
{
    public Animator HydrationAnimator;

    public RectTransform HydrationThresholdLine;
    public Image HydrationMeterUI;
    public Image HealthMeterUI;
    public PatientStatusColorConfig StatusColorConfig; // Use same color config as for the patient status
    public float UIOffset = 1;

    public float WarningThreshold;

    private bool IsWarning;

    private HealthController HealthController;
    //private Transform Patient;
    public GameObject WarningIconInstance;

    public void SetExcreteWarning(bool pValue)
    {
        WarningIconInstance.SetActive(pValue);
    }

    public void InitializeHydrationUI(HealthController pHealthController)
    {
        HealthController = pHealthController;

        HydrationMeterUI.fillAmount = HealthController.HydrationMeter / 100;
        HealthMeterUI.fillAmount = HealthController.Health / 100;

        var thresholdLineXpos = HealthController.HydrationHealingConfig.ListOfThresholds.LastOrDefault().ThresholdOfActivation;
        HydrationThresholdLine.anchoredPosition = new Vector2(thresholdLineXpos + 1, HydrationThresholdLine.anchoredPosition.y);
        // UI position
        transform.position = Camera.main.WorldToScreenPoint(HealthController.transform.position + new Vector3(0, UIOffset, UIOffset));
    }

    private void LateUpdate()
    {
        if (HealthController != null)
        {
            // For testing HydrationMeterUI_v2, switch lines for v1
            //HydrationMeterUI.GetComponent<UIFillAmount>().FillAmount = HealthController.HydrationMeter / 100;
            HydrationMeterUI.fillAmount = HealthController.HydrationMeter / 100;
            HealthMeterUI.fillAmount = HealthController.Health / 100;

            SetSeverityColor();
            SetHydrationWarning();

            transform.position = Camera.main.WorldToScreenPoint(HealthController.transform.position + new Vector3(0, UIOffset, UIOffset));
        }
    }

    private void SetHydrationWarning()
    {
        if(!IsWarning && HydrationMeterUI.fillAmount <= WarningThreshold)
        {
            SetHydrationWarning(true);
            IsWarning = true;
        }
        else if(IsWarning && HydrationMeterUI.fillAmount > WarningThreshold)
        {
            SetHydrationWarning(false);     
            IsWarning = false;
        }
    }

    private void SetHydrationWarning(bool pValue)
    {
        HydrationAnimator.SetBool(Constants.AnimationParameters.IsPulsatingHydration, pValue);
    }

    private void SetHealthWarning(bool pValue)
    {
        HydrationAnimator.SetBool(Constants.AnimationParameters.IsPulsatingHP, pValue);
    }

    private void SetSeverityColor()
    {
        if (HealthMeterUI.fillAmount >= 0.45)
        {
            HealthMeterUI.color = StatusColorConfig.StatusGreen;
        }
        else if (HealthMeterUI.fillAmount <= 0.25)
        {
            HealthMeterUI.color = StatusColorConfig.StatusRed;
        }
        else
        {
            HealthMeterUI.color = StatusColorConfig.StatusYellow;
        }
    }
}
