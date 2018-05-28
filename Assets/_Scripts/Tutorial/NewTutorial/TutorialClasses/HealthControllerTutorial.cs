using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControllerTutorial : HealthController
{
    private TutorialController TutorialController;

    protected override void Excrete()
    {
        base.Excrete();
        EventManager.TriggerEvent(EventManager.EventCodes.DonePuking);
    }

    public override void Initialize()
    {
        base.Initialize();
        TutorialController = FindObjectOfType<TutorialController>();
        StartCoroutine(TriggerInitializedEvent());
    }

    private IEnumerator TriggerInitializedEvent()
    {
        yield return new WaitForEndOfFrame();
        if (TutorialController.CurrentObjective.OnFinishEvent == EventManager.EventCodes.PatientInitialized)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.PatientInitialized);
        }
        else if (TutorialController.CurrentObjective.OnFinishEvent == EventManager.EventCodes.PatientInitializedLvl2)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.PatientInitializedLvl2);
        }
    }
}
