using UnityEngine;
using System.Collections;

public class ActionableActionerTutorial : ActionableActioner
{

    protected override void OnSuccess()
    {
        base.OnSuccess();

        if(CurrentAction is PickupStationController && (CurrentAction as PickupStationController).ToolObject.GetComponent<ToolBase>().ToolName == ToolName.Water)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneGetWater);
        }
        if(CurrentAction is HydrationController)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneHydration);
        }
        else if(CurrentAction is PatientCheckoutController)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneCheckOut);
        }
    }



}
