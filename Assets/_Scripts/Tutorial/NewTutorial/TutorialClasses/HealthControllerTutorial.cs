using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControllerTutorial : HealthController
{
    private int Counter = 0;
     
    protected override void Excrete()
    {
        base.Excrete();
        EventManager.TriggerEvent(EventManager.EventCodes.DonePuking);
    }

    public override void Initialize()
    {
        base.Initialize();

        if (Counter <= 0)
        {
            Counter++;
            EventManager.TriggerEvent(EventManager.EventCodes.FirstPatientInitialized);
        }
        else if (Counter > 0)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.SecondPatientInitialized);
        }

    }
}
