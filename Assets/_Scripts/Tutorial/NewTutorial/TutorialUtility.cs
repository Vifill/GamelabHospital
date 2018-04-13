using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUtility : MonoBehaviour 
{

	private void Start() 
	{
		
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
}
