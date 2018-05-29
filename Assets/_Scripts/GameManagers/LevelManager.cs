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
    [HideInInspector]
    public float StartTime;
    [HideInInspector]
    public float TimerClampMin = 0;
    [HideInInspector]
    public float TimerClampMax;
   

    public bool DebugDatagathering;
    public int TestingPeriod;
    public static bool TimeOver { get; private set; }
    public GameObject ClockUIPrefab;
    public GameObject GoalUIPrefab;
    public GameObject EndScreenPrefab;
    public GameObject SunPrefab;
    public LevelConfig LevelConfig;
    public GameObject PlayerAI;
    public GameObject ShiftDoctor;
    public int PatientsHealed { get; private set; } = 0;
    public int PatientDeaths { get; private set; } = 0;
    public float Timer { get; private set; }
    public bool InEndScreen;

    public GameObject ShiftOverCanvas;

    private GameObject Player;

    private int Points;

    //private Datalogic DataLogic = new Datalogic();

    private PlayerDataController PlayerDataController = new PlayerDataController();

    private void Start()
    {
        TimeOver = false;
        InEndScreen = false;
        Player = FindObjectOfType<PlayerActionController>()?.gameObject;
        Time.timeScale = 1;
        UICanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        var timerUI = Instantiate(ClockUIPrefab, UICanvas.transform);
        timerUI.GetComponent<TimerUIManager>().Initialize(this, LevelConfig.LevelTimeSecs);
        var goalUI = Instantiate(GoalUIPrefab, UICanvas.transform);
        UIManager = goalUI.GetComponent<PointsUIManager>();
        UIManager.Initialize();
        UIManager.UpdateUI(PatientsHealed, Vector3.zero);
        StartTime = LevelConfig.LevelTimeSecs;
        TimerClampMax = StartTime;
        Timer = StartTime;
        //var sun = Instantiate(SunPrefab);
        //DayNightCycle = sun.GetComponent<Animator>();
        var multiplier = 1 / (StartTime / 60);
        //DayNightCycle.SetFloat("SpeedMultiplier", multiplier);//endre
    }

    private void Update()
    {
        Timer = Mathf.Clamp(Timer -= Time.deltaTime, TimerClampMin, TimerClampMax);
        
        if (Timer <= 0 && !TimeOver)
        {
            //TimeOver = true;
            //FindObjectOfType<PatientSpawner>().StopSpawning();
            //StartCoroutine(ShiftOver());
            EndLevel();
            //CheckIfAllPatientsAreDone();
            //DayNightCycle.StopPlayback();
            //CheckIfPassed();
        }
    }

    public void EndLevel ()
    {
        if (!TimeOver)
        {
            TimeOver = true;
            FindObjectOfType<PatientSpawner>().StopSpawning();
            StartCoroutine(ShiftOver());
        }
    }

    public void AddPoints (int pPoints, Vector3 pPosition)
    {
        Points += pPoints;
        if (Points < 0)
        {
            Points = 0;
        }
        StartCoroutine(UIManager.UpdateUI(pPoints, pPosition));
    }

    public void AddHealed()
    {
        PatientsHealed++;

        UIManager.UpdateUI(PatientsHealed, Vector3.zero);

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

    private IEnumerator ShiftOver()
    {
        GameObject UIGO = (GameObject)Instantiate(ShiftOverCanvas, FindObjectOfType<Canvas>().transform);
        Transform exit = GameObject.Find("Exit").transform;
        var checkouts = new List<PatientCheckoutController>(FindObjectsOfType<PatientCheckoutController>()).Where(a => a.GetComponent<PatientStatusController>().IsHealed).ToList();
        Time.timeScale = 1.5f;

        //Orderly
        OrderlyController orderlyController = FindObjectOfType<OrderlyController>();
        orderlyController?.CancelOrder();
        if (orderlyController != null && Player != null)
        {
            orderlyController.GetComponent<ActionableActioner>().StopAction();
            orderlyController.CurrentAction?.CancelOrder();
            OrderlyOrder orderlyOrder = new OrderlyOrder(exit.position);
            orderlyOrder.AddAction(new OrderlyMoveAction(exit));
            orderlyController.StartOrder(orderlyOrder);
        }
        else if (orderlyController != null && Player == null)
        {
            orderlyController.GetComponent<ActionableActioner>().StopAction();
            orderlyController.CurrentAction?.CancelOrder();
            OrderlyOrder order = new OrderlyOrder(exit.position);
            if (checkouts.Count > 0)
            {
                order = new OrderlyOrder(checkouts[0].transform.position);
            }

            for (int i = 0; i < checkouts.Count; i++)
            {
                order.AddAction(new OrderlyMoveAction(checkouts[i].transform));
                order.AddAction(new OrderlyInteractionAction(checkouts[i]));
            }
            order.AddAction(new OrderlyMoveAction(exit));

            orderlyController.StartOrder(order);
        }
        


        //shift doctor change
        var EmptyBeds = new List<BedController>(FindObjectsOfType<BedController>()).Where(a => !a.IsReserved).ToList();
        var PatientBeds = new List<BedController>(FindObjectsOfType<BedController>()).Where(a => a.IsReserved /* &&(a.PatientInBed?.GetComponentInChildren<PatientStatusController>()?.IsHealed ?? false)*/).ToList();
        GameObject bedToMoveTo;
        //GameObject
        if (PatientBeds.Any())
        {
            bedToMoveTo = PatientBeds[Random.Range(0, PatientBeds.Count() - 1)].gameObject;
        }
        else
        {
            bedToMoveTo = EmptyBeds[Random.Range(0, EmptyBeds.Count() - 1)].gameObject;
        }

        GameObject shiftDoctorGO = (GameObject)Instantiate(ShiftDoctor, exit.position, Quaternion.identity);
        shiftDoctorGO.SetActive(true);
        OrderlyController shiftDoctorController = shiftDoctorGO.GetComponent<OrderlyController>();
        OrderlyOrder shiftOrder = new OrderlyOrder(bedToMoveTo.transform.position);
        shiftOrder.AddAction(new OrderlyMoveAction(bedToMoveTo.transform));

        OrderlyController playerController = null;
        //Player
        if (Player != null)
        {
            GameObject CheckoutPlayer = Instantiate(PlayerAI, Player.transform.position, Player.transform.rotation);
            Player.GetComponent<ActionableActioner>().StopAction();
            Destroy(Player);
            playerController = CheckoutPlayer.GetComponent<OrderlyController>();
            OrderlyOrder order = new OrderlyOrder(exit.position);
            if (checkouts.Count > 0)
            {
                order = new OrderlyOrder(checkouts[0].transform.position);
            }

            for (int i = 0; i < checkouts.Count; i++)
            {
                order.AddAction(new OrderlyMoveAction(checkouts[i].transform));
                order.AddAction(new OrderlyInteractionAction(checkouts[i]));
            }
            order.AddAction(new OrderlyMoveAction(exit));

            playerController.StartOrder(order);
        }
        shiftDoctorController.StartOrder(shiftOrder);


        while (playerController?.CurrentOrder != null || shiftDoctorController?.CurrentOrder != null || orderlyController?.CurrentOrder != null)
        {
            yield return null;
        }




        yield return new WaitForSeconds(0.5f);
        Destroy(UIGO);
        LevelPassed();
    }

    private void LevelPassed()
    {
        var endscreen = Instantiate(EndScreenPrefab, UICanvas.transform);
        var collectables = FindObjectsOfType<HoverOverInfoscreens>().OrderBy(a => a.index).Select(a => a.isFound).ToList();
        PlayerDataController.SaveLevelData(new LevelModel(LevelConfig.LevelNumber, Points, collectables));
        Time.timeScale = 0;
        endscreen.GetComponent<EndScreenUIManager>().InitializeUI(LevelConfig.StarConfig, PatientsHealed, Points, PatientDeaths, true);
        InEndScreen = true;
    }
    private void LevelFailed()
    {
        var endscreen = Instantiate(EndScreenPrefab, UICanvas.transform);

        Time.timeScale = 0;
        endscreen.GetComponent<EndScreenUIManager>().InitializeUI(LevelConfig.StarConfig, PatientsHealed, Points, PatientDeaths, false);
        InEndScreen = true;
    }
}
