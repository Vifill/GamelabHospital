using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControllerTutorial : HealthController
{
    protected override void Excrete()
    {
        base.Excrete();
        EventManager.TriggerEvent(EventManager.EventCodes.DonePuking);
    }
}
