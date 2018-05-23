using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SanitationUI : MonoBehaviour 
{
    public PatientStatusColorConfig SanitationStatusColorConfig; // Use same color config as for the patient status
    public Image SanitationProgressBar;
    public Image Portrait;
    //public Image ColorCode;
    private SanitationController SanitationController;

    public void UpdateSanitationUI()
    {
        SanitationProgressBar.fillAmount = SanitationController.Sanitation / SanitationController.MaxSanitation;

        var thresholds = SanitationController.DoctorSanitationConfig.ListOfThresholds;

        if (SanitationController.Sanitation >= SanitationController.MaxSanitation)
        {
            SanitationController.Sanitation = SanitationController.MaxSanitation;
        }

        if (SanitationController.Sanitation >= thresholds.LastOrDefault().ThresholdOfActivation)
        {
            SanitationProgressBar.color = SanitationStatusColorConfig.StatusRed;
        }
        else if (SanitationController.Sanitation >= thresholds.FirstOrDefault().ThresholdOfActivation)
        {
            SanitationProgressBar.color = SanitationStatusColorConfig.StatusYellow;
        }
        else
        {
            SanitationProgressBar.color = SanitationStatusColorConfig.StatusGreen;
        }
    }

    public void Initialize(Sprite pPlayerPortrait, Color pColorCode, SanitationController pSanitationController, string pUIpos)
    {
        SanitationController = pSanitationController;
        Portrait.sprite = pPlayerPortrait;
        //ColorCode.color = pColorCode;
        var UIpos = transform.parent.Find(pUIpos);
        transform.localPosition = UIpos.localPosition;
        UpdateSanitationUI();
    }
}
