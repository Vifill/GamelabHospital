using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : Actionable 
{
    public Vector3 PickupRotation = new Vector3(0, 0, 0);
    public Vector3 StationaryRotation = new Vector3(0, 0, 0);
    public Vector3 StationaryOffsetPosition = new Vector3(0, 0, 0);

    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        return pCurrentTool == ToolName.NoTool && IsPickupable == true && IsActionActive;
    }

    //public override ActionableParameters GetActionableParameters()
    //{
    //    return new ActionableParameters() { RadiusOfActivation = 2, TimeToTakeAction = 0, ActionSoundClip = null, IsPickupable = true};
    //}

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        var toolController = pObjectActioning.GetComponent<ToolController>();
        toolController.SetTool(gameObject);

        //Destroy(gameObject);
    }
}