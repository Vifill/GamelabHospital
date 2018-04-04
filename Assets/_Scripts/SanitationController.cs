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

    public float CurrentSanitationLevel;
    public float MaxSanitationLevel;

    public GameObject SanitationBarUIPrefab;
    public string UIPosition;
    public Sprite SanitationUIPicture;
    public Color SanitationUIColor;
    public GameObject DirtyParticles;
    [HideInInspector]
    public SanitationThresholdConfig DoctorSanitationConfig;
    private GameController GC;

	// Use this for initialization
	private void Start() 
	{
        Tutorial2ScreenController = FindObjectOfType<Level2TutorialScreenController>();
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        GC = FindObjectOfType<GameController>();

        if (DirtyParticles.activeInHierarchy)
        {
            DirtyParticles.SetActive(false);
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
            CurrentSanitationLevel += pDirt;
            SanitationUI.UpdateSanitationUI();

            if (CurrentSanitationLevel >= DoctorSanitationConfig.ListOfThresholds.FirstOrDefault().ThresholdOfActivation)
            {
                DirtyParticles.SetActive(true);
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
            CurrentSanitationLevel = 0;
            SanitationUI.UpdateSanitationUI();

            if (DirtyParticles.activeInHierarchy)
            {
                DirtyParticles.SetActive(false);
            }
        }
    }

}
