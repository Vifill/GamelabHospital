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
            //Debug.Log("Action button down");
            var action = HighlightController.HighlightedObject?.GetComponent<Actionable>().GetMostRelevantAction(GetCurrentTool(), gameObject);

            if (action != null && action.CanBeActioned(GetCurrentTool(), gameObject))
            {
                ActionableActioner.AttemptAction(action, GetComponent<MovementController>());
            }
            
            else if (action != null && !action.CanBeActioned(GetCurrentTool(), gameObject))
            {
                //error sound
                if (GetCurrentTool() == ToolName.Bucket && action is BedStation)
                {
                    ActionableActioner.SpawnFloatingText("The bed is not dirty");
                }
                else if (GetCurrentTool() != ToolName.NoTool && action is PickupStationController)
                {
                    ActionableActioner.SpawnFloatingText();
                }

                ActionController.Asource.PlayOneShot(ActionController.InvalidActionSound);
            }
        }
        
        if(Input.GetButtonUp("Action"))
        {
            //Debug.Log("Action button up");

            if (ActionableActioner.CurrentTime >= ActionableActioner.TotalTime - 0.1f)
            {
                return;
            }
            else
            {
                ActionableActioner.StopAction();
            }
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
