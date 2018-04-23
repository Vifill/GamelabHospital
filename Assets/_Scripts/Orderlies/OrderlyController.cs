using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class OrderlyController : MonoBehaviour 
{
    [HideInInspector] public OrderlyAction CurrentAction;
    [HideInInspector] public OrderlyOrder CurrentOrder;
    [HideInInspector] public Queue<OrderlyOrder> OrderQueue = new Queue<OrderlyOrder>();

    public GameObject MovementParticle;
    public GameObject QueueUIPrefab;
    public float YUIOffset;
    public float MaxVisibleIcons = 4;

    private bool HasParticleSystem;
    private GameObject QueueUI;
    private List<GameObject> QueueIcons = new List<GameObject>();
    private ParticleSystem.EmissionModule EmissionModule;
    private MouseInputController MouseInputController;
    private Transform QueueWorldPos;
    private float UIWidth;
    private Vector3 UIPos = new Vector3();

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
        //CurrentAction.CancelOrder();
        CurrentAction = null;
        CurrentOrder = null;
        ClearQueue();
    }

    private void ClearQueue()
    {
        OrderQueue.Clear();
        InitializeQueueUI();
    }

    internal void ActionFinished()
    {
        var nextAction = CurrentOrder?.GetNextAction();
        if(nextAction == null)
        {
            CurrentAction = null;
            CurrentOrder = null;
            CheckOrderQueue();
        }
        else
        {
            StartAction(nextAction);
        }

        InitializeQueueUI();
    }

    public void AddQueue(OrderlyOrder pOrder)
    {
        OrderQueue.Enqueue(pOrder);
        CheckOrderQueue();
        InitializeQueueUI();
    }

    private void CheckOrderQueue()
    {
        if (OrderQueue.Count > 0 && CurrentAction == null)
        {
            StartOrder(OrderQueue.Dequeue());
        }
    }

    // Use this for initialization
    private void Start () 
	{
        QueueWorldPos = GetComponent<ActionableActioner>().ProgressBarWorldPosition;
        if (QueueUIPrefab != null)
        {
            QueueUI = Instantiate(QueueUIPrefab, QueueWorldPos.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
            UIWidth = QueueUI.GetComponent<RectTransform>().rect.width;
        }
        MouseInputController = FindObjectOfType<MouseInputController>();
        Actioner = GetComponent<ActionableActioner>();

        if (MovementParticle != null)
        {
            GameObject tempParticle = ((GameObject)Instantiate(MovementParticle, new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z), transform.rotation, transform));
            EmissionModule = tempParticle.GetComponentInChildren<ParticleSystem>().emission;
            EmissionModule.enabled = false;
            HasParticleSystem = true;
        }
    }

    public void StartActionable(Actionable pActionable)
    {
        Actioner.AttemptAction(pActionable);
    }

    public void EnableMovementParticle()
    {
        if (HasParticleSystem)
        {
            EmissionModule.enabled = true;
        }
    }

    public void DisableMovementParticle()
    {
        if (HasParticleSystem)
        {
            EmissionModule.enabled = false;
        }
    }

    // Update is called once per frame
    private void Update () 
	{
        CheckOrderQueue();
        if (CurrentAction != null)
        {
            CurrentAction.UpdateAction();
        }
        UIPos = Camera.main.WorldToScreenPoint(QueueWorldPos.position) + new Vector3(0, YUIOffset, 0);

        if (QueueUI != null)
        {
            QueueUI.transform.position = UIPos;
        }
	}

    private List<OrderlyInteractionAction> GetAllInteractionActions()
    {
        return OrderQueue.Select(a => a.GetInteractionAction()).ToList();
    }

    private void InitializeQueueUI()
    {
        if (QueueIcons.Any())
        {
            foreach(var icon in QueueIcons)
            {
                Destroy(icon);
            }
            QueueIcons.Clear();
        }

        List<OrderlyInteractionAction> interactionActions = new List<OrderlyInteractionAction>();

        if (CurrentOrder?.GetInteractionAction() != null)
        {
            interactionActions.Add(CurrentOrder.GetInteractionAction());
        }
        
        interactionActions.AddRange(GetAllInteractionActions());

        if (interactionActions.Any())
        {
            int counter = 1;

            foreach (var action in interactionActions)
            {
                float Xpos = 0;

                Xpos = ((UIWidth / (interactionActions.Count + 1)) * counter);

                var icon = Instantiate(action.GetActionIcon(), new Vector3(0, 0, 0), QueueUI.transform.rotation, QueueUI.transform);
                icon.transform.localPosition = new Vector3(Xpos - (UIWidth / 2), 0, 0);
                icon.transform.SetSiblingIndex(interactionActions.Count - 1);
                QueueIcons.Add(icon);
                counter++;
            }
        }        
    }
}
