using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour 
{
    public Animator Animator;
    private bool DontClose;

	private void Start() 
	{

	}
	
	private void Update() 
	{
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Patient")
        {
            print("Door trigger activated");
            Animator.SetTrigger(Constants.AnimationParameters.DoorOpen);
        }
    }
}
