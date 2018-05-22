using UnityEngine;
using System.Collections;

public class ActionableActionerTutorial : ActionableActioner
{
    protected override void OnSuccess()
    {
        if(CurrentAction is TableStation && (CurrentAction as TableStation).TableObject == null && GetComponent<ToolController>().GetCurrentToolName() == ToolName.Bucket)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneFinishingTutorialQueue);
        }

        if (CurrentAction is PickupStationController && (CurrentAction as PickupStationController).ToolObject.GetComponent<ToolBase>().ToolName == ToolName.Water)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneGetWater);
            EventManager.TriggerEvent(EventManager.EventCodes.DoneGetWaterLvl2);
        }
        if (CurrentAction is HydrationController)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneHydration);
            EventManager.TriggerEvent(EventManager.EventCodes.DoneHydrationLvl2);
        }
        if (CurrentAction is PatientCheckoutController)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneCheckOut);
        }
        if (CurrentAction is WashingStation)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneCleanDoctor);
        }

        base.OnSuccess();
    }
}
