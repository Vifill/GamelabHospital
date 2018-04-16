using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float CholeraSeverity;
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
    public GameObject PukeWarningSignPrefab;
    public GameObject PukeParticleEffectPrefab;
    public Transform PukePosition;
    public Transform WarningIconPosition;
    
    public float ConstantDehydrationSpeed;
    public float ConstantHealing;

    public BedManager BedManagerInstance;

    private HydrationController HydrationController;
    private PatientStatusController PatientStatusController;
    private GameObject HydrationUI;
    private GameObject PukeWarningSignInstance;
    private Transform MainCanvasTransform;

    private Coroutine CurrentCoroutineSick;

    private void Start()
    {
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
        var severityDecrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.CholeraSeverityDecreasePerSecond ?? 0;

        if (severityDecrease > 0)
        {
            CholeraSeverity = Mathf.Clamp(CholeraSeverity -= severityDecrease * Time.deltaTime, HealthClampMin, HealthClampMax);
        }

        if (!PatientStatusController.IsHealed && CholeraSeverity <= 0)
        {
            PatientStatusController.IsHealed = true;
        }

        if (!PatientStatusController.IsDead && !PatientStatusController.IsHealed)
        {
            HydrationMeter = Mathf.Clamp(HydrationMeter -= ConstantDehydrationSpeed * Time.deltaTime, HydrationClampMin, HydrationClampMax);
            CholeraSeverity = Mathf.Clamp(CholeraSeverity -= ConstantHealing * Time.deltaTime, HealthClampMin, HealthClampMax);

            if (!PatientStatusController.IsDead && HydrationMeter <= 0)
            {
                PatientStatusController.Death();
            }
        }

        if(PukeWarningSignInstance != null)
        {
            PukeWarningSignInstance.transform.position = Camera.main.WorldToScreenPoint(WarningIconPosition.position);
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
                var severityIncrease = BedSanitationConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= inBed.GetComponent<BedStation>().DirtyMeter)?.CholeraSeverityIncreasePerSecond ?? 0;
                CholeraSeverity = Mathf.Clamp(CholeraSeverity += severityIncrease, HealthClampMin, HealthClampMax);
            }
        }
    }

    private IEnumerator SickCoroutine()
    {
        while(true)
        {
            float odds = ThresholdOddsConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= CholeraSeverity)?.OddsOfExcretion ?? 0.0f;
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
        ReduceCholeraSeverity();
        MakeBedDirty();
        StartPukingAnimation();
        Destroy(PukeWarningSignInstance, 2);
        HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(false);

        Debug.Log($"I'M PUKING!");
    }

    private void StartPukingAnimation()
    {
        var puke = Instantiate(PukeParticleEffectPrefab, PukePosition.position, PukePosition.rotation, PukePosition);
        Destroy(puke, 3f);
    }

    private void ReduceCholeraSeverity()
    {
        CholeraSeverity = Mathf.Clamp(CholeraSeverity -= CholeraConfig.ExcreteCholeraSeverityLoss, HealthClampMin, HealthClampMax);
    }

    private void ReduceHydration()
    {
        float randomVariance = UnityEngine.Random.Range(-CholeraConfig.ExcreteHydrationLossVariance, CholeraConfig.ExcreteHydrationLossVariance);
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