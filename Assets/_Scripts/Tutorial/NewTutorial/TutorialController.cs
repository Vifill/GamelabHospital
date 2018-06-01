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
    public TutorialCoroutineStartLogic StartLogic;

    public Objective CurrentObjective { private set; get; }
   

    private Dictionary<EventManager.EventCodes, UnityAction> EventActions = new Dictionary<EventManager.EventCodes, UnityAction>();
    private int ObjectiveIndex = 0;
    private List<GameObject> CurrentArrows = new List<GameObject>();
    private Level1TutorialScreenController TutorialScreenController;
    private bool CanEndLevel;
    private int InitializedPatients = 0;



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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && CanEndLevel && !GameController.InMenuScreen)
        {
            EventManager.TriggerEvent(EventManager.EventCodes.FinishLevel);
        }

        if (GameController.InMenuScreen && ObjectiveTextUIObject.gameObject.activeSelf || LevelManager.TimeOver && ObjectiveTextUIObject.gameObject.activeSelf)
        {
            ObjectiveTextUIObject.gameObject.SetActive(false);
        }
        else if (!GameController.InMenuScreen && !ObjectiveTextUIObject.gameObject.activeSelf && !LevelManager.TimeOver)
        {
            ObjectiveTextUIObject.gameObject.SetActive(true);
        }
    }

    private void AddActionsToEvents()
    {
        // Tutorial Level 1
        EventActions.Add(EventManager.EventCodes.DoneWalking, OnWalkingDoneEvent);
        EventActions.Add(EventManager.EventCodes.PatientInitialized, OnPatientInitialized);
        EventActions.Add(EventManager.EventCodes.DonePuking, OnPukingDone);
        EventActions.Add(EventManager.EventCodes.DoneHydration, OnHydrateDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCheckOut, OnCheckOutDoneEvent);
        EventActions.Add(EventManager.EventCodes.DonePatientDeath, OnPatientDeath);
        EventActions.Add(EventManager.EventCodes.FinishLevel, OnFinishLevel);
        // Tutorial Level 2
        EventActions.Add(EventManager.EventCodes.DoneGetWaterLvl2, OnDoneGetWaterLvl2Event);
        EventActions.Add(EventManager.EventCodes.DoneHydrationLvl2, OnDoneHydrationLvl2Event);
        EventActions.Add(EventManager.EventCodes.DoneGetBucket, OnGetBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanBed, OnCleanBedDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanBucket, OnCleanBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneReturnBucket, OnReturnBucketDoneEvent);
        EventActions.Add(EventManager.EventCodes.DoneFinishingTutorialQueue, DoneFinishingTutorialQueueEvent);
        EventActions.Add(EventManager.EventCodes.DoneCleanDoctor, OnCleanDoctorDoneEvent);
        EventActions.Add(EventManager.EventCodes.PatientInitializedLvl2, OnPatientInitializedLvl2Event);
    }

    private void OnPatientInitializedLvl2Event()
    {
        TutorialUtility.SetPatientHydration(50);
        TutorialUtility.SetPatientHealth(50);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(true);
        TutorialUtility.SetFreezeExcretion(true);
    }

    private void OnFinishLevel()
    {
        StartCoroutine(EndLevelCoroutine(0));
    }

    private void OnPukingDone()
    {
        TutorialUtility.SetActionablesActive(true);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(true);
        TutorialUtility.SetFreezeExcretion(true);
    }
    private void DoneFinishingTutorialQueueEvent()
    {
        Debug.Log("DoneTutorialQueue, event triggered.");
        TutorialUtility.SetPlayerSanitation(75);
    }

    private void OnDoneHydrationLvl2Event()
    {
        Debug.Log("DoneHydrationLvl2, event triggered.");
        var orderly = FindObjectOfType<OrderlyController>();
        orderly.CancelOrder();
        TutorialUtility.SetBucketTableActive(true);
        Time.timeScale = 0;
    }

    private void OnDoneGetWaterLvl2Event()
    {
        Debug.Log("DoneGetWaterLvl2, event triggered.");
    }

    private void OnCleanDoctorDoneEvent()
    {
        Debug.Log("Doctor Cleaned, event triggered.");

        TutorialUtility.SetSpawnFreeze(false);

        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.SetHealthFreeze(false);
        TutorialUtility.SetFreezeExcretion(false);
        CanEndLevel = true;
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
        TutorialUtility.SetSpawnFreeze(false);
        CanEndLevel = true;
    }

    private IEnumerator EndLevelCoroutine(float pDelay)
    {
        yield return new WaitForSeconds(pDelay);
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
            List<Tuple<Transform, Vector3>> realPoses =  GetArrowPosition(position);
            foreach (var realPos in realPoses)
            {
                var arrowObj = Instantiate(ArrowPrefab);
                arrowObj.transform.position = realPos.Item2;
                arrowObj.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                CurrentArrows.Add(arrowObj);
            }
        }
    }

    private List<Tuple<Transform, Vector3>> GetArrowPosition(Transform pPosition)
    {
        var arrowPlacement = pPosition.GetComponent<ArrowPlacement>();
        if (arrowPlacement)
        {
            return arrowPlacement.TransformsToPutArrowOn.Select(a => new Tuple<Transform, Vector3>(a, a.position + arrowPlacement.Offset)).ToList();
        }
        return new List<Tuple<Transform, Vector3>> {new Tuple<Transform, Vector3>(pPosition, Vector3.zero)};
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
        StartCoroutine(SpawnPatient());
    }

    private IEnumerator SpawnPatient()
    {
        TutorialUtility.SetSpawnFreeze(false);

        yield return new WaitForEndOfFrame();

        TutorialUtility.SetSpawnFreeze(true);
    }

    private void OnPatientInitialized()
    {
        InitializedPatients++;
        switch (InitializedPatients)
        {
            case 1:
                { 
                    TutorialUtility.SetPatientHydration(100);
                    TutorialUtility.SetPatientHealth(80);
                    TutorialUtility.SetHealthFreeze(true);
                    TutorialUtility.SetFreezeExcretion(true);
                    TutorialUtility.SetConstantDehydrationFreeze(true);
                    TutorialUtility.ForcePatientSickness();
                }
                break;
            case 2:
                {
                    TutorialUtility.SetPatientHydration(5);
                    TutorialUtility.SetPatientHealth(10);
                    TutorialUtility.SetHydrationFreeze(true);
                    TutorialUtility.SetHealthFreeze(true);
                    TutorialUtility.SetFreezeExcretion(true);
                    FindObjectOfType<HealthController>().GetComponents<Actionable>().ToList().ForEach(a => a.IsActionActive = false);

                    StartCoroutine(PatientDeathSequence());
                }
                break;
        }
        
    }

    private void OnCheckOutDoneEvent()
    {
        StartCoroutine(SpawnPatient());
    }

    private IEnumerator PatientDeathSequence()
    {
        yield return new WaitForSeconds(6);

        TutorialUtility.SetHydrationFreeze(false);
        TutorialUtility.ForcePatientExcretion();
    }

}
