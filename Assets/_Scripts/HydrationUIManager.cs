using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydrationUIManager : MonoBehaviour 
{
    public Image HydrationMeterUI;
    public Image SeveretyMeterUI;
    public PatientStatusColorConfig StatusColorConfig; // Use same color config as for the patient status
    public float UIOffset = 2;

    private HealthController HealthController;
    private Transform Patient;

    public void InitializeHydrationUI(HealthController pHealthController)
    {
        HealthController = pHealthController;
        Patient = pHealthController.transform;
    }

    private void Update()
    {
        HydrationMeterUI.fillAmount = HealthController.HydrationMeter;
        SeveretyMeterUI.fillAmount = HealthController.CholeraSeverity;

        if (HydrationMeterUI.fillAmount >= 0.75)
        {
            HydrationMeterUI.color = StatusColorConfig.StatusRed;
        }
        else if (HydrationMeterUI.fillAmount >= 0.45)
        {
            HydrationMeterUI.color = StatusColorConfig.StatusYellow;
        }
        else
        {
            HydrationMeterUI.color = StatusColorConfig.StatusGreen;
        }

        HydrationMeterUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, UIOffset, UIOffset));
    }
}
