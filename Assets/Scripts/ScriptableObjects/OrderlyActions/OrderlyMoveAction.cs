﻿using System.Collections;
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

    public OrderlyMoveAction(Transform pPosition, float pDistanceToStop = 0, float pDistanceWhenCloseEnough = 2)
    {
        Target = pPosition;
        DistanceToStop = pDistanceToStop;
        DistanceWhenCloseEnough = pDistanceWhenCloseEnough;
        PatientStatusController = pPosition.GetComponent<PatientStatusController>();
        IsGoingToPatient = PatientStatusController != null;
    }

    public override void UpdateAction()
    {
        if(IsGoingToPatient && PatientStatusController.IsDead)
        {
            CancelOrder();
        }

        if(Vector3.Distance(PositionToMoveTo, OrderlyObject.transform.position) <= DistanceWhenCloseEnough)
        {
            NavAgent.isStopped = true;

            ActionFinished();
        }
    }

    protected override void OnStartAction()
    {
        PositionToMoveTo = GetTargetPoint(Target);

        NavAgent = OrderlyObject.GetComponent<NavMeshAgent>();
        NavAgent.isStopped = false;

        NavAgent.SetDestination(PositionToMoveTo);
        NavAgent.stoppingDistance = DistanceToStop;
    }

    private Vector3 GetTargetPoint(Transform actionable)
    {
        var parentTransform = actionable.Find("Navmesh Guidepoints");
        Vector3 closestPoint = actionable.position;
        if (parentTransform != null)
        {
            float closestDist = float.MaxValue;
            foreach (Transform point in parentTransform.transform)
            {
                float tmpDist = Vector3.Distance(OrderlyObject.transform.position, point.position);
                if(tmpDist < closestDist)
                {
                    closestPoint = point.position;
                    closestDist = tmpDist;
                }
            }
        }
        return closestPoint;
    }
}
