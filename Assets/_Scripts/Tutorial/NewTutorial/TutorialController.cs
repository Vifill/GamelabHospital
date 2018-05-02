﻿using UnityEngine;
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
    public TutorialCoroutineStartLogic StartLogic;

    public Objective CurrentObjective { private set; get; }
    private Dictionary<EventManager.EventCodes, UnityAction> EventActions = new Dictionary<EventManager.EventCodes, UnityAction>();
    private int ObjectiveIndex = 0;
    private List<GameObject> CurrentArrows = new List<GameObject>();
    private Level1TutorialScreenController TutorialScreenController;



    private void Start()
    {
        StartCoroutine(StartLogic.TutorialStartCoroutine());
        AddActionsToEvents();
        
        if(Objectives?.Any() ?? false)
        {
            if(Objectives[0] != null)
            {
                StartNewObjective(Objectives[0]);
            }
        }

        TutorialScreenController = FindObjectOfType<Level1TutorialScreenController>() ?? null;
    }

    private void AddActionsToEvents()
    {
        // Tutorial Level 1
        EventActions.Add(EventManager.EventCodes.DoneWalking, OnWalkingDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneHydration, OnHydrateDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCheckOut, OnCheckOutDoneEvent);
        EventActions.Add(EventManager.EventCodes.DonePatientDeath, OnPatientDeath);
        EventActions.Add(EventManager.EventCodes.DoneWaitingForHealed, OnWaitingForHealDone);
        // Tutorial Level 2
        EventActions.Add(EventManager.EventCodes.DoneGetWaterLvl2, OnDoneGetWaterLvl2Event);
        EventActions.Add(EventManager.EventCodes.DoneHydrationLvl2, OnDoneHydrationLvl2Event);
        EventActions.Add(EventManager.EventCodes.DoneGetBucket, OnGetBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanBed, OnCleanBedDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanBucket, OnCleanBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneReturnBucket, OnReturnBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneFinishingTutorialQueue, DoneFinishingTutorialQueueEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanDoctor, OnCleanDoctorDoneEvent);
    }

    private void DoneFinishingTutorialQueueEvent()
    {
        Debug.Log("DoneTutorialQueue, event triggered.");
        TutorialUtility.SetPlayerSanitation(55);
    }

    private void OnDoneHydrationLvl2Event()
    {
        Debug.Log("DoneHydrationLvl2, event triggered.");
        Time.timeScale = 0;
    }

    private void OnDoneGetWaterLvl2Event()
    {
        Debug.Log("DoneGetWaterLvl2, event triggered.");
    }

    private void OnWaitingForHealDone()
    {
        //TutorialScreenController?.DisplayCheckoutScreen();
    }

    private void OnCleanDoctorDoneEvent()
    {
        Debug.Log("Doctor Cleaned, event triggered.");
        TutorialUtility.SetTimeFreeze(false);
        TutorialUtility.SetTimerUIAsActive(true);

        TutorialUtility.SetSpawnFreeze(false);

        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.SetHealthFreeze(false);
        TutorialUtility.SetFreezeExcretion(false);
    }

    private void OnCleanBucketDoneEvent()
    {
        Debug.Log("Buckets Cleaned, event triggered.");
    }

    private void OnReturnBucketDoneEvent()
    {
        Debug.Log("Buckets Returned, event triggered.");
        Time.timeScale = 1;
    }

    private void OnCleanBedDoneEvent()
    {
        Debug.Log("Bed Cleaned, event triggered.");
    }

    private void OnGetBucketDoneEvent()
    {
        Debug.Log("Buckets picked up, event triggered.");
    }

    private void OnPatientDeath()
    {
        //end level?
        StartCoroutine(EndLevelCoroutine());
    }

    private IEnumerator EndLevelCoroutine()
    {
        yield return new WaitForSeconds(3);
        TutorialUtility.ForceEndLevel();
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
        if(Objectives.Count > ObjectiveIndex)
        {
            StartNewObjective(Objectives[ObjectiveIndex]);
        }
    }
    
    private void OnHydrateDoneEvent()
    {
        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.SetHealthFreeze(false);
        //instantiate infoscreen explaining how the hydration and health system works
        TutorialScreenController?.DisplayHydrationScreen();
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
        TutorialUtility.SetFreezeExcretion(true);

        //yield return new WaitForSeconds(0.2f);

        //TutorialUtility.SetPlayerMovementFreeze(true);

        yield return new WaitForSeconds(6);

        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.ForcePatientExcretion();

    }

    }
