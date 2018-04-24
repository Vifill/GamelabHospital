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
        EventActions.Add(EventManager.EventCodes.DoneWalking, OnWalkingDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneHydration, OnHydrateDoneEvent);
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
    }

    private void OnWalkingDoneEvent()
    {
        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.ForcePatientExcretion();
        TutorialUtility.SetHydrationFreeze(true);
    }

    private IEnumerator TutorialStart()
    {
        TutorialUtility.SetTimeFreeze(true);

        yield return new WaitForSeconds(1);

        TutorialUtility.SetSpawnFreeze(true);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(true);
    }

}
