using System;
using System.Collections;
using System.Collections.Generic;
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

    internal void AddAction(OrderlyAction pOrderlyAction)
    {
        ActionQueue.Enqueue(pOrderlyAction);
    }
}
