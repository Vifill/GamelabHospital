using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OrderlyAction 
{
    /// <summary>
    /// UpdateAction is called every update frame;
    /// </summary>
    public virtual void UpdateAction() { }
    protected virtual void OnStartAction() { }
    protected virtual void OnFinishedAction() { }
    protected virtual void OnStopAction() { }

    protected GameObject OrderlyObject;

    protected void ActionFinished()
    {
        OnStopAction();
        OrderlyObject.GetComponent<OrderlyController>().ActionFinished();
    }

    internal void CancelOrder()
    {
        OnStopAction();
        OrderlyObject.GetComponent<OrderlyController>().CancelOrder();
    }

    public void StartAction(GameObject pOrderlyObject)
    {
        OrderlyObject = pOrderlyObject;
        OnStartAction();
    }
}
