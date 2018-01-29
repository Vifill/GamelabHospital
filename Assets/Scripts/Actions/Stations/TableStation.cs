using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableStation : Actionable
{
    public GameObject TableObject;
    public GameObject StartingTableObjectPrefab;

    [Header("Table Sounds")]
    public AudioClip PickUpSound;
    public AudioClip DropSound;

    protected override void Initialize()
    {
        if (StartingTableObjectPrefab != null)
        {
            TableObject = Instantiate(StartingTableObjectPrefab);
            PlaceTool();
            StartCoroutine(SetToolAsInactive());
        }
    }

    private IEnumerator SetToolAsInactive()
    {
        yield return new WaitForEndOfFrame();
        TableObject.GetComponent<Pickupable>().IsActionActive = false;
    }

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        //Can only be actioned if player is holding a tool and table empty or player empty handed and tool on table
        return (pCurrentTool == ToolName.NoTool && TableObject != null) || (pCurrentTool != ToolName.NoTool && TableObject == null);
    }

    public override void OnFinishedAction(GameObject pObjectActioning = null)
    {
        var toolController = pObjectActioning.GetComponent<ToolController>();
        ToolName pCurrentTool = toolController.GetCurrentToolName();

        if (pCurrentTool == ToolName.NoTool && TableObject != null) // if player empty handed and table has tool
        {
            ToolBase pToolOnTable = TableObject.GetComponent<ToolBase>();
            pToolOnTable.gameObject.GetComponent<Pickupable>().IsActionActive = true;
            toolController.SetTool(TableObject);
            ChangeObjectLayer(TableObject.transform, "Default");
            TableObject = null;
            ActionFinishedSoundEvent = PickUpSound;
            PlayFinishedActionSFX();            
        }
        else if (pCurrentTool != ToolName.NoTool && TableObject == null) // if player holding tool & table empty
        {
            ToolBase pTool = toolController.GetToolBase();
            TableObject = pTool.gameObject;
            pTool.gameObject.GetComponent<Pickupable>().IsActionActive = false;
            toolController.RemoveTool();
            PlaceTool();
            ActionFinishedSoundEvent = DropSound;
            PlayFinishedActionSFX();
        }
    }

    private void PlaceTool()
    {
        TableObject.transform.SetParent(transform.GetChild(0).transform);
        TableObject.transform.localRotation = Quaternion.Euler(TableObject.GetComponent<Pickupable>().StationaryRotation);
        TableObject.transform.localPosition = TableObject.GetComponent<Pickupable>().StationaryOffsetPosition;
        ChangeObjectLayer(TableObject.transform, "Ignore Raycast");
    }

    private void ChangeObjectLayer(Transform pObject, string pLayer)
    {
        pObject.gameObject.layer = LayerMask.NameToLayer(pLayer);
        foreach (Transform child in pObject)
        {
            child.gameObject.layer = LayerMask.NameToLayer(pLayer);
            foreach (Transform pchild in child)
            {
                pchild.gameObject.layer = LayerMask.NameToLayer(pLayer);
            }
        }
    }

    public override void OnStartAction()
    {
        //GetComponent<Animator>().StartPlayback();
    }
}
