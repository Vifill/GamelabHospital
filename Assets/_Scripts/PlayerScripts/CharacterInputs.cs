﻿using UnityEngine;

public class CharacterInputs : MonoBehaviour 
{
    private PlayerActionController ActionController;
    private ToolController ToolController;
    private ActionableActioner ActionableActioner;


    // Use this for initialization
    private void Start ()
	{
        ActionController = GetComponent<PlayerActionController>();
        ToolController = GetComponent<ToolController>();
        ActionableActioner = GetComponent<ActionableActioner>();
	}
	
	// Update is called once per frame
	private void Update () 
	{
        if (Input.GetButtonDown("Action") && !GameController.InMenuScreen)
        {
            //Debug.Log("Action button down");
            var action = HighlightController.HighlightedObject?.GetComponent<Actionable>().GetMostRelevantAction(GetCurrentTool(), gameObject);

            if (action != null && action.CanBeActioned(GetCurrentTool(), gameObject))
            {
                ActionableActioner.AttemptAction(action, GetComponent<MovementController>());
                SuccessWarnings(action);              
            }
            
            else if (action != null && !action.CanBeActioned(GetCurrentTool(), gameObject))
            {

                FailedActionMessages(action);
                ActionController.Asource.PlayOneShot(ActionController.InvalidActionSound);
            }
        }
        
        if(Input.GetButtonUp("Action"))
        {
            //Debug.Log("Action button up");

            if (ActionableActioner.IsActioning && ActionableActioner.CurrentTime >= ActionableActioner.TotalTime - 0.1f)
            {
                return;
            }
            else
            {
                ActionableActioner.StopAction();
            }
        }

        if (Input.GetButtonDown("Drop"))
        {
            Actionable actionable;

            if (HighlightController.HighlightedObject == null)
            {
                actionable = null;
            }
            else
            {
                actionable = HighlightController.HighlightedObject.GetComponent<Actionable>();
            }

            if (actionable == null && ToolController.GetCurrentToolName() != ToolName.NoTool && ToolController.GetToolBase().CanBeDropped || actionable != null && actionable.IsPickupable && ToolController.GetCurrentToolName() != ToolName.NoTool && ToolController.GetToolBase().CanBeDropped)
            {
                DropTool(ToolController.CurrentTool);
            }
        }
    }

    private void SuccessWarnings(Actionable action)
    {
        if ((GetCurrentTool() == ToolName.Water || GetCurrentTool() == ToolName.IVBag) && ActionableActioner.IsDirty)
        {
            ActionableActioner.SpawnFloatingText("I should clean myself!");
        }
    }

    private void FailedActionMessages(Actionable action)
    {
        //error sound
        if (GetCurrentTool() == ToolName.NoTool)
        {
            if (action is BedStation)
            {
                ActionableActioner.SpawnFloatingText("I need a bucket to do this");
            }
            if (action is HydrationController)
            {
                ActionableActioner.SpawnFloatingText("I need water for the patient");
            }
            if (action is TableStation)
            {
                ActionableActioner.SpawnFloatingText("I can't do that right now");
            }
        }

        if (GetCurrentTool() == ToolName.Bucket && action is BedStation)
        {
            if (ToolController.GetToolBase().IsDirty)
            {
                ActionableActioner.SpawnFloatingText("I need a clean bucket for that");
            }
            else
            {
                ActionableActioner.SpawnFloatingText("The bed is not dirty");
            }
        }
        else if (GetCurrentTool() != ToolName.NoTool && action is PickupStationController)
        {
            ActionableActioner.SpawnFloatingText();
        }
    }

    private ToolName GetCurrentTool()
    {
        return ToolController.GetCurrentToolName();
    }

    private void DropTool(GameObject pTool)
    {
        ActionController.Asource.PlayOneShot(ActionController.DropSound);
        ToolController.DropTool();
    }
}
