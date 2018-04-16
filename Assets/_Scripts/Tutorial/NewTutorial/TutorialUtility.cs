using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialUtility : MonoBehaviour 
{
    private PatientSpawner PatientSpawner;
    private float OriginalSpawnRate;
    private List<BedController> UnreservedBeds;

	private void Start() 
	{
        PatientSpawner = FindObjectOfType<PatientSpawner>();
        OriginalSpawnRate = PatientSpawner.SpawnConfig.SpawnRate;
	}
	
	private void Update() 
	{
		
	}

    public void SetHydrationFreeze(bool pState)
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

    public void SetSpawnFreeze(bool pState)
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
            PatientSpawner.StopSpawning();
        }
        else
        {
            //foreach (var bed in UnreservedBeds)
            //{
            //    bed.IsReserved = false;
            //}
            Debug.Log("freeze spawn " + pState);
            PatientSpawner.StartSpawnCoroutine();
        }
    }

    public void SetExcretionFreeze(bool pState)
    {
        var patientsInScene = FindObjectsOfType<HealthController>();
        if (pState)
        {
            foreach (var patient in patientsInScene)
            {
                StopCoroutine(patient.GetSickCoroutine());
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

    public void SetHealthFreeze(bool pState)
    {
        var healthControllers = FindObjectsOfType<HealthController>();

        foreach (var controller in healthControllers)
        {
            if (pState)
            {
                controller.HealthClampMin = controller.CholeraSeverity;
                controller.HealthClampMax = controller.CholeraSeverity;
            }
            else
            {
                controller.HealthClampMin = controller.MinHealth;
                controller.HealthClampMax = controller.MaxHealth;
            }
        }
    }

    public void SetPlayerSanitationFreeze(bool pState)
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

    public void SetBedSanitationFreeze(bool pState)
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
}
