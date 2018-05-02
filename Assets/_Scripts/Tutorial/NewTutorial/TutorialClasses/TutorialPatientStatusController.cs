using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPatientStatusController : PatientStatusController 
{
    public override void Death()
    {
        base.Death();

        EventManager.TriggerEvent(EventManager.EventCodes.DonePatientDeath);
    }
}
