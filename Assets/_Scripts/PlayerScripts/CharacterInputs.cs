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
    private ActionableActioner ActionableActioner;
    private OptionMenuManager OptionMenuManager;


    // Use this for initialization
    private void Start ()
	{
        LevelController = new LevelController(FindObjectOfType<SceneLoader>());
        GC = FindObjectOfType<GameController>();
        ActionController = GetComponent<PlayerActionController>();
        ToolController = GetComponent<ToolController>();
        ActionableActioner = GetComponent<ActionableActioner>();
	}
	
	// Update is called once per frame
	private void Update () 
	{
        if (Input.GetButtonDown("Action") && !GameController.InMenuScreen)
        {
            var action = HighlightController.HighlightedObject?.GetComponent<Actionable>().GetMostRelevantAction(GetCurrentTool(), gameObject);

            if (action != null && action.CanBeActioned(GetCurrentTool(), gameObject))
            {
                ActionableActioner.AttemptAction(action, GetComponent<MovementController>());
            }
            
            if (action != null && !action.CanBeActioned(GetCurrentTool(), gameObject))
            {
                //error sound
                ActionController.Asource.PlayOneShot(ActionController.InvalidActionSound);
            }
        }

        if(Input.GetButtonUp("Action"))
        {
            ActionableActioner.StopAction();
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

        if(Input.GetKeyDown(KeyCode.N))
        {
            LevelController.LoadNextLevel(FindObjectOfType<LevelManager>().LevelConfig.LevelNumber);
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (GameController.InOptionMenu)
            {
                OptionMenuManager = FindObjectOfType<OptionMenuManager>();
                OptionMenuManager.ButtonBack();
            }
            else if (!GameController.InPauseMenu)
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
