using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetActionablesUtility: ScriptableObject
{
    //public static Actionable GetActionable(ToolName pCurrentTool, Transform pPlayerTransform)
    //{
    //    var closeActionables = FindObjectsOfType<Actionable>().Where(a => a.CanBeActioned(pCurrentTool) && a.IsClose(pPlayerTransform) && !a.GetActionableParameters().IsPickupable);
    //    return closeActionables.OrderBy(a => Vector3.Distance(pPlayerTransform.position, a.transform.position)).FirstOrDefault();
    //}

    //public static Actionable GetPickupable(ToolName pCurrentTool, Transform pPlayerTransform)
    //{
    //    var closeActionables = FindObjectsOfType<Actionable>().Where(a => a.CanBeActioned(pCurrentTool) && a.IsClose(pPlayerTransform) && a.GetActionableParameters().IsPickupable);
    //    return closeActionables.OrderBy(a => Vector3.Distance(pPlayerTransform.position, a.transform.position)).FirstOrDefault();
    //}

    //public static Actionable GetActionableAndPickupable(ToolName pCurrentTool, Transform pPlayerTransform)
    //{
    //    var closeActionables = FindObjectsOfType<Actionable>().Where(a => a.IsClose(pPlayerTransform) && a.CanBeActioned(pCurrentTool));
    //    return closeActionables.OrderBy(a => Vector3.Distance(pPlayerTransform.position, a.transform.position)).FirstOrDefault();
    //}

    //public static Actionable GetActionablesForDrop(Transform pPlayerTransform)
    //{
    //    var closeActionables = FindObjectsOfType<Actionable>().Where(a => a.IsClose(pPlayerTransform) && !a.GetActionableParameters().IsPickupable);
    //    return closeActionables.OrderBy(a => Vector3.Distance(pPlayerTransform.position, a.transform.position)).FirstOrDefault();
    //}

    public static Actionable GetActionableForHighlight(ToolBase pCurrentTool, Transform pPlayerTransform)
    {
        var closeActionables = FindObjectsOfType<Actionable>().Where(a => a.gameObject != pCurrentTool?.gameObject && a.IsClose(pPlayerTransform) && a.IsActionActive);
        return closeActionables.OrderBy(a => Vector3.Distance(pPlayerTransform.position, a.transform.position)).FirstOrDefault();
    }
}
