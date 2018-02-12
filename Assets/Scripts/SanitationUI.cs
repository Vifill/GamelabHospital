using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanitationUI : MonoBehaviour 
{
    public PatientStatusColorConfig SanitationStatusColorConfig; // Use same color config as for the patient status
    private Image SanitationProgressBar;
    private Image Portrait;
    private Image ColorCode;
    private SanitationController SanitationController;


    // Use this for initialization
    private void Start() 
	{
        
    }
	
	// Update is called once per frame
	private void Update() 
	{
        
    }

    public void UpdateSanitationUI()
    {
        SanitationProgressBar.fillAmount = SanitationController.CurrentSanitationLevel / SanitationController.MaxSanitationLevel;

        if (SanitationController.CurrentSanitationLevel >= SanitationController.MaxSanitationLevel)
        {
            SanitationController.CurrentSanitationLevel = SanitationController.MaxSanitationLevel;
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

    public void Initialize(Sprite pPlayerPortrait, Color pColorCode, SanitationController pSanitationController, Transform pUIpos)
    {
        SanitationProgressBar = transform.GetChild(2).GetComponent<Image>();
        Portrait = transform.GetChild(4).GetComponent<Image>();
        ColorCode = transform.GetChild(5).GetComponent<Image>();

        SanitationController = pSanitationController;
        Portrait.sprite = pPlayerPortrait;
        ColorCode.color = pColorCode;
        transform.localPosition = pUIpos.localPosition;
        UpdateSanitationUI();
    }
}
