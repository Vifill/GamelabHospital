using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighlightController : MonoBehaviour 
{
    public Shader HighlightShader;
    public static GameObject HighlightedObject;

    private Actionable PreviousActionable;
    private ToolController ToolCtrl;

	// Use this for initialization
	private void Start () 
	{
        ToolCtrl = GetComponent<ToolController>();
    }
	
	// Update is called once per frame
	private void Update () 
	{
        var actionable = GetActionablesUtility.GetActionableForHighlight(ToolCtrl, transform)?.GetMostRelevantAction(ToolCtrl.GetCurrentToolName(), gameObject);

        if (actionable == null)
        {
            HighlightedObject = null;
            PreviousActionable?.RemoveHighlight();
        }

        else if (actionable != PreviousActionable)
        {
            actionable.SetHighlight(HighlightShader);
            HighlightedObject = actionable.gameObject;
            PreviousActionable?.RemoveHighlight();
        }
        //if (actionable != null)
        //{
        //    actionable.SetHighlight(HighlightShader);
        //    HighlightedObject = actionable.gameObject;
        //}

        PreviousActionable = actionable;

    }
}
