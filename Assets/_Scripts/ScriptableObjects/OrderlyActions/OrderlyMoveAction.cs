﻿using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrderlyMoveAction : OrderlyAction
{
    private Vector3 PositionToMoveTo;
    private Transform Target;
    private float DistanceToStop;
    private float DistanceWhenCloseEnough;

    private PatientStatusController PatientStatusController;
    private bool IsGoingToPatient;

    private NavMeshAgent NavAgent;

    private Animator Animator;

    public OrderlyMoveAction(Transform pTargetTransform = null, Vector3 pPosition = new Vector3(), float pDistanceToStop = 0, float pDistanceWhenCloseEnough = 2)
    {
        if (pTargetTransform != null)
        {
            Target = pTargetTransform;
            DistanceToStop = pDistanceToStop;
            PatientStatusController = pTargetTransform.GetComponent<PatientStatusController>();
            IsGoingToPatient = PatientStatusController != null;
        }
        else
        {
            PositionToMoveTo = pPosition;
        }
        
        DistanceWhenCloseEnough = pDistanceWhenCloseEnough;

    }

    public override void UpdateAction()
    {
        if(IsGoingToPatient && PatientStatusController.IsDead)
        {
            CancelOrder();
        }

        if (Vector3.Distance(PositionToMoveTo, OrderlyObject.transform.position) <= DistanceWhenCloseEnough)
        {
            NavAgent.isStopped = true;

            ActionFinished();
        }
        //if (NavAgent.remainingDistance <= DistanceWhenCloseEnough)
        //{
        //    NavAgent.isStopped = true;

        //    ActionFinished();
        //}
    }

    protected override void OnStartAction()
    {
        Animator = OrderlyObject.GetComponentInChildren<Animator>();

        if (!Animator.GetBool(Constants.AnimationParameters.CharacterIsWalking))
        {
            Animator.SetBool(Constants.AnimationParameters.CharacterIsWalking, true);
            //EmissionModule.enabled = true;
        }

        OrderlyObject.GetComponent<OrderlyController>().EnableMovementParticle();

        if (Target != null)
        {
            PositionToMoveTo = Target.GetComponent<Actionable>()?.GetTargetPoint(OrderlyObject.transform) ?? Target.position;
        }

        NavAgent = OrderlyObject.GetComponent<NavMeshAgent>();
        NavAgent.isStopped = false;

        NavAgent.SetDestination(PositionToMoveTo);
        NavAgent.stoppingDistance = DistanceToStop;

        Vector3 particlePos = PositionToMoveTo;
        SetDestinationParticle(particlePos, false);
    }

    protected override void OnStopAction()
    {
        NavAgent = OrderlyObject.GetComponent<NavMeshAgent>();
        NavAgent.isStopped = true;

        if (Animator.GetBool(Constants.AnimationParameters.CharacterIsWalking))
        {
            Animator.SetBool(Constants.AnimationParameters.CharacterIsWalking, false);
        }

        OrderlyObject.GetComponent<OrderlyController>().DisableMovementParticle();

        Vector3 particlePos = OrderlyObject.transform.position;
        SetDestinationParticle(particlePos, true);
    }

    private void SetDestinationParticle(Vector3 pPosition, bool pSetParent)
    {
        var particle = OrderlyObject.GetComponent<OrderlyController>().SelectionParticleEffect;

        if (particle == null)
        {
            return;
        }

        if (!pSetParent)
        {
            particle.transform.parent = null;
        }
        else
        {
            particle.transform.SetParent(OrderlyObject.transform);
        }

        var pos = pPosition - new Vector3(0, pPosition.y, 0) + new Vector3(0, 0.1f, 0);
        particle.transform.position = pos;
    }
}
