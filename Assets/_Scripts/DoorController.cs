using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorController : MonoBehaviour 
{
    public Animator Animator;
    private bool IsOpen;
    private List<GameObject> PeopleAtDoor = new List<GameObject>();

	private void Start() 
	{

	}
	
	private void Update() 
	{
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("poopy player door enter");
        }

        if (other.tag == "Patient" || other.tag == "Player" && LevelManager.TimeOver)
        {
            PeopleAtDoor.Add(other.gameObject);
            if (!IsOpen)
            {
                Animator.SetBool(Constants.AnimationParameters.DoorOpen, true);
                IsOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            print("poopy player door exit");
        }

        if (other.tag == "Patient" || other.tag == "Player" && LevelManager.TimeOver)
        {
            PeopleAtDoor.Remove(other.gameObject);
            if (IsOpen && !PeopleAtDoor.Any())
            {
                Animator.SetBool(Constants.AnimationParameters.DoorOpen, false);
                IsOpen = false;
            }
        }
    }
}
