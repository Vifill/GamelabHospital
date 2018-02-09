using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PatientMovementController : MonoBehaviour
{
    public BedManager BedManager;
    public WaitingRoomSlotManager SlotManager;
    private GameObject ExitPoint;
    private GameObject TargetBed;
    private NavMeshAgent NavMeshAgent;
    private PatientStatusController PatientStatus;
    private StretchersController StretchersController;
    private AilmentController AilmentController;

	// Use this for initialization
	private void Start()
	{
        ExitPoint = GameObject.Find("Exit");
        PatientStatus = GetComponent<PatientStatusController>();
        StretchersController = GetComponent<StretchersController>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        AilmentController = GetComponent<AilmentController>();

        if (BedManager.GetAvailableBeds().Any())
        {
            var bedScript = BedManager.GetAvailableBeds()[0];
            bedScript.GetComponent<BedController>().IsReserved = true;
            TargetBed = bedScript.gameObject;
            NavMeshAgent.SetDestination(TargetBed.transform.position);
        }
        else if (SlotManager.AvailableSlots().Any())
        {
            //SlotManager.AvailableSlots()[0];
            var slotScript = SlotManager.AvailableSlots()[0];
            slotScript.PatientWaiting = gameObject;
            NavMeshAgent.SetDestination(slotScript.transform.position);            
        }
	}
	
	// Update is called once per frame
	private void Update () 
	{

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == TargetBed && !(PatientStatus.IsDead || PatientStatus.IsHealed))
        {
            GoToBed();
        }
        if(other.gameObject == ExitPoint)
        {
            Destroy(gameObject);
        }
    }

    private void GoToBed()
    {
        NavMeshAgent.enabled = false;

        NavMeshAgent.stoppingDistance = 0;

        StretchersController.OnStretchers = false;

        var patientPlacement = TargetBed.transform.GetChild(0);
        transform.SetPositionAndRotation(patientPlacement.position, patientPlacement.rotation);

        PatientStatus.IsInBed = true;
        TargetBed.GetComponent<BedController>().PatientInBed = gameObject;
        AilmentController.IsActionActive = true;
    }

    public void GetOutOfBed()
    {
        NavMeshAgent.enabled = true;
        PatientStatus.IsInBed = false;

        var patientPlacement = new Vector3(TargetBed.transform.position.x, TargetBed.transform.position.y, TargetBed.transform.position.z + 2f);
        transform.SetPositionAndRotation(patientPlacement, TargetBed.transform.rotation);

        TargetBed.GetComponent<BedController>().IsReserved = false;
        TargetBed.GetComponent<BedController>().PatientInBed = null;

        NavMeshAgent.stoppingDistance = 0;

        NavMeshAgent.SetDestination(ExitPoint.transform.position);
        AilmentController.IsActionActive = false;
    }
    
}
