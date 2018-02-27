using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrderlyController : MonoBehaviour 
{
    [HideInInspector] public OrderlyAction CurrentAction;
    [HideInInspector] public OrderlyOrder CurrentOrder;
    public GameObject MovementParticle;

    private ParticleSystem.EmissionModule EmissionModule;
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
        FindObjectOfType<MouseInputController>().ClearQueue();
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
    }

    // Use this for initialization
    private void Start () 
	{
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
	}
}
