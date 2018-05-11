using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderlyOrder
{
    private Queue<OrderlyAction> ActionQueue = new Queue<OrderlyAction>();

    public Vector3 FirstActionPosition;

    public OrderlyOrder(Vector3 pFirstPosition)
    {
        FirstActionPosition = pFirstPosition;
    }

    public OrderlyAction GetNextAction()
    {
        if(ActionQueue.Count > 0)
        {
            return ActionQueue.Dequeue();
        }
        return null;
    }

    public OrderlyInteractionAction GetInteractionAction()
    {
        return ActionQueue.FirstOrDefault(a => a is OrderlyInteractionAction) as OrderlyInteractionAction;
    }

    public OrderlyMoveAction GetMoveAction()
    {
        return ActionQueue.FirstOrDefault(a => a is OrderlyMoveAction) as OrderlyMoveAction;
    }

    public bool IsMoveAction()
    {
        return !ActionQueue.Any(a => a is OrderlyInteractionAction);
    }

    internal void AddAction(OrderlyAction pOrderlyAction)
    {
        ActionQueue.Enqueue(pOrderlyAction);
    }
}
