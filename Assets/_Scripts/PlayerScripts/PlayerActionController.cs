using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionController : MonoBehaviour 
{
    private MovementController MovementController;
    //private bool IsActioning;
    //private Image ProgressBar;
    //private Canvas Canvas;

    //private GameObject ActionableParticles;

    //private float TotalTime;
    //private float CurrentTime { get { return CurrentAction.ActionProgress; } set { CurrentAction.ActionProgress = value; } }
    //private Action ActionAfterFinishing;
    //private Actionable CurrentAction;

    //public GameObject ProgressBarPrefab;
    //public float UIOffset;
    public AudioSource Asource;
    public AudioClip PickUpSound;
    public AudioClip DropSound;
    public AudioClip InvalidActionSound;

    private ActionableActioner Actioner;

    private void Start()
    {
        Actioner = GetComponent<ActionableActioner>();
        MovementController = FindObjectOfType<MovementController>();
    }

    internal void AttemptAction(Actionable pAction)
    {
        Actioner.AttemptAction(pAction, MovementController);
        return;

        //CurrentAction = pAction;
        //var parameters = pAction.GetActionableParameters();
        
        //if(parameters.ActionParticles != null)
        //{
        //    ActionableParticles = Instantiate(parameters.ActionParticles, pAction.transform); //Start particles
        //    ActionableParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        //}

        //PlayActionSound();

        //pAction.OnStartAction();
        //ActionAfterFinishing = pAction.OnFinishedAction;
        //var progressBar = Instantiate(ProgressBarPrefab);
        //progressBar.transform.SetParent(Canvas.transform);
        //ProgressBar = progressBar.transform.GetChild(0).GetComponent<Image>();
        //TotalTime = parameters.TimeToTakeAction;
        //IsActioning = true;
        //MovementController.StopMovement();
    }

    private void Update()
    {
        //if (IsActioning)
        //{
        //    CurrentTime += Time.deltaTime;
        //    ProgressBar.fillAmount = CurrentTime / TotalTime;
        //    ProgressBar.transform.parent.position = Camera.main.WorldToScreenPoint(MovementController.transform.position) + new Vector3(0, UIOffset);

        //    if (CurrentTime >= TotalTime)
        //    {
        //        CurrentTime = 0;
        //        ActionAfterFinishing?.Invoke();
        //        StopAction();
        //        CurrentAction.PlayFinishedActionSFX();
        //    }
        //}
    }

    public void PlayActionSound()
    {
        //if (CurrentAction.IsPickupable)
        //{
        //    Asource.PlayOneShot(PickUpSound);
        //}
        //else
        //{
        //    CurrentAction.PlayActionSFX();
        //}
    }
    
    public void StopAction(Actionable pAction)
    {
        //if(pAction == CurrentAction)
        //{
        //    StopAction();
        //}
    }

    public void StopAction()
    {
        Actioner.StopAction();
        //if(ActionableParticles != null)
        //{
        //    Destroy(ActionableParticles);
        //}

        //CurrentAction?.StopActionSFX();

        //IsActioning = false;
        //DestroyProgressBar();
        //MovementController.StartMovement();
    }

    private void DestroyProgressBar()
    {
        //if(ProgressBar != null)
        //{
        //    var parent = ProgressBar.transform.parent.gameObject;
        //    Destroy(ProgressBar.gameObject);
        //    Destroy(parent);
        //}        
    }
    
}
