using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUtility : MonoBehaviour 
{
    private PatientSpawner PatientSpawner;
    private float OriginalSpawnRate;

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
        if (pState)
        {
            PatientSpawner.SpawnConfig.SpawnRate = 0;
        }
        else
        {
            PatientSpawner.SpawnConfig.SpawnRate = OriginalSpawnRate;
        }
    }

    public void SetExcretionFreeze(bool pState)
    {

    }

    public void SetCholeraSeverityFreeze(bool pState)
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
