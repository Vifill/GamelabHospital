using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float CholeraSeverity;
    public float HydrationMeter;

    public HydrationConfig HydrationConfig;
    public CholeraConfig CholeraConfig;
    public CholeraThresholdOddsConfig ThresholdOddsConfig;
    public SanitationConfig BedSanitationConfig;
    public HydrationHealingConfig HydrationHealingConfig;

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

    private void Start()
    {
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
        var severityDecrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.CholeraSeverityDecreasePerSecond ?? 0;

        if (severityDecrease > 0)
        {
            CholeraSeverity -= severityDecrease * Time.deltaTime;
        }

        if (!PatientStatusController.IsHealed && CholeraSeverity <= 0)
        {
            PatientStatusController.IsHealed = true;
        }

        if (!PatientStatusController.IsDead && !PatientStatusController.IsHealed)
        {
            HydrationMeter -= ConstantDehydrationSpeed * Time.deltaTime;
            CholeraSeverity -= ConstantHealing * Time.deltaTime;

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

    private IEnumerator BedSanitationCheckCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            var inBed = BedManagerInstance?.Beds.SingleOrDefault(a => a.PatientInBed == gameObject);            
            if(inBed != null)
            {
                var severityIncrease = BedSanitationConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= inBed.GetComponent<BedStation>().DirtyMeter)?.CholeraSeverityIncreasePerSecond ?? 0;
                CholeraSeverity += severityIncrease;
                CholeraSeverity = Mathf.Clamp(CholeraSeverity, 0, 100);
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
        CholeraSeverity -= CholeraConfig.ExcreteCholeraSeverityLoss;
    }

    private void ReduceHydration()
    {
        float randomVariance = UnityEngine.Random.Range(-CholeraConfig.ExcreteHydrationLossVariance, CholeraConfig.ExcreteHydrationLossVariance);
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