using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour 
{
    public Transform ToolPosition;
    public GameObject CurrentTool { get; private set; }
    public Transform DropPosition;
    public Animator Animator;

    public ToolName GetCurrentToolName()
    {
        return CurrentTool?.GetComponent<ToolBase>()?.ToolName ?? ToolName.NoTool; 
    }

    internal void DestroyTool()
    {
        DestroyImmediate(CurrentTool);
        RemoveTool();
        SetAnimation();
    }

    internal void RemoveTool()
    {
        CurrentTool = null;
        SetAnimation();
    }

    internal void SetTool(GameObject pToolObject)
    {
        //TODO: Check if already has tool, to be safe?
        if(GetCurrentToolName() != ToolName.NoTool)
        {
            Debug.Log("Already has a tool! you fool! :o");
            return;
        }

        //CurrentTool = Instantiate(pToolObject, ToolPosition, false);
        CurrentTool = pToolObject;
        CurrentTool.transform.SetParent(ToolPosition);
        CurrentTool.transform.localPosition = Vector3.zero;
        CurrentTool.transform.localRotation = Quaternion.Euler(CurrentTool.GetComponent<Pickupable>().PickupRotation);    
        CurrentTool.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(SetActionActiveFalse(CurrentTool.GetComponent<Pickupable>()));

        //Debug.Log("Pickingup and setting actionactive false");
        SetAnimation();
    }

    private IEnumerator SetActionActiveFalse(Pickupable pickupable)
    {
        yield return new WaitForEndOfFrame();
        pickupable.IsActionActive = false;
    }

    private void SetAnimation()
    {
        foreach(var parameter in Animator.parameters)
        {
            Animator.SetBool(parameter.name, false);
        }
        if(Animator != null)
        {
            switch(GetCurrentToolName())
            {
                case ToolName.Bandage:
                    Animator.SetBool("HoldingBandage", true);
                    break;
                case ToolName.AnaestheticSyringe:
                    Animator.SetBool("HoldingSyringe", true);
                    break;
                case ToolName.VaccinationSyringe:
                    Animator.SetBool("HoldingSyringe", true);
                    break;
                case ToolName.Saw:
                    Animator.SetBool("HoldingSaw", true);
                    break;
                case ToolName.Bucket:
                    Animator.SetBool("HoldingSyringe", true);
                    break;
                case ToolName.Water:
                    Animator.SetBool("HoldingBandage", true);
                    break;
                case ToolName.IVBag:
                    Animator.SetBool("HoldingSyringe", true);
                    break;
            }



            //if (GetCurrentToolName() == ToolName.Bandage)
            //{
            //    Animator.SetBool("HoldingBandage", true);
            //}
            //else if(GetCurrentToolName)
            //{
            //    Animator.SetBool("HoldingBandage", false);
            //}
        }         
    }

    internal ToolBase GetToolBase()
    {
        return CurrentTool?.GetComponent<ToolBase>();
    }

    internal void DropTool()
    {
        CurrentTool.transform.parent = null;
        CurrentTool.GetComponent<Rigidbody>().isKinematic = false;
        CurrentTool.transform.position = DropPosition.position;
        CurrentTool.GetComponent<Pickupable>().IsActionActive = true;
        RemoveTool();
    }
}
