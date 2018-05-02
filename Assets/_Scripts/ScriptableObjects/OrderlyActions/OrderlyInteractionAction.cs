using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderlyInteractionAction : OrderlyAction
{
    public Actionable Action { private set; get; }
    private Vector3 ActionPosition;
    private const float OrderlyLookAtSpeed = 3;

    public OrderlyInteractionAction(Actionable pActionable)
    {
        Action = pActionable;
    }

    public override void UpdateAction()
    {
        OrderlyObject.transform.LookAt(ActionPosition);
    }

    protected override void OnStartAction()
    {
        Action = Action.GetMostRelevantAction(OrderlyObject.GetComponent<ToolController>().GetCurrentToolName(), OrderlyObject);
        ActionPosition = Action.transform.position;
        ActionPosition.y = OrderlyObject.transform.position.y;
        if (Action.CanBeActioned(OrderlyObject.GetComponent<ToolController>().GetCurrentToolName(), OrderlyObject) /*&& Action.IsClose(OrderlyObject.transform)*/)
        {
            OrderlyObject.GetComponent<ActionableActioner>().AttemptAction(Action, null, ActionFinished, CancelOrder);
        }
        else
        {
            OrderlyObject.GetComponent<ActionableActioner>().PlayInvalidActionSound();
            CancelOrder();
        }
    }

    protected override void OnStopAction()
    {
        OrderlyObject.GetComponent<ActionableActioner>().StopAction();
    }

    public GameObject GetActionIcon()
    {
        return Action.ActionIcon;
    }
}
