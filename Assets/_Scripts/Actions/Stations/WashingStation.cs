using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingStation : Actionable
{
    public GameObject DocWashedParticlePrefab;
    private float OriginalStartingTime;
    private GameObject ObjectActioning;

    protected override void Initialize()
    {
        OriginalStartingTime = ActionTime;
    }

    public override ActionableParameters GetActionableParameters(GameObject pObjectActioning = null)
    {
        ActionTime += pObjectActioning?.GetComponent<SanitationController>().Sanitation / 100 ?? 0;
        return new ActionableParameters() { ActionParticles = ActionParticles, ActionSoundClip = ActionSoundEvent, ActionFinishedSoundClip = ActionFinishedSoundEvent, IsPickupable = IsPickupable, RadiusOfActivation = RadiusOfActivation, TimeToTakeAction = ActionTime, AnimationParameter = AnimatorParameter, ActionSuccessParticles = ActionSuccessParticles };
    }

    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        var toolbase = pObjectActioning.GetComponent<ToolController>().GetToolBase();

        return toolbase == null && IsActionActive;
    }

    public override void OnStartAction(GameObject pObjectActioning)
    {
        //ActionTime += pObjectActioning.GetComponent<SanitationController>().CurrentSanitationLevel / 100;
        ObjectActioning = pObjectActioning;
    }

    public override void OnStopAction()
    {
        ActionTime = OriginalStartingTime;
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        pObjectActioning.GetComponent<SanitationController>().ClearSanitation();
        Instantiate(DocWashedParticlePrefab, pObjectActioning.transform.position, Quaternion.identity, pObjectActioning.transform);
        ActionTime = OriginalStartingTime;
    }
}
