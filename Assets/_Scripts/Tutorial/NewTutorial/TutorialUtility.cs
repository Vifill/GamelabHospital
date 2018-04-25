using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialUtility : MonoBehaviour
{
    public static bool TimeFreeze = false;

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
            //UnreservedBeds = new List<BedController>(FindObjectsOfType<BedController>().Where(a => !a.IsReserved));

            //foreach (var bed in UnreservedBeds)
            //{
            //    bed.IsReserved = true;
            //}
            Debug.Log("freeze spawn " + pState);
            patientSpawner.StopSpawning();
        }
        else
        {
            //foreach (var bed in UnreservedBeds)
            //{
            //    bed.IsReserved = false;
            //}
            Debug.Log("freeze spawn " + pState);
            patientSpawner.StartSpawnCoroutine();
        }
    }

    public static void SetExcretionFreeze(bool pState)
    {
        var patientsInScene = FindObjectsOfType<HealthController>();
        if (pState)
        {
            foreach (var patient in patientsInScene)
            {
                patient.StopCoroutine(patient.GetSickCoroutine());
            }
        }
        else
        {
            foreach (var patient in patientsInScene)
            {
                patient.StartSickCoroutine();
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
            TimeFreeze = true;
            levelManager.TimerClampMin = levelManager.Timer;
            levelManager.TimerClampMax = levelManager.Timer;
        }
        else
        {
            TimeFreeze = false;
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
            patient.HydrationMeter = pHydrationAmount;
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
        var timerUI = FindObjectOfType<TimerUIManager>().gameObject;

        timerUI.SetActive(pState);
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
}
