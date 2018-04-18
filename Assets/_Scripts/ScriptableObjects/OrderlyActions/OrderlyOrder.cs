<<<<<<< HEAD

﻿using System;
using System.Collections;
using System.Collections.Generic;

=======
using System.Collections.Generic;
>>>>>>> 8774382426c28c09dae0bb5d59dc17f3e5ad6cc5
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
        return ActionQueue.First(a => a is OrderlyInteractionAction) as OrderlyInteractionAction;
    }

    internal void AddAction(OrderlyAction pOrderlyAction)
    {
        ActionQueue.Enqueue(pOrderlyAction);
    }
}
