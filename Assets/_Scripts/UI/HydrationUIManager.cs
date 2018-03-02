using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydrationUIManager : MonoBehaviour 
{
    public Image HydrationMeterUI;
    public Image SeveretyMeterUI;
    public PatientStatusColorConfig StatusColorConfig; // Use same color config as for the patient status
    public float UIOffset = 1;

    private HealthController HealthController;
    private Transform Patient;

    public void InitializeHydrationUI(HealthController pHealthController)
    {
        HealthController = pHealthController;
        Patient = pHealthController.transform;
        transform.position = Camera.main.WorldToScreenPoint(HealthController.transform.position + new Vector3(0, UIOffset, UIOffset));
    }

    private void Update()
    {
        if (HealthController != null)
        {
            HydrationMeterUI.fillAmount = HealthController.HydrationMeter / 100;
            SeveretyMeterUI.fillAmount = HealthController.CholeraSeverity / 100;

            if (SeveretyMeterUI.fillAmount >= 0.75)
            {
                SeveretyMeterUI.color = StatusColorConfig.StatusRed;
            }
            else if (SeveretyMeterUI.fillAmount >= 0.45)
            {
                SeveretyMeterUI.color = StatusColorConfig.StatusYellow;
            }
            else
            {
                SeveretyMeterUI.color = StatusColorConfig.StatusGreen;
            }

            transform.position = Camera.main.WorldToScreenPoint(HealthController.transform.position + new Vector3(0, UIOffset, UIOffset));
        }
    }
}
