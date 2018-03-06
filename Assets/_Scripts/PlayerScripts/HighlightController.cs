using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighlightController : MonoBehaviour 
{
    public Shader StandardOutlineShader;
    public Shader WebGLOutlineShader;
    [System.NonSerialized]
    public Shader HighlightShader;
    public static GameObject HighlightedObject;

    private Actionable PreviousActionable;
    private ToolController ToolCtrl;

    private void Awake()
    {
    #if UNITY_WEBGL
            print("webGL");
            HighlightShader = WebGLOutlineShader;
    #else
            print("notWebgl");
            HighlightShader = StandardOutlineShader;
    #endif
    }

    private void Start() 
	{
        ToolCtrl = GetComponent<ToolController>();
    }
	
	// Update is called once per frame
	private void Update() 
	{
        var actionable = GetActionablesUtility.GetActionableForHighlight(ToolCtrl, transform)?.GetMostRelevantAction(ToolCtrl.GetCurrentToolName(), gameObject);

        if (actionable == null)
        {
            HighlightedObject = null;
            PreviousActionable?.RemoveHighlight();
        }

        else if (actionable != PreviousActionable)
        {
            if (actionable.CanBeActioned(ToolCtrl.GetCurrentToolName(), gameObject))
            {
                actionable.SetHighlight(HighlightShader);
            }
            else
            {
                //cant be used... RED
                actionable.SetHighlight(HighlightShader, new Color(0.8f,0,0));
            }

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
