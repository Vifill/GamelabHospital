using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableStation : Actionable
{
    public GameObject StartingTableObjectPrefab;

    [Header("Table Sounds")]
    public AudioClip PickUpSound;
    public AudioClip DropSound;

    public GameObject TableObject { get; private set; }
    private GameObject ObjectToSwitch;

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

    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        //Can only be actioned if player is holding a tool and table empty or player empty handed and tool on table or Player is holding tool and table has tool (switch items)
        return (pCurrentTool == ToolName.NoTool && TableObject != null) || (pCurrentTool != ToolName.NoTool && TableObject == null || (pCurrentTool != ToolName.NoTool && TableObject != null));
    }

    public override void OnFinishedAction(GameObject pObjectActioning = null)
    {
        base.OnFinishedAction(pObjectActioning);

        var toolController = pObjectActioning.GetComponent<ToolController>();
        ToolName pCurrentTool = toolController.GetCurrentToolName();

        if (pCurrentTool == ToolName.NoTool && TableObject != null) // If player empty handed and table has tool
        {
            ToolBase pToolOnTable = TableObject.GetComponent<ToolBase>();
            pToolOnTable.gameObject.GetComponent<Pickupable>().IsActionActive = true;
            pToolOnTable.gameObject.GetComponent<Pickupable>().RemoveHighlight();
            toolController.SetTool(TableObject);
            ChangeObjectLayer(TableObject.transform, "Default");
            TableObject = null;
            ActionFinishedSoundEvent = PickUpSound;
            PlayFinishedActionSFX();            
        }
        else if (pCurrentTool != ToolName.NoTool && TableObject != null) //swapping tools on the table
        {
            ToolBase pToolOnTable = TableObject.GetComponent<ToolBase>();
            ToolBase pTool = toolController.GetToolBase();

            pToolOnTable.gameObject.GetComponent<Pickupable>().IsActionActive = true;
            pToolOnTable.gameObject.GetComponent<Pickupable>().RemoveHighlight();
            toolController.RemoveTool();
            toolController.SetTool(TableObject);
            ChangeObjectLayer(TableObject.transform, "Default");

            TableObject = pTool.gameObject;
            pTool.gameObject.GetComponent<Pickupable>().IsActionActive = false;
            PlaceTool();

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
        else if ((pCurrentTool != ToolName.NoTool && TableObject != null)) // If player holding tool & table has tool
        {
            // Find tool on table, assign as object to switch
            ToolBase pToolOnTable = TableObject.GetComponent<ToolBase>();
            pToolOnTable.gameObject.GetComponent<Pickupable>().IsActionActive = true;
            pToolOnTable.gameObject.GetComponent<Pickupable>().RemoveHighlight();
            ObjectToSwitch = pToolOnTable.gameObject;

            // Get current tool and place it on table
            ToolBase pTool = toolController.GetToolBase();
            TableObject = pTool.gameObject;
            pTool.gameObject.GetComponent<Pickupable>().IsActionActive = false;
            toolController.RemoveTool();
            PlaceTool();

            // Pick up object
            toolController.SetTool(ObjectToSwitch);

            ActionFinishedSoundEvent = PickUpSound;
            PlayFinishedActionSFX();
        }
    }

    private void PlaceTool()
    {
        TableObject.transform.SetParent(transform.Find(Constants.Highlightable));
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
    
}
