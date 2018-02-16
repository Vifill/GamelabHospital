using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrationController : Actionable
{
    private HealthController HealthCtrl;
    private HydrationModel CurrentHydrationModel;
    private bool IsHydrating;
    private float Counter;

    protected override void Initialize()
    {
        HealthCtrl = GetComponent<HealthController>();
    }

    private void Update()
    {
        if (IsHydrating)
        {
            Hydrate();
        }
    }

    public override ActionableParameters GetActionableParameters(GameObject pObjectActioning = null)
    {
        float actionTime = 0;
        if(pObjectActioning != null)
        {
            CurrentHydrationModel = (pObjectActioning.GetComponent<ToolController>().GetToolBase() as HydrationTool).HydrationModel;
            actionTime = CurrentHydrationModel.ActionTime;
        }

        return new ActionableParameters() { ActionParticles = ActionParticles, ActionSoundClip = ActionSoundEvent, ActionFinishedSoundClip = ActionFinishedSoundEvent, IsPickupable = IsPickupable, RadiusOfActivation = RadiusOfActivation, TimeToTakeAction = actionTime, AnimationParameter = AnimatorParameter, ActionSuccessParticles = ActionSuccessParticles };

    }

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        return pObjectActioning?.GetComponent<ToolController>().GetToolBase() is HydrationTool;
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {

        IsHydrating = true;
        pObjectActioning.GetComponent<ToolController>().RemoveTool();
    }

    private void Hydrate()
    {
        Counter += Time.deltaTime;

        if (Counter <= CurrentHydrationModel.TimeItTakes)
        {
            HealthCtrl.HydrationMeter += (CurrentHydrationModel.HydrationReplenished / CurrentHydrationModel.TimeItTakes) * Time.deltaTime;
        }
        else
        {
            IsHydrating = false;
            Counter = 0;
        }
    }
}
