using UnityEngine;
using System.Collections;

public class ActionableActionerTutorial : ActionableActioner
{
    protected override void OnSuccess()
    {
        if (CurrentAction is TableStation && (CurrentAction as TableStation).TableObject != null && (CurrentAction as TableStation).TableObject.GetComponent<ToolBase>().ToolName == ToolName.Bucket)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneGetBucket);
        }
        if (CurrentAction is TableStation && (CurrentAction as TableStation).TableObject == null && GetComponent<ToolController>().GetCurrentToolName() == ToolName.Bucket)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneReturnBucket);
        }

        base.OnSuccess();

        if(CurrentAction is PickupStationController && (CurrentAction as PickupStationController).ToolObject.GetComponent<ToolBase>().ToolName == ToolName.Water)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneGetWater);
        }
        if(CurrentAction is HydrationController)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneHydration);
        }
        if (CurrentAction is PatientCheckoutController)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneCheckOut);
        }
        if (CurrentAction is BedStation)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneCleanBed);
        }
        if (CurrentAction is CleaningStation)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneCleanBucket);
        }
        if (CurrentAction is WashingStation)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneCleanDoctor);
        }
        
    }
}
