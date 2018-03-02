using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
    private PointsUIManager UIManager;
    private Canvas UICanvas;
    private Animator DayNightCycle;
    private float StartTime;

    public bool DebugDatagathering;
    public int TestingPeriod;
    public bool TimeOver { get; private set; }
    public GameObject ClockUIPrefab;
    public GameObject GoalUIPrefab;
    public GameObject EndScreenPrefab;
    public GameObject SunPrefab;
    public LevelConfig LevelConfig;
    public int PatientsHealed { get; private set; } = 0;
    public int PatientDeaths { get; private set; } = 0;
    public float Timer { get; private set; }

    private int Points;

    private Datalogic DataLogic = new Datalogic();

    private PlayerDataController PlayerDataController = new PlayerDataController();

    private void Start()
    {
        Time.timeScale = 1;
        UICanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        var timerUI = Instantiate(ClockUIPrefab, UICanvas.transform);
        timerUI.GetComponent<TimerUIManager>().Initialize(this, LevelConfig.LevelTimeSecs);
        var goalUI = Instantiate(GoalUIPrefab, UICanvas.transform);
        UIManager = goalUI.GetComponent<PointsUIManager>();
        UIManager.Initialize();
        UIManager.UpdateUI(PatientsHealed);
        StartTime = LevelConfig.LevelTimeSecs;
        Timer = StartTime;
        var sun = Instantiate(SunPrefab);
        DayNightCycle = sun.GetComponent<Animator>();
        var multiplier = 1 / (StartTime / 60);
        DayNightCycle.SetFloat("SpeedMultiplier", multiplier);
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        Mathf.Clamp(Timer, 0, StartTime);
        
        if (Timer <= 0 && !TimeOver)
        {
            TimeOver = true;
            FindObjectOfType<PatientSpawner>().StopSpawning();
            CheckIfAllPatientsAreDone();
            //DayNightCycle.StopPlayback();
            //CheckIfPassed();
        }
    }

    public void AddPoints (int pPoints)
    {
        Points += pPoints;
        if (Points < 0)
        {
            Points = 0;
        }
    }

    public void AddHealed()
    {
        PatientsHealed++;

        UIManager.UpdateUI(PatientsHealed);

        //if (PatientsHealed == LevelConfig.PatientsToHeal)
        //{
        //    //win level
        //}
    }

    public void AddDeath ()
    {
        PatientDeaths++;
    }

    public void CheckIfAllPatientsAreDone()
    {
        var statuses = new List<PatientStatusController>(FindObjectsOfType<PatientStatusController>());
        var checkouts = new List<PatientCheckoutController>(FindObjectsOfType<PatientCheckoutController>());
        if(!statuses.Any() || statuses.All(a => a.IsDead) || checkouts.All(a => a.IsCheckingOut))
        {
            Invoke("CheckIfPassed", 3); 
//#if !UNITY_EDITOR
//            StartCoroutine(DataLogic.SendLevelInfo(PlayerDataController.GetUserID(), SceneManager.GetActiveScene().buildIndex, Points, LevelConfig.StarConfig.GetStar(Points), PatientsHealed, PatientDeaths, TestingPeriod));
//#endif
            //if (true)
            //{
            //    //StartCoroutine(DataLogic.SendLevelInfo(PlayerDataController.GetUserID(), SceneManager.GetActiveScene().buildIndex, Points, LevelConfig.StarConfig.GetStar(Points), PatientsHealed, PatientDeaths));
            //}
        }
    }

    public void CheckIfPassed()
    {     
        if (Points >= LevelConfig.StarConfig.PointsForBronze)
        {
            LevelPassed();
        }
        else
        {
            LevelFailed();
        }
    }

    private void LevelPassed()
    {
        var endscreen = Instantiate(EndScreenPrefab, UICanvas.transform);

        PlayerDataController.SaveLevelData(new LevelModel(LevelConfig.LevelNumber, Points));
        Time.timeScale = 0;
        endscreen.GetComponent<EndScreenUIManager>().InitializeUI(LevelConfig.StarConfig, PatientsHealed, Points, PatientDeaths, true);
    }
    private void LevelFailed()
    {
        var endscreen = Instantiate(EndScreenPrefab, UICanvas.transform);

        Time.timeScale = 0;
        endscreen.GetComponent<EndScreenUIManager>().InitializeUI(LevelConfig.StarConfig, PatientsHealed, Points, PatientDeaths, false);
    }
}
