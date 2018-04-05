using UnityEngine;

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
