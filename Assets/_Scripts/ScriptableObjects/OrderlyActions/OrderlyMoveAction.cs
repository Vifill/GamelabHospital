using Assets._Scripts.Utilities;
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

        if(Vector3.Distance(PositionToMoveTo, OrderlyObject.transform.position) <= DistanceWhenCloseEnough)
        {
            NavAgent.isStopped = true;
            
            ActionFinished();
            
        }
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
            PositionToMoveTo = GetTargetPoint(Target);
        }

        NavAgent = OrderlyObject.GetComponent<NavMeshAgent>();
        NavAgent.isStopped = false;

        NavAgent.SetDestination(PositionToMoveTo);
        NavAgent.stoppingDistance = DistanceToStop;

        Vector3 particlePos = PositionToMoveTo + new Vector3(0, 0.1f, 0);
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

        Vector3 particlePos = OrderlyObject.transform.position - new Vector3(0, 1, 0);
        SetDestinationParticle(particlePos, true);
    }

    private Vector3 GetTargetPoint(Transform actionable)
    {
        if (actionable.GetComponent<Actionable>() != null)
        {
            var parentTransform = actionable.root.Find(Constants.GuidePoints);
            Vector3 closestPoint = actionable.position;
            if (parentTransform != null)
            {
                float closestDist = float.MaxValue;
                foreach (Transform point in parentTransform.transform)
                {
                    float tmpDist = Vector3.Distance(OrderlyObject.transform.position, point.position);
                    if (tmpDist < closestDist)
                    {
                        closestPoint = point.position;
                        closestDist = tmpDist;
                    }
                }
            }
            return closestPoint;
        }
        else
        {
            return actionable.position;
        }
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

        particle.transform.position = pPosition;
    }
}
