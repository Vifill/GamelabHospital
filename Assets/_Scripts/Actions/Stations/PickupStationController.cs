using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStationController : Actionable
{
    //public float TimeToAction;
    public GameObject ToolObject;

    private Animator Animator;


    protected override void Initialize()
    {
        Animator = GetComponentInChildren<Animator>();
    }

    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        return pCurrentTool == ToolName.NoTool && IsActionActive; //Can only be actioned if player doesnt hold anything(tool).
    }    

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        var toolController = pObjectActioning.GetComponent<ToolController>();
        var toolObject = Instantiate(ToolObject);
        toolController.SetTool(toolObject);
    }

    public override void OnStopAction()
    {
        if (ActionProgress == 0)
        {
            Animator?.SetBool(Constants.AnimationParameters.CharacterIsActioning, false);
        }
        else
        {
            Animator?.SetFloat(Constants.AnimationParameters.Speed, 0);
        }        
    }

    public override void OnStartAction(GameObject pObjectActioning)
    {
        if (ActionProgress == 0)
        {
            Animator?.SetBool(Constants.AnimationParameters.CharacterIsActioning, true);
            Animator?.SetFloat(Constants.AnimationParameters.Speed, 1);
        }
        else
        {
            Animator?.SetFloat(Constants.AnimationParameters.Speed, 1);
        }
    }
}
