using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HydrationController : Actionable
{
    private SanitationThresholdConfig DoctorSanitationThresholdConfig;
    private HealthController HealthCtrl;
    private HydrationModel CurrentHydrationModel;
    private bool IsHydrating;
    private float Counter;
    private GameObject DisplayedObject;
    private PatientMovementController MovementController;
    private Dictionary<HydrationModel, Coroutine> CurrentHydrations = new Dictionary<HydrationModel, Coroutine>();

    protected override void Initialize()
    {
        HealthCtrl = GetComponent<HealthController>();
        DoctorSanitationThresholdConfig = HealthCtrl.DoctorSanitationThresholdConfig;
        MovementController = GetComponent<PatientMovementController>();
    }

    public override ActionableParameters GetActionableParameters(GameObject pObjectActioning = null)
    {
        float actionTime = 0;
        if(pObjectActioning != null)
        {
            CurrentHydrationModel = (pObjectActioning.GetComponent<ToolController>().GetToolBase() as HydrationTool).HydrationModel;
            actionTime = CurrentHydrationModel.ActionTime;
            ActionSoundEvent = CurrentHydrationModel.HydrationSound;
        }

        return new ActionableParameters() { ActionParticles = ActionParticles, ActionSoundClip = ActionSoundEvent, ActionFinishedSoundClip = ActionFinishedSoundEvent, IsPickupable = IsPickupable, RadiusOfActivation = RadiusOfActivation, TimeToTakeAction = actionTime, AnimationParameter = AnimatorParameter, ActionSuccessParticles = ActionSuccessParticles, MakesPlayerDirty = MakesPlayerDirty};

    }

    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        return pObjectActioning?.GetComponent<ToolController>().GetToolBase() is HydrationTool;
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        if (CurrentHydrations.ContainsKey(CurrentHydrationModel))
        {
            StopCoroutine(CurrentHydrations[CurrentHydrationModel]);
            CurrentHydrations.Remove(CurrentHydrationModel);
        }
        CurrentHydrations.Add(CurrentHydrationModel, StartCoroutine(HydrationCoroutine(CurrentHydrationModel)));
        ResolveSanitationEffect(pObjectActioning.GetComponent<SanitationController>().CurrentSanitationLevel);
    }

    private void ResolveSanitationEffect(float pDirtyStatus)
    {
        var severity = DoctorSanitationThresholdConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= pDirtyStatus);
        HealthCtrl.CholeraSeverity += severity?.CholeraSeverityIncreasePerSecond ?? 0;
    }

    private IEnumerator HydrationCoroutine(HydrationModel pHmodel)
    {
        if (pHmodel.DisplayPrefab != null)
        {
            if (DisplayedObject == null)
            {
                var posObj = MovementController.TargetBed.transform.Find("IVPos");
                DisplayedObject = Instantiate(pHmodel.DisplayPrefab, posObj.position, posObj.rotation, posObj);
            }
        }

        float counter = 0;

        while(counter <= pHmodel.TimeItTakes)
        {
            HealthCtrl.HydrationMeter = Mathf.Clamp(HealthCtrl.HydrationMeter + (CurrentHydrationModel.HydrationReplenished / CurrentHydrationModel.TimeItTakes) * Time.deltaTime, 0, 100);

            counter += Time.deltaTime;
            yield return null;
        }

        if (DisplayedObject != null)
        {
            Destroy(DisplayedObject);
        }

        CurrentHydrations.Remove(pHmodel);
    }

    public void StopAllHydrations()
    {
        foreach (var hydration in CurrentHydrations)
        {
            StopCoroutine(hydration.Value);
        }

        if (DisplayedObject != null)
        {
            Destroy(DisplayedObject);
        }
    }
}
