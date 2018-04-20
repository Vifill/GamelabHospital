using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIManager : MonoBehaviour 
{
    public Animator ClockAnimator;

    private Image ClockHand;
    private LevelManager LvlManager;
    private LevelConfig LvlConfig;
    private float ClockHandAngle;
    private float TimeToPulsate;
    private TutorialUtility TutorialUtility;

    public void Initialize(LevelManager pManager, float pStartTime)
    {
        TutorialUtility = FindObjectOfType<TutorialUtility>();
        LvlManager = pManager;
        LvlConfig = LvlManager.LevelConfig;
        ClockHand = transform.GetChild(0).GetComponent<Image>();
        ClockHandAngle = 360 / pStartTime;
        TimeToPulsate = LvlConfig.LevelTimeSecs * 0.25f;
    }

    // Update is called once per frame
    private void Update ()  
	{
        if (!TutorialUtility.TimeFreeze)
        {
            if (LvlManager.Timer < TimeToPulsate && !LvlManager.TimeOver)
            {
                ClockAnimator.SetBool("IsPulsating", true);
            }
            if (LvlManager.Timer > 0)
            {
                float clockHandRotation = ClockHandAngle * Time.deltaTime;
                //Debug.Log(clockHandRotation);
                ClockHand.rectTransform.Rotate(new Vector3(0, 0, -clockHandRotation));
            }
            if (LvlManager.TimeOver)
            {
                ClockAnimator.SetBool("IsPulsating", false);
            }
        }
        
    }
}
