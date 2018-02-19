using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionableActioner : MonoBehaviour 
{
    private Actionable CurrentAction;
    private GameObject ActionableParticles;
    private Image ProgressBar;
    private Canvas Canvas;

    public GameObject ProgressBarPrefab;
    public Transform ProcessBarWorldPosition;
    public AudioSource Asource;
    public AudioClip PickUpSound;
    public AudioClip DropSound;
    public AudioClip InvalidActionSound;
    public Animator Animator;

    private float TotalTime;
    private float CurrentTime { get { return CurrentAction.ActionProgress; } set { CurrentAction.ActionProgress = value; } }
    private Action<GameObject> ActionAfterFinishing;
    private Action ExternalActionWhenSuccessful;
    private Action ExternalActionWhenFailed;
    private bool IsActioning;
    private MovementController MovementController;

    // Use this for initialization
    private void Start () 
	{
        Asource = GetComponent<AudioSource>();
        Canvas = FindObjectOfType<Canvas>();
    }

    // Update is called once per frame
    private void Update () 
	{
        if (IsActioning)
        {
            if (!CurrentAction.IsActionActive)
            {
                IsActioning = false;
                CurrentTime = 0;
                StopAction();
                ExternalActionWhenFailed?.Invoke();
            }

            CurrentTime += Time.deltaTime;
            ProgressBar.fillAmount = CurrentTime / TotalTime;
            ProgressBar.transform.parent.position = Camera.main.WorldToScreenPoint(ProcessBarWorldPosition.position) /*+ new Vector3(0, UIOffset)*/;

            if (CurrentTime >= TotalTime)
            {
                OnSuccess();
            }
        }
    }

    private void OnSuccess()
    {
        PlayParticleEffects(CurrentAction.GetActionableParameters().ActionSuccessParticles, CurrentAction.transform);

        CurrentTime = 0;
        StopAction();
        ProcessToolAfterSuccess();
        ActionAfterFinishing?.Invoke(gameObject);
        ExternalActionWhenSuccessful?.Invoke();
        CurrentAction.PlayFinishedActionSFX();
    }

    private void ProcessToolAfterSuccess()
    {
        var toolController = GetComponent<ToolController>();
        if (toolController.GetToolBase()?.IsUsedUpAfterUse ?? false)
        {
            toolController.DestroyTool();
        }
        if (CurrentAction.DirtiesTool && toolController.GetToolBase().NeedsToBeSanitized)
        {
            toolController.GetToolBase().ToolUsed();
        }
    }

    internal void AttemptAction(Actionable pAction, MovementController pMovementController = null, Action pExternalActionWhenSuccessful = null, Action pExternalActionWhenFailed = null)
    {
        MovementController = pMovementController;
        ExternalActionWhenSuccessful = pExternalActionWhenSuccessful;
        ExternalActionWhenFailed = pExternalActionWhenFailed;
        ActionAfterFinishing = pAction.OnFinishedAction;
        CurrentAction = pAction;
        var parameters = pAction.GetActionableParameters(gameObject);

        if (parameters.ActionParticles != null)
        {
            ActionableParticles = Instantiate(parameters.ActionParticles, pAction.transform); //Start particles
            ActionableParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }

        PlayActionSound();
        PlayAnimation();

        pAction.OnStartAction(gameObject);
        var progressBar = Instantiate(ProgressBarPrefab);
        progressBar.transform.SetParent(Canvas.transform);
        ProgressBar = progressBar.transform.GetChild(0).GetComponent<Image>();
        TotalTime = parameters.TimeToTakeAction;
        IsActioning = true;
        MovementController?.StopMovement();
    }

    private void PlayAnimation()
    {
        var parameter = CurrentAction.GetActionableParameters()?.AnimationParameter;
        if (Animator != null && !string.IsNullOrEmpty(parameter))
        {
            Animator.SetBool(CurrentAction.GetActionableParameters()?.AnimationParameter, true);
        }
    }

    private void StopAnimation()
    {
        if (Animator != null && !string.IsNullOrEmpty(CurrentAction?.GetActionableParameters()?.AnimationParameter))
        {
            Animator.SetBool(CurrentAction.GetActionableParameters()?.AnimationParameter, false );
        }
    }

    public void StopAction()
    {
        if (ActionableParticles != null)
        {
            Destroy(ActionableParticles);
        }

        StopAnimation();
        CurrentAction?.StopActionSFX();
        CurrentAction?.OnStopAction();

        IsActioning = false;
        DestroyProgressBar();
        MovementController?.StartMovement();

        
    }

    private void DestroyProgressBar()
    {
        if (ProgressBar != null)
        {
            var parent = ProgressBar.transform.parent.gameObject;
            Destroy(ProgressBar.gameObject);
            Destroy(parent);
        }
    }

    public void PlayActionSound()
    {
        if (CurrentAction.IsPickupable)
        {
            Asource.PlayOneShot(PickUpSound);
        }
        else
        {
            CurrentAction.PlayActionSFX();
        }
    }

    public void PlayInvalidActionSound()
    {
        Asource.PlayOneShot(InvalidActionSound);
    }

    private GameObject PlayParticleEffects(GameObject pParticles, Transform pTransform)
    {
        if (pParticles != null)
        {
            var particle = Instantiate(pParticles, pTransform.position, Quaternion.LookRotation(Vector3.forward));
            //particle.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            return particle;
        }
        return null;
    }
}
