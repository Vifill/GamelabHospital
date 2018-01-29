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

    protected GameObject OrderlyObject;

    protected void ActionFinished()
    {
        OrderlyObject.GetComponent<OrderlyController>().ActionFinished();
    }

    protected void CancelOrder()
    {
        OrderlyObject.GetComponent<OrderlyController>().CancelOrder();
    }

    public void StartAction(GameObject pOrderlyObject)
    {
        OrderlyObject = pOrderlyObject;
        OnStartAction();
    }


}
