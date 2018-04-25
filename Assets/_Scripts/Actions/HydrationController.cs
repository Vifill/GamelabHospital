using Assets._Scripts.Utilities;
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
    private LevelManager LevelManager;
    private Animator PatientAnimator;

    protected override void Initialize()
    {
        StartCoroutine(GetPatientAnimator());
        HealthCtrl = GetComponent<HealthController>();
        DoctorSanitationThresholdConfig = HealthCtrl.DoctorSanitationThresholdConfig;
        MovementController = GetComponent<PatientMovementController>();
        LevelManager = FindObjectOfType<LevelManager>();
    }

    private IEnumerator GetPatientAnimator()
    {
        yield return new WaitForEndOfFrame();

        PatientAnimator = transform.Find(Constants.Highlightable).GetComponentInChildren<Animator>();
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
            // Stop animation and restart if giving new water
            PatientAnimator.ResetTrigger(CurrentHydrationModel.PatientAnimationParameter);
        }
        CurrentHydrations.Add(CurrentHydrationModel, StartCoroutine(HydrationCoroutine(CurrentHydrationModel)));
        ResolveSanitationEffect(pObjectActioning.GetComponent<SanitationController>().Sanitation);

        // start animation
        if (!string.IsNullOrEmpty(CurrentHydrationModel.PatientAnimationParameter))
        {
            PatientAnimator.SetTrigger(CurrentHydrationModel.PatientAnimationParameter);
        }

        if (LevelManager == null)
        {
            print("POP2");
        }
        LevelManager.AddPoints(20, transform.position);
    }

    private void ResolveSanitationEffect(float pDirtyStatus)
    {
        var severity = DoctorSanitationThresholdConfig.ListOfThresholds.LastOrDefault(a => a.ThresholdOfActivation <= pDirtyStatus);
        HealthCtrl.Health = Mathf.Clamp(HealthCtrl.Health -= severity?.HealthDecreasePerSecond ?? 0, HealthCtrl.HealthClampMin, HealthCtrl.HealthClampMax);
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
            HealthCtrl.HydrationMeter = Mathf.Clamp(HealthCtrl.HydrationMeter + (CurrentHydrationModel.HydrationReplenished / CurrentHydrationModel.TimeItTakes) * Time.deltaTime, HealthCtrl.MinHydration, HealthCtrl.MaxHydration);

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
