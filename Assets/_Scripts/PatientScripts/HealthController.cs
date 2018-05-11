using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float Health;
    public float HydrationMeter { get; private set; }

    public float MaxHydration = 100;
    public float MinHydration = 0;

    public float MaxHealth = 100;
    public float MinHealth = 0;

    [HideInInspector]
    public float HydrationClampMax = 100;
    [HideInInspector]
    public float HydrationClampMin = 0;
    [HideInInspector]
    public float HealthClampMax = 100;
    [HideInInspector]
    public float HealthClampMin = 0;

    [HideInInspector]
    public bool FreezePuke;

    [HideInInspector]
    public HydrationConfig HydrationConfig;
    [HideInInspector]
    public CholeraConfig CholeraConfig;
    [HideInInspector]
    public CholeraThresholdOddsConfig ThresholdOddsConfig;
    //[HideInInspector]
    //public SanitationThresholdConfig BedSanitationConfig;
    [HideInInspector]
    public HydrationHealingConfig HydrationHealingConfig;
    [HideInInspector]
    public SanitationThresholdConfig DoctorSanitationThresholdConfig;

    public GameObject HydrationUIPrefab;
    public GameObject PukeParticleEffectPrefab;
    public Transform PukePosition;
    
    public float ConstantDehydrationSpeed;
    //public float ConstantHealing;

    public BedManager BedManagerInstance;

    private HydrationController HydrationController;
    private PatientStatusController PatientStatusController;
    [HideInInspector]
    public GameObject HydrationUI;
    private Transform MainCanvasTransform;

    private Coroutine CurrentCoroutineSick;
    private Animator PatientAnimator;
    private Animator PatientPrefabAnimator;

    private float ExcreteTimeOffset;
    private float HydrationChangeModifier { get { return HydrationConfig.HydrationLossModifier.Evaluate(HydrationMeter / 100); } }
    private float HealthIncrease;

    [Header("Hydration Combo Parameters")]
    public float MaxHealthIncresePerSecond;
    public float ComboBonusAmount;
    public float ComboBonusTime;
    public float ComboRedemptionTime;


    public void Initialize()
    {
        //HydrationMeter = 100;
        StartCoroutine(GetPatientAnimator());
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

    private IEnumerator GetPatientAnimator()
    {
        yield return new WaitForEndOfFrame();

        PatientAnimator = transform.Find(Constants.Highlightable).GetComponentInChildren<Animator>();
        PatientPrefabAnimator = GetComponent<Animator>();
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
            if (HealthIncrease > 0)
            {
                Health = Mathf.Clamp(Health += HealthIncrease * Time.deltaTime, HealthClampMin, HealthClampMax);
            }

            if (!PatientStatusController.IsHealed && Health >= 100)
            {
                PatientStatusController.IsHealed = true;
                HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(false);
            }

            if (!PatientStatusController.IsDead && !PatientStatusController.IsHealed)
            {
                SetHydration(HydrationMeter - (ConstantDehydrationSpeed * HydrationChangeModifier * Time.deltaTime));

                //Health = Mathf.Clamp(Health += ConstantHealing * Time.deltaTime, HealthClampMin, HealthClampMax);

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
            var bed = BedManagerInstance?.Beds.SingleOrDefault(a => a.PatientInBed == gameObject)?.GetComponent<BedStation>();            
            if(bed != null)
            {
                var healthDecrease = bed.BedSanitationThresholds.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= bed.DirtyMeter)?.HealthDecreasePerSecond ?? 0;
                Health = Mathf.Clamp(Health -= healthDecrease, HealthClampMin, HealthClampMax);
            }
        }
    }

    private IEnumerator SickCoroutine()
    {
        ExcreteTimeOffset = UnityEngine.Random.Range(-ThresholdOddsConfig.RandomRangeOffset, ThresholdOddsConfig.RandomRangeOffset);
        float timeCounter = 0;

        while (true)
        {
            float targetTime = ThresholdOddsConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= Health)?.TimeToExcrete ?? 0.0f;
            targetTime += ExcreteTimeOffset;
            timeCounter += Time.deltaTime;
            if(timeCounter >= targetTime)
            {
                StartFeelingSick();
                break;
            }
            yield return null;
        }
        yield return null;
    }

    private void StartFeelingSick()
    {
        if (!PatientStatusController.IsHealed)
        {
            SetExcreteWarning();
            Invoke("Excrete", 5);
        }
    }

    private void SetExcreteWarning()
    {
        HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(true);
        PatientAnimator.SetTrigger(Constants.AnimationParameters.PatientPukeWarning);
    }

    protected virtual void Excrete()
    {
        if (!PatientStatusController.IsHealed)
        {
            ReduceHydration();
            IncreaseHealthWhenExcreting();
            MakeBedDirty();
            StartPukingAnimation();
            HydrationUI.GetComponent<HydrationUIManager>().SetExcreteWarning(false);
            // puke animation trigger
            PatientAnimator.SetTrigger(Constants.AnimationParameters.PatientPuke);
            PatientPrefabAnimator.SetTrigger(Constants.AnimationParameters.PatientPuke);
            Debug.Log($"I'M PUKING!");
        }
        StartSickCoroutine();
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
        var loss = CholeraConfig.ExcreteHydrationLoss * HydrationChangeModifier;
        print(loss);
        SetHydration(HydrationMeter - loss);
    }

    private void MakeBedDirty()
    {
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

    public void SetHydration (float pValue)
    {
        var prevHydration = HydrationMeter;

        HydrationMeter = Mathf.Clamp(pValue, HydrationClampMin, HydrationClampMax);

        var threshold = HydrationHealingConfig.ListOfThresholds.LastOrDefault().ThresholdOfActivation;

        if (prevHydration >= threshold && HydrationMeter < threshold)
        {
            //StartCoroutine(ComboRedemptionCheck());
            StartCoroutine(ComboBonusSetter(ComboRedemptionTime, -ComboBonusAmount));
        }
        else if (prevHydration < threshold && HydrationMeter >= threshold)
        {
            HealthIncrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;
            StartCoroutine(ComboBonusSetter(ComboBonusTime, ComboBonusAmount));
        }
        else if (prevHydration < threshold && HydrationMeter < threshold)
        {
            HealthIncrease = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;
        }
    }
    
    private IEnumerator ComboBonusSetter(float pTime, float pAmount)
    {
        while (true)
        {
            yield return new WaitForSeconds(pTime);
            var minHPS = HydrationHealingConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= HydrationMeter)?.HealthIncreasePerSecond ?? 0;

            if (pAmount < 0)
            {
                pTime -= 1;
            }
            else if (pAmount > 0)
            {
                pTime += 1;
            }

            HealthIncrease = Mathf.Clamp(HealthIncrease + pAmount, minHPS, MaxHealthIncresePerSecond);
            //print("healthincrease is now = " + HealthIncrease + " for " + gameObject);
        }
    }

    public void ForceExcretion()
    {
        Excrete();
    }

    public void ForceSickness()
    {
        StartFeelingSick();
    }
}