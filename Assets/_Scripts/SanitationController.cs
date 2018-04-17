using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SanitationController : MonoBehaviour
{
    private GameObject SanitationBar;
    private SanitationUI SanitationUI;
    private Transform MainCanvas;

    private Level2TutorialScreenController Tutorial2ScreenController;

    public float Sanitation;
    public float MaxSanitation = 100;
    public float MinSanitation = 0;
    [HideInInspector]
    public float SanitationClampMax;
    [HideInInspector]
    public float SanitationClampMin;

    public GameObject SanitationBarUIPrefab;
    public string UIPosition;
    public Sprite SanitationUIPicture;
    public Color SanitationUIColor;
    public GameObject DirtyParticles50Percent;
    public GameObject DirtyParticles85Percent;
    [HideInInspector]
    public SanitationThresholdConfig DoctorSanitationConfig;
    private GameController GC;

	// Use this for initialization
	private void Start() 
	{
        SanitationClampMax = MaxSanitation;
        SanitationClampMin = MinSanitation;
        Tutorial2ScreenController = FindObjectOfType<Level2TutorialScreenController>();
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        GC = FindObjectOfType<GameController>();

        if (DirtyParticles50Percent.activeInHierarchy)
        {
            DirtyParticles50Percent.SetActive(false);
        }

        if (DirtyParticles85Percent.activeInHierarchy)
        {
            DirtyParticles85Percent.SetActive(false);
        }
        
        InitializeSanitationUI();
	}

    private void InitializeSanitationUI()
    {
        if (GC.ShouldSpawnSanitationUI())
        {
            SanitationBar = Instantiate(SanitationBarUIPrefab, MainCanvas);
            SanitationUI = SanitationBar.GetComponent<SanitationUI>();
            SanitationUI.Initialize(SanitationUIPicture, SanitationUIColor, this, UIPosition);
        }
    }

    public void MakePlayerDirty(float pDirt)
    {
        if (SanitationUI != null)
        {
            Sanitation = Mathf.Clamp(Sanitation += pDirt, SanitationClampMin, SanitationClampMax);
            SanitationUI.UpdateSanitationUI();

            if (Sanitation >= DoctorSanitationConfig.ListOfThresholds.FirstOrDefault().ThresholdOfActivation)
            {
                DirtyParticles85Percent.SetActive(true);
            }
            else if (CurrentSanitationLevel >= DoctorSanitationConfig.ListOfThresholds.FirstOrDefault().ThresholdOfActivation)
            {
                DirtyParticles50Percent.SetActive(true);
                if(Tutorial2ScreenController != null)
                {
                    Tutorial2ScreenController.DisplayDirtyPlayerScreen();
                }
            }
        }
    }

    public void ClearSanitation()
    {
        if (SanitationBar != null)
        {
            Sanitation = 0;
            SanitationUI.UpdateSanitationUI();

            if (DirtyParticles50Percent.activeInHierarchy)
            {
                DirtyParticles50Percent.SetActive(false);
            }

            if (DirtyParticles85Percent.activeInHierarchy)
            {
                DirtyParticles85Percent.SetActive(false);
            }
        }
    }

}
