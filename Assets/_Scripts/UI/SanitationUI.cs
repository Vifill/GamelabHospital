using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanitationUI : MonoBehaviour 
{
    public PatientStatusColorConfig SanitationStatusColorConfig; // Use same color config as for the patient status
    public Image SanitationProgressBar;
    public Image Portrait;
    public Image ColorCode;
    private SanitationController SanitationController;

    public void UpdateSanitationUI()
    {
        SanitationProgressBar.fillAmount = SanitationController.Sanitation / SanitationController.MaxSanitation;

        if (SanitationController.Sanitation >= SanitationController.MaxSanitation)
        {
            SanitationController.Sanitation = SanitationController.MaxSanitation;
        }

        if (SanitationProgressBar.fillAmount >= 0.75)
        {
            SanitationProgressBar.color = SanitationStatusColorConfig.StatusRed;
        }
        else if (SanitationProgressBar.fillAmount >= 0.45)
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
        ColorCode.color = pColorCode;
        var UIpos = transform.parent.Find(pUIpos).transform;
        transform.localPosition = UIpos.localPosition;
        UpdateSanitationUI();
    }
}
