using UnityEngine;
using System.Collections;

public class MovementControllerTutorial : MovementController
{

    protected override void WalkInput()
    {
        base.WalkInput();

        if(CanMove && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
        {
            StartCoroutine(DoneWalking());
        }
    }

    private IEnumerator DoneWalking()
    {
        yield return new WaitForSeconds(5);

        EventManager.TriggerEvent(EventManager.EventCodes.DoneWalking);
    }
}
