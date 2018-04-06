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
                Mathf.Clamp(controller.HydrationMeter, controller.MinHydration, controller.HydrationMeter);
            }
            else
            {
                Mathf.Clamp(controller.HydrationMeter, controller.MinHydration, controller.MaxHydration);
            }
        }
    }
}
