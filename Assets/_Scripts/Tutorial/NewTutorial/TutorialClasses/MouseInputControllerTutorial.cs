using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputControllerTutorial : MouseInputController
{
    private TutorialController TutorialController;

    public override void Start()
    {
        base.Start();
        TutorialController = FindObjectOfType<TutorialController>();
    }

    protected override void CancelOrder()
    {
        if (TutorialController.CurrentObjective.OnFinishEvent == EventManager.EventCodes.DoneFinishingTutorialQueue)
        {
            // do nothing
        }
        else
        {
            base.CancelOrder();
        }
    }

    public override void Update()
    {
        if (Time.timeScale == 0 && !GameController.InMenuScreen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OrderlyOrder order = GetOrderFromMouse();

                if(TutorialController.CurrentObjective.OnFinishEvent == EventManager.EventCodes.DoneGetBucket && order.GetInteractionAction().Action is TableStation && (order.GetInteractionAction().Action as TableStation).TableObject.GetComponent<ToolBase>().ToolName == ToolName.Bucket)
                {
                    EventManager.TriggerEvent(EventManager.EventCodes.DoneGetBucket);
                    CurrentOrderly.AddQueue(order);
                }
                else if (TutorialController.CurrentObjective.OnFinishEvent == EventManager.EventCodes.DoneCleanBed && order.GetInteractionAction().Action is BedStation)
                {
                    EventManager.TriggerEvent(EventManager.EventCodes.DoneCleanBed);
                    CurrentOrderly.AddQueue(order);
                }
                else if (TutorialController.CurrentObjective.OnFinishEvent == EventManager.EventCodes.DoneCleanBucket && order.GetInteractionAction().Action is WashingStation)
                {
                    EventManager.TriggerEvent(EventManager.EventCodes.DoneCleanBucket);
                    CurrentOrderly.AddQueue(order);
                }
                else if (TutorialController.CurrentObjective.OnFinishEvent == EventManager.EventCodes.DoneReturnBucket && order.GetInteractionAction().Action is TableStation && (order.GetInteractionAction().Action as TableStation).TableObject.GetComponent<ToolBase>().ToolName == ToolName.Bucket)
                {
                    EventManager.TriggerEvent(EventManager.EventCodes.DoneReturnBucket);
                    CurrentOrderly.AddQueue(order);
                }
            }
        }
        else
        {
            base.Update();
        }
    }
}
