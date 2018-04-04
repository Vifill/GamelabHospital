using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class TutorialController : MonoBehaviour
{
    public List<Objective> Objectives;
    public Objective CurrentObjective;

    public Text ObjectiveTextUIObject;

    private int ObjectiveIndex = 0;

    private void Start()
    {
        if(Objectives?.Any() ?? false)
        {
            if(Objectives[0] != null)
            {
                StartNewObjective(Objectives[0]);
            }
        }
    }

    private void StartNewObjective(Objective pNextObjective)
    {
        //Stop listening to the old event if there is one
        if(CurrentObjective != null)
        {
            EventManager.StopListening(CurrentObjective.OnFinishEvent, ObjectiveEnd);
        }
        //Set the next objective
        CurrentObjective = pNextObjective;
        //Start listening to the new event for the new objective
        EventManager.StartListening(CurrentObjective.OnFinishEvent, ObjectiveEnd);
        //Set the new objective text
        ObjectiveTextUIObject.text = CurrentObjective.ObjectiveDescription;
    }
    
    private void ObjectiveEnd()
    {
        ObjectiveIndex++;
        if(Objectives[ObjectiveIndex] != null)
        {
            StartNewObjective(Objectives[ObjectiveIndex]);
        }
    }
}
