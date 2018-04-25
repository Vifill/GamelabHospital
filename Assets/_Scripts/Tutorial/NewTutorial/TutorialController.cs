using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class TutorialController : MonoBehaviour
{
    public List<Objective> Objectives;
    public GameObject ArrowPrefab;
    public Text ObjectiveTextUIObject;

    private Objective CurrentObjective;
    private Dictionary<EventManager.EventCodes, UnityAction> EventActions = new Dictionary<EventManager.EventCodes, UnityAction>();
    private int ObjectiveIndex = 0;
    private List<GameObject> CurrentArrows = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(TutorialStart());
        AddActionsToEvents();

        if(Objectives?.Any() ?? false)
        {
            if(Objectives[0] != null)
            {
                StartNewObjective(Objectives[0]);
            }
        }
    }

    private void AddActionsToEvents()
    {
        // Tutorial Level 1
        EventActions.Add(EventManager.EventCodes.DoneWalking, OnWalkingDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneHydration, OnHydrateDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCheckOut, OnCheckOutDoneEvent);
        EventActions.Add(EventManager.EventCodes.DonePatientDeath, OnPatientDeath);
        // Tutorial Level 2
        EventActions.Add(EventManager.EventCodes.DoneGetBucket, OnGetBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanBed, OnCleanBedDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanBucket, OnCleanBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneReturnBucket, OnReturnBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanDoctor, OnCleanDoctorDoneEvent);
    }

    private void OnCleanDoctorDoneEvent()
    {
        throw new NotImplementedException();
    }

    private void OnCleanBucketDoneEvent()
    {
        throw new NotImplementedException();
    }

    private void OnReturnBucketDoneEvent()
    {
        throw new NotImplementedException();
    }

    private void OnCleanBedDoneEvent()
    {
        throw new NotImplementedException();
    }

    private void OnGetBucketDoneEvent()
    {
        throw new NotImplementedException();
    }

    private void OnPatientDeath()
    {
        //end level?
    }

    private void StartNewObjective(Objective pNextObjective)
    {
        //Clean up last objective
        if(CurrentObjective != null)
        {
            EventManager.StopListening(CurrentObjective.OnFinishEvent, ObjectiveEnd);
            CurrentArrows.ForEach(a => Destroy(a));
            CurrentArrows.Clear();
        }
        //Set the next objective
        CurrentObjective = pNextObjective;
        //Start listening to the new event for the new objective
        EventManager.StartListening(CurrentObjective.OnFinishEvent, ObjectiveEnd);
        //Set the new objective text
        ObjectiveTextUIObject.text = CurrentObjective.ObjectiveDescription;

        SpawnArrowsForObjective(pNextObjective);
    }

    private void SpawnArrowsForObjective(Objective pObjective)
    {
        foreach(Transform position in pObjective.GetArrowPositions())
        {
            var arrowObj = Instantiate(ArrowPrefab, position);
            CurrentArrows.Add(arrowObj);
        }
    }

    private void ObjectiveEnd()
    {
        if(EventActions.ContainsKey(CurrentObjective.OnFinishEvent))
        {
            EventActions[CurrentObjective.OnFinishEvent].Invoke();
        }

        ObjectiveIndex++;
        if(Objectives[ObjectiveIndex] != null)
        {
            StartNewObjective(Objectives[ObjectiveIndex]);
        }
    }
    
    private void OnHydrateDoneEvent()
    {
        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.SetHealthFreeze(false);
        //instantiate infoscreen explaining how the hydration and health system works
    }

    private void OnWalkingDoneEvent()
    {
        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.ForcePatientExcretion();
        TutorialUtility.SetHydrationFreeze(true);
    }

    private void OnCheckOutDoneEvent()
    {
        StartCoroutine(SpawnNewPatient());
    }

    private IEnumerator SpawnNewPatient()
    {
        TutorialUtility.SetSpawnFreeze(false);

        yield return new WaitForSeconds(0.1f);

        TutorialUtility.SetSpawnFreeze(true);
        TutorialUtility.SetPatientHydration(10);
        TutorialUtility.SetPatientHealth(5);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(true);
        TutorialUtility.SetExcretionFreeze(true);

        yield return new WaitForSeconds(0.2f);

        TutorialUtility.SetPlayerMovementFreeze(true);

        yield return new WaitForSeconds(6);

        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.ForcePatientExcretion();

    }

    private IEnumerator TutorialStart()
    {
        TutorialUtility.SetTimeFreeze(true);
        TutorialUtility.SetTimerUIAsActive(false);

        yield return new WaitForSeconds(0.1f);

        TutorialUtility.SetSpawnFreeze(true);
        TutorialUtility.SetPatientHydration(100);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(true);
        TutorialUtility.SetExcretionFreeze(true);
    }

}
