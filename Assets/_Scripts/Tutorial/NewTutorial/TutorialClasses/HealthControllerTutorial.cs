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

    public override void Initialize()
    {
        base.Initialize();

        StartCoroutine(TriggerInitializedEvent());
    }

    private IEnumerator TriggerInitializedEvent()
    {
        yield return new WaitForEndOfFrame();

        EventManager.TriggerEvent(EventManager.EventCodes.PatientInitialized);
    }
}
