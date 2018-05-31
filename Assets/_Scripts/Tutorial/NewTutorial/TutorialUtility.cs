using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialUtility : MonoBehaviour
{
    public bool TimeFreeze = false;
    private static TutorialUtility tutorialEntity;
    [HideInInspector]
    public GameObject TimerUI;
    [HideInInspector]
    public float ConstDehydration;

    public static TutorialUtility instance
    {
        get
        {
            if (!tutorialEntity)
            {
                tutorialEntity = FindObjectOfType(typeof(TutorialUtility)) as TutorialUtility;
            }

            return tutorialEntity;
        }
    }

    private void Initialize()
    {

    }

    public static void SetHydrationFreeze(bool pState)
    {
        var healthControllers = FindObjectsOfType<HealthController>();

        foreach (var controller in healthControllers)
        {
            if (pState)
            {
                controller.HydrationClampMin = controller.HydrationMeter;
                controller.HydrationClampMax = controller.HydrationMeter;
            }
            else
            {
                controller.HydrationClampMin = controller.MinHydration;
                controller.HydrationClampMax = controller.MaxHydration;
            }
        }
    }

    public static void SetSpawnFreeze(bool pState)
    {
        var patientSpawner = FindObjectOfType<PatientSpawner>();

        if (pState)
        {
            Debug.Log("freeze spawn " + pState);
            patientSpawner.StopSpawning();
        }
        else
        {
            Debug.Log("freeze spawn " + pState);
            patientSpawner.StartSpawnCoroutine();
        }
    }
    
    public static void SetFreezeExcretion(bool pState)
    {
        instance.StartCoroutine("FreezeExcretion", pState);
    }
    
    private IEnumerator FreezeExcretion(bool pState)
    {
        yield return new WaitForSeconds(0.1f);
        var patientsInScene = FindObjectsOfType<HealthController>().ToList().Where(a => a.GetComponent<HydrationController>().IsActionActive);
        if (pState)
        {
            foreach (var patient in patientsInScene)
            {
                patient?.StopSickCoroutine();
            }
        }
        else
        {
            foreach (var patient in patientsInScene)
            {
                if (patient.IsInitialized)
                {
                    patient?.StartSickCoroutine();
                }
            }
        }
    }
    
    public static void SetHealthFreeze(bool pState)
    {
        var healthControllers = FindObjectsOfType<HealthController>();

        foreach (var controller in healthControllers)
        {
            if (pState)
            {
                controller.HealthClampMin = controller.Health;
                controller.HealthClampMax = controller.Health;
            }
            else
            {
                controller.HealthClampMin = controller.MinHealth;
                controller.HealthClampMax = controller.MaxHealth;
            }
        }
    }

    public static void SetPlayerSanitationFreeze(bool pState)
    {
        var sanitationControllers = FindObjectsOfType<SanitationController>();

        foreach (var controller in sanitationControllers)
        {
            if (pState)
            {
                controller.SanitationClampMin = controller.Sanitation;
                controller.SanitationClampMax = controller.Sanitation;
            }
            else
            {
                controller.SanitationClampMin = controller.MinSanitation;
                controller.SanitationClampMax = controller.MaxSanitation;
            }
        }
    }

    public static void SetPlayerSanitation(float pValue)
    {
        var sanitationControllers = FindObjectsOfType<SanitationController>();

        foreach (var controller in sanitationControllers)
        {
            controller.MakePlayerDirty(pValue);
        }
    }

    public static void SetBedSanitationFreeze(bool pState)
    {
        var bedStations = FindObjectsOfType<BedStation>();

        foreach (var controller in bedStations)
        {
            if (pState)
            {
                controller.DirtynessClampMin = controller.DirtyMeter;
                controller.DirtynessClampMax = controller.DirtyMeter;
            }
            else
            {
                controller.DirtynessClampMin = controller.MinDirtyness;
                controller.DirtynessClampMax = controller.MaxDirtyness;
            }
        }
    }

    public static void SetTimeFreeze(bool pState)
    {
        var levelManager = FindObjectOfType<LevelManager>();

        if (pState)
        {
            instance.TimeFreeze = true;
            levelManager.TimerClampMin = levelManager.Timer;
            levelManager.TimerClampMax = levelManager.Timer;
        }
        else
        {
            instance.TimeFreeze = false;
            levelManager.TimerClampMin = 0;
            levelManager.TimerClampMax = levelManager.StartTime;
        }
    }

    public static void ForcePatientExcretion()
    {
        var patient = FindObjectOfType<HealthController>();

        patient.ForceExcretion();
    }

    public static void SetPatientHydration(float pHydrationAmount)
    {
        var patients = FindObjectsOfType<HealthController>();

        foreach (var patient in patients)
        {
            //patient.HydrationMeter = pHydrationAmount;
            patient.SetHydration(pHydrationAmount);
        }
    }

    public static void SetPatientHealth(float pHealthAmount)
    {
        var patients = FindObjectsOfType<HealthController>();

        foreach (var patient in patients)
        {
            patient.Health = pHealthAmount;
        }
    }

    public static void SetTimerUIAsActive(bool pState)
    {
        if (!pState)
        {
            instance.TimerUI = FindObjectOfType<TimerUIManager>().gameObject;
        }
        //var timerUI = FindObjectOfType<TimerUIManager>().gameObject;

        instance.TimerUI?.SetActive(pState);
    }

    public static void SetPlayerMovementFreeze(bool pState)
    {
        var player = FindObjectOfType<MovementController>();

        if (pState)
        {
            player.StopMovement();
        }
        else
        {
            player.StartMovement();
        }
    }

    public static void ForceEndLevel()
    {
        var levelManager = FindObjectOfType<LevelManager>();

        levelManager.EndLevel();
    }

    public static void ForcePatientSickness()
    {
        var patient = FindObjectOfType<HealthController>();

        patient.ForceSickness();
    }

    public static void SetConstantDehydrationFreeze(bool pState)
    {
        var patient = FindObjectOfType<HealthController>();

        if (pState)
        {
            instance.ConstDehydration = patient.ConstantDehydrationSpeed;

            patient.ConstantDehydrationSpeed = 0;
        }

        else
        {
            patient.ConstantDehydrationSpeed = instance.ConstDehydration;
        }
    }

    public static void SetActionablesActive(bool pState)
    {
        foreach (var actionable in FindObjectsOfType<Actionable>())
        {
            actionable.IsActionActive = pState;
        }
    }

    public static void SetBedSanitationUIActive(bool pState)
    {
        FindObjectsOfType<BedStation>().ToList().ForEach(a => a.DirtyBarInstance.SetActive(pState));
    }

    public static void SetBucketTableActive(bool pState)
    {
        FindObjectsOfType<TableStation>().ToList().FirstOrDefault(a => a.StartingTableObjectPrefab != null).IsActionActive = pState;
    }
}
