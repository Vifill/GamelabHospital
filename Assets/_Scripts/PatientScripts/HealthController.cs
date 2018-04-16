using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float Health;
    public float HydrationMeter;

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
    private GameObject HydrationUI;
    private Transform MainCanvasTransform;

    private void Start()
    {
        LevelManager = FindObjectOfType<LevelManager>();
        MainCanvasTransform = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        SpawnHydrationUI();
        PatientStatusController = GetComponent<PatientStatusController>();
        HydrationController = GetComponent<HydrationController>();
        StartCoroutine(SickCoroutine());
        StartCoroutine(BedSanitationCheckCoroutine());
    }

    private void SpawnHydrationUI()
    {
        HydrationUI = Instantiate(HydrationUIPrefab, MainCanvasTransform);
        HydrationUI.GetComponent<HydrationUIManager>().InitializeHydrationUI(this);
    }

    private void Update()
    {
        var healthIncrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;

        if (healthIncrease > 0)
        {
            Health += healthIncrease * Time.deltaTime;
        }

        if (!PatientStatusController.IsHealed && Health >= 100)
        {
            PatientStatusController.IsHealed = true;
        }

        if (!PatientStatusController.IsDead && !PatientStatusController.IsHealed)
        {
            HydrationMeter -= ConstantDehydrationSpeed * Time.deltaTime;
            Health += ConstantHealing * Time.deltaTime;

            if (!PatientStatusController.IsDead && HydrationMeter <= 0)
            {
                PatientStatusController.Death();

            }
        }
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
                Health -= healthDecrease;
                Health = Mathf.Clamp(Health, 0, 100);
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
        HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(true);

        Invoke("Excrete", 5);
    }

    private void Excrete()
    {
        ReduceHydration();
        IncreaseHealthWhenExcreting();
        MakeBedDirty();
        StartPukingAnimation();
        HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(false);

        Debug.Log($"I'M PUKING!");
    }

    private void StartPukingAnimation()
    {
        var puke = Instantiate(PukeParticleEffectPrefab, PukePosition.position, PukePosition.rotation, PukePosition);
        Destroy(puke, 3f);
    }

    private void IncreaseHealthWhenExcreting()
    {
        Health += CholeraConfig.ExcreteHealthIncrease;
    }

    private void ReduceHydration()
    {
        float randomVariance = UnityEngine.Random.Range(CholeraConfig.ExcreteHydrationLossVariance, CholeraConfig.ExcreteHydrationLossVariance*2);
        float hydrationLossModifier = HydrationConfig.HydrationLowerThreshold >= HydrationMeter ? HydrationConfig.HydrationLowerThresholdModifier : 1;
        HydrationMeter -= (CholeraConfig.ExcreteHydrationLoss + randomVariance) * hydrationLossModifier;
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