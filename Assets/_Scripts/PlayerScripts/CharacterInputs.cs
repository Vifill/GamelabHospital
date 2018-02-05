using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInputs : MonoBehaviour 
{
    private PlayerActionController ActionController;
    private ToolController ToolController;
    private LevelController LevelController;
    private GameController GC;
    private PlayerDataController DataCtrl = new PlayerDataController();

    // Use this for initialization
    private void Start ()
	{
        LevelController = new LevelController(FindObjectOfType<SceneLoader>());
        GC = FindObjectOfType<GameController>();
        ActionController = GetComponent<PlayerActionController>();
        ToolController = GetComponent<ToolController>();
	}
	
	// Update is called once per frame
	private void Update () 
	{
        if (Input.GetButtonDown("Action") && !GameController.InMenuScreen)
        {
            //var action = GetActionablesUtility.GetActionableAndPickupable(GetCurrentTool(), transform);


            //if (action != null)
            //{
            //    ActionController.AttemptAction(action);
            //    //ToolController.GetToolBase()?.ToolUsed();
            //}

            var action = HighlightController.HighlightedObject?.GetComponent<Actionable>();

            if (action != null && action.CanBeActioned(GetCurrentTool(), gameObject))
            {
                ActionController.AttemptAction(action);
            }
            
            if (action != null && !action.CanBeActioned(GetCurrentTool(), gameObject))
            {
                //error sound
                ActionController.Asource.PlayOneShot(ActionController.InvalidActionSound);
            }
        }

        if(Input.GetButtonUp("Action"))
        {
            ActionController.StopAction();
            
        }

        if (Input.GetButtonDown("Drop"))
        {
            //var ActionablesForDrop = GetActionablesUtility.GetActionablesForDrop(transform);

            //if (ActionablesForDrop == null && ToolController.GetCurrentToolName() != ToolName.NoTool)
            //{
            //    DropTool(ToolController.CurrentTool);
            //    ToolController.RemoveTool();
            //}

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

        if(Input.GetKeyDown(KeyCode.N))
        {
            LevelController.LoadNextLevel(FindObjectOfType<LevelManager>().LevelConfig.LevelNumber);
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (!GameController.InPauseMenu)
            {
                GC.PauseGame(GC.PauseMenuPrefab);
            }
            else
            {
                GC.ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            DataCtrl.UnlockAllLevels();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            DataCtrl.ClearPrefs();
        }

        //if (Input.GetButtonDown("PickUp"))
        //{
        //    var pickupable = GetPickupable();
        //    if (pickupable != null)
        //    {
        //        ActionController.AttemptAction(pickupable);
        //    }

        //    else if (pickupable == null && ToolController.GetCurrentToolName() != ToolName.NoTool)
        //    {
        //        DropTool(ToolController.CurrentTool);
        //        ToolController.RemoveTool();
        //    }
        //}
    }

    private ToolName GetCurrentTool()
    {
        return ToolController.GetCurrentToolName();
    }

    private void DropTool(GameObject pTool)
    {
        ActionController.Asource.PlayOneShot(ActionController.DropSound);
        ToolController.DropTool();
        //pTool.transform.parent = null;
        //pTool.GetComponent<Rigidbody>().isKinematic = false;
        //pTool.transform.position = transform.GetChild(0).position;
        //ActionController.Asource.PlayOneShot(ActionController.DropSound);
        //ToolController.RemoveTool();
    }
}
