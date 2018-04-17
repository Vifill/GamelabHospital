﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class OrderlyController : MonoBehaviour 
{
    [HideInInspector] public OrderlyAction CurrentAction;
    [HideInInspector] public OrderlyOrder CurrentOrder;
    public GameObject MovementParticle;
    public GameObject QueueUIPrefab;

    private GameObject QueueUI;
    private List<GameObject> QueueIcons = new List<GameObject>();
    private ParticleSystem.EmissionModule EmissionModule;
    private MouseInputController MouseInputController;
    private Transform QueueWorldPos;
    //public NavMeshAgent NavAgent;

    private ActionableActioner Actioner;

    public void StartOrder(OrderlyOrder pOrder)
    {
        CurrentOrder = pOrder;
        StartAction(CurrentOrder.GetNextAction());
    }

    public void StartAction(OrderlyAction pAction)
    {
        CurrentAction = pAction;
        CurrentAction?.StartAction(gameObject);
    }

    internal void CancelOrder()
    {
        CurrentAction = null;
        CurrentOrder = null;
        MouseInputController.ClearQueue();
    }

    internal void ActionFinished()
    {
        var nextAction = CurrentOrder?.GetNextAction();
        if(nextAction == null)
        {
            CurrentAction = null;
            CurrentOrder = null;
        }
        else
        {
            StartAction(nextAction);
        }

        InitializeQueueUI();
    }

    // Use this for initialization
    private void Start () 
	{
        QueueWorldPos = GetComponent<ActionableActioner>().ProgressBarWorldPosition;
        QueueUI = Instantiate(QueueUIPrefab, QueueWorldPos.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
        MouseInputController = FindObjectOfType<MouseInputController>();
        Actioner = GetComponent<ActionableActioner>();

        if (MovementParticle != null)
        {
            GameObject tempParticle = ((GameObject)Instantiate(MovementParticle, new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z), transform.rotation, transform));
            EmissionModule = tempParticle.GetComponentInChildren<ParticleSystem>().emission;
            EmissionModule.enabled = false;
        }
    }

    public void StartActionable(Actionable pActionable)
    {
        Actioner.AttemptAction(pActionable);
    }

    public void EnableMovementParticle()
    {
        EmissionModule.enabled = true;
    }

    public void DisableMovementParticle()
    {
        EmissionModule.enabled = false;
    }

    //private IEnumerator ActionableCoroutine()
    //{
    //    StartCoroutine(ProgressAction);
    //}

    // Update is called once per frame
    private void Update () 
	{
		if(CurrentAction != null)
        {
            CurrentAction.UpdateAction();
        }

        QueueUI.transform.position = Camera.main.WorldToScreenPoint(QueueWorldPos.position);
	}

    //private void UpdateQueueUI()
    //{
    //    var interactionActions = MouseInputController.GetAllInteractionActions();

    //    if (interactionActions.Any())
    //    {
    //        if (!QueueIcons.Any())
    //        {
    //            foreach (var action in interactionActions)
    //            {
    //                var icon = Instantiate(action.GetActionIcon());
    //                QueueIcons.Add(icon);
    //            }
    //        }
    //        else
    //        {

    //        }
            
    //    }
    //    else
    //    {
    //        if (QueueIcons.Any())
    //        {
    //            foreach (var icon in QueueIcons)
    //            {
    //                Destroy(icon);
    //                QueueIcons.Remove(icon);
    //            }
    //        }
    //    }
    //}

    public void InitializeQueueUI()
    {
        if (QueueIcons.Any())
        {
            foreach(var icon in QueueIcons)
            {
                Destroy(icon);
            }
            QueueIcons.Clear();
        }

        var interactionActions = MouseInputController.GetAllInteractionActions();

        if (interactionActions.Any())
        {
            int counter = 0;
            float uiWidth = QueueUI.GetComponent<RectTransform>().rect.width;
            print(uiWidth);
            foreach (var action in interactionActions)
            {
                //instantiate in right pos
                float Xpos = (uiWidth / (interactionActions.Count() + 1) * counter);
                print("Xpos of Icon = " + Xpos);
                Vector3 iconSpawnPos = new Vector3((QueueUI.transform.position.x - (uiWidth/2)) + Xpos, QueueUI.transform.position.y, QueueUI.transform.position.z);
                var icon = Instantiate(action.GetActionIcon(), Vector3.zero, QueueUI.transform.rotation, QueueUI.transform);
                icon.transform.position = iconSpawnPos;
                QueueIcons.Add(icon);
                counter++;
            }
        }
        
    }
}
