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
    public GameObject PlayerAI;
    public GameObject ShiftDoctor;
    public int PatientsHealed { get; private set; } = 0;
    public int PatientDeaths { get; private set; } = 0;
    public float Timer { get; private set; }

    public GameObject ShiftOverCanvas;

    private GameObject Player;

    private int Points;

    private Datalogic DataLogic = new Datalogic();

    private PlayerDataController PlayerDataController = new PlayerDataController();

    private void Start()
    {
        Player = FindObjectOfType<PlayerActionController>().gameObject;
        Time.timeScale = 1;
        UICanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        var timerUI = Instantiate(ClockUIPrefab, UICanvas.transform);
        timerUI.GetComponent<TimerUIManager>().Initialize(this, LevelConfig.LevelTimeSecs);
        var goalUI = Instantiate(GoalUIPrefab, UICanvas.transform);
        UIManager = goalUI.GetComponent<PointsUIManager>();
        UIManager.Initialize();
        UIManager.UpdateUI(PatientsHealed, Vector3.zero);
        StartTime = LevelConfig.LevelTimeSecs;
        Timer = StartTime;
        var sun = Instantiate(SunPrefab);
        DayNightCycle = sun.GetComponent<Animator>();
        var multiplier = 1 / (StartTime / 60);
        DayNightCycle.SetFloat("SpeedMultiplier", multiplier);//endre
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        Mathf.Clamp(Timer, 0, StartTime);
        
        if (Timer <= 0 && !TimeOver)
        {
            TimeOver = true;
            FindObjectOfType<PatientSpawner>().StopSpawning();
            StartCoroutine(ShiftOver());
            //CheckIfAllPatientsAreDone();
            //DayNightCycle.StopPlayback();
            //CheckIfPassed();
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
        //GameObject tempCanvas = Instantiate(ShiftOverCanvas, transfor);
        GameObject UIGO = (GameObject)Instantiate(ShiftOverCanvas, FindObjectOfType<Canvas>().transform);
        Transform exit = GameObject.Find("Exit").transform;

        //Orderly
        OrderlyController ordelyController = FindObjectOfType<OrderlyController>();
        OrderlyOrder orderlyOrder = new OrderlyOrder(exit.position);
        orderlyOrder.AddAction(new OrderlyMoveAction(exit));
        ordelyController.StartOrder(orderlyOrder);


        //shift doctor change

        var EmptyBeds = new List<BedController>(FindObjectsOfType<BedController>()).Where(a => !a.IsReserved).ToList();
        var PatientBeds = new List<BedController>(FindObjectsOfType<BedController>()).Where(a => a.IsReserved).ToList();

        GameObject bedToMoveTo;
        //GameObject
        if (PatientBeds.Any())
        {
            bedToMoveTo = PatientBeds[Random.Range(0, EmptyBeds.Count())].gameObject;
        }
        else
        {
            bedToMoveTo = EmptyBeds[Random.Range(0, EmptyBeds.Count())].gameObject;
        }

        GameObject shiftDoctorGO = (GameObject)Instantiate(ShiftDoctor, exit.position, Quaternion.identity);
        shiftDoctorGO.SetActive(true);
        OrderlyController shiftDoctorController = shiftDoctorGO.GetComponent<OrderlyController>();
        OrderlyOrder shiftOrder = new OrderlyOrder(bedToMoveTo.transform.position);
        shiftOrder.AddAction(new OrderlyMoveAction(bedToMoveTo.transform));


        //Player
        if (Player == null)
        {
            GameObject CheckoutPlayer = Instantiate(PlayerAI, Player.transform.position, Player.transform.rotation);
            Destroy(Player);


            var checkouts = new List<PatientCheckoutController>(FindObjectsOfType<PatientCheckoutController>()).Where(a => a.GetComponent<PatientStatusController>().IsHealed).ToList();
            var playerController = CheckoutPlayer.GetComponent<OrderlyController>();
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

            shiftDoctorController.StartOrder(shiftOrder);
            playerController.StartOrder(order);

            Time.timeScale = 1.5f;

            while (playerController.CurrentOrder != null || shiftDoctorController.CurrentOrder != null)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            Destroy(UIGO);
            LevelPassed();
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
