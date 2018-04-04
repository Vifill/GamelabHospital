using UnityEngine;
using System.Collections;

public class MovementControllerTutorial : MovementController
{

    protected override void WalkInput()
    {
        base.WalkInput();

        if(CanMove && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
        {
            EventManager.TriggerEvent(EventManager.EventCodes.DoneWalking);
        }
    }

}
