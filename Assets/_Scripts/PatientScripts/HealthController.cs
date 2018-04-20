using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float Health;
    public float HydrationMeter;

    public float MaxHydration = 100;
    public float MinHydration = 0;

    public float MaxHealth = 100;
    public float MinHealth = 0;

    [HideInInspector]
    public float HydrationClampMax;
    [HideInInspector]
    public float HydrationClampMin;
    [HideInInspector]
    public float HealthClampMax;
    [HideInInspector]
    public float HealthClampMin;


    [HideInInspector]
    public HydrationConfig HydrationConfig;
    [HideInInspector]
    public CholeraConfig CholeraConfig;
    [HideInInspector]
    public CholeraThresholdOddsConfig ThresholdOddsConfig;
    [HideInInspector]
    public SanitationThresholdConfig BedSanitationConfig;
    [HideInInspector]
    public HydrationHealingConfig HydrationHealingConfig;
    [HideInInspector]
    public SanitationThresholdConfig DoctorSanitationThresholdConfig;

    public GameObject HydrationUIPrefab;
    public GameObject PukeParticleEffectPrefab;
    public Transform PukePosition;
    
    public float ConstantDehydrationSpeed;
    public float ConstantHealing;

    public BedManager BedManagerInstance;

    private LevelManager LevelManager;
    private HydrationController HydrationController;
    private PatientStatusController PatientStatusController;
    [HideInInspector]
    public GameObject HydrationUI;
    private Transform MainCanvasTransform;

    private Coroutine CurrentCoroutineSick;

    private void Start()
    {
        LevelManager = FindObjectOfType<LevelManager>();
        HydrationClampMax = MaxHydration;
        HydrationClampMin = MinHydration;
        HealthClampMax = MaxHealth;
        HealthClampMin = MinHealth;
        MainCanvasTransform = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        SpawnHydrationUI();
        PatientStatusController = GetComponent<PatientStatusController>();
        HydrationController = GetComponent<HydrationController>();
        StartSickCoroutine();
        StartCoroutine(BedSanitationCheckCoroutine());
    }

    private void SpawnHydrationUI()
    {
        HydrationUI = Instantiate(HydrationUIPrefab, MainCanvasTransform);
        HydrationUI.GetComponent<HydrationUIManager>().InitializeHydrationUI(this);
    }

    private void Update()
    {
        if (!LevelManager.TimeOver)
        {
            var healthIncrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;

            if (healthIncrease > 0)
            {
                Health = Mathf.Clamp(Health += healthIncrease * Time.deltaTime, HealthClampMin, HealthClampMax);
            }

            if (!PatientStatusController.IsHealed && Health >= 100)
            {
                PatientStatusController.IsHealed = true;
                HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(false);
            }

            if (!PatientStatusController.IsDead && !PatientStatusController.IsHealed)
            {
                HydrationMeter = Mathf.Clamp(HydrationMeter -= ConstantDehydrationSpeed * Time.deltaTime, HydrationClampMin, HydrationClampMax);
                Health = Mathf.Clamp(Health += ConstantHealing * Time.deltaTime, HealthClampMin, HealthClampMax);

                if (!PatientStatusController.IsDead && HydrationMeter <= 0)
                {
                    PatientStatusController.Death();
                }
            }
        }
    }

    public Coroutine GetSickCoroutine()
    {
        return CurrentCoroutineSick;
    }

    public void StartSickCoroutine()
    {
        CurrentCoroutineSick = StartCoroutine(SickCoroutine());
    }

    private IEnumerator BedSanitationCheckCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            var inBed = BedManagerInstance?.Beds.SingleOrDefault(a => a.PatientInBed == gameObject);            
            if(inBed != null)
            {
                var healthDecrease = BedSanitationConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= inBed.GetComponent<BedStation>().DirtyMeter)?.HealthDecreasePerSecond ?? 0;
                Health = Mathf.Clamp(Health -= healthDecrease, HealthClampMin, HealthClampMax);
            }
        }
    }

    private IEnumerator SickCoroutine()
    {
        while(true)
        {
            float odds = ThresholdOddsConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= Health)?.OddsOfExcretion ?? 0.0f;
            if(UnityEngine.Random.Range(0,100) < odds && HydrationController.IsActionActive)
            {
                StartFeelingSick();
                yield return new WaitForSeconds(CholeraConfig.ExcreteCooldown);
            }
            else if (!(UnityEngine.Random.Range(0,100) < odds))
            {
                yield return new WaitForSeconds(CholeraConfig.CholeraCheckRate);
            }
        }
    }

    private void StartFeelingSick()
    {
        if (!PatientStatusController.IsHealed)
        {
            HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(true);

            Invoke("Excrete", 5);
        }
    }

    private void Excrete()
    {
        if (!PatientStatusController.IsHealed)
        {
            ReduceHydration();
            IncreaseHealthWhenExcreting();
            MakeBedDirty();
            StartPukingAnimation();
            HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(false);

            Debug.Log($"I'M PUKING!");
        }
    }

    private void StartPukingAnimation()
    {
        var puke = Instantiate(PukeParticleEffectPrefab, PukePosition.position, PukePosition.rotation, PukePosition);
        Destroy(puke, 3f);
    }

    private void IncreaseHealthWhenExcreting()
    {
        Health = Mathf.Clamp(Health += CholeraConfig.ExcreteHealthIncrease, HealthClampMin, HealthClampMax);
    }

    private void ReduceHydration()
    {
        float randomVariance = UnityEngine.Random.Range(CholeraConfig.ExcreteHydrationLossVariance, CholeraConfig.ExcreteHydrationLossVariance*2);
        float hydrationLossModifier = HydrationConfig.HydrationLowerThreshold >= HydrationMeter ? HydrationConfig.HydrationLowerThresholdModifier : 1;
        HydrationMeter = Mathf.Clamp(HydrationMeter -= (CholeraConfig.ExcreteHydrationLoss + randomVariance) * hydrationLossModifier, HydrationClampMin, HydrationClampMax);
    }

    private void MakeBedDirty()
    {
        var beds = BedManagerInstance?.Beds;
            
        var patientInBed = BedManagerInstance?.Beds.SingleOrDefault(a => a.PatientInBed == gameObject);        

        if (patientInBed != null)
        {
            patientInBed.BedStation.IncreaseDirtyMeter(CholeraConfig.ExcreteBedDirtyIncrease);
        }
        else
        {
           // print("<color=magenta> puked but was not in bed </color>");
        }
    }
}