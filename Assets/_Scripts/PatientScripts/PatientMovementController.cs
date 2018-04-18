using Assets._Scripts.Utilities;
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
    public GameObject TargetBed;
    private NavMeshAgent NavMeshAgent;
    private PatientStatusController PatientStatus;
    private StretchersController StretchersController;

	// Use this for initialization
	private void Start()
	{
        ExitPoint = GameObject.Find("Exit");
        PatientStatus = GetComponent<PatientStatusController>();
        StretchersController = GetComponent<StretchersController>();
        NavMeshAgent = GetComponent<NavMeshAgent>();

        if (BedManager.GetAvailableBeds().Any())
        {
            var bedScript = BedManager.GetAvailableBeds()[0];
            bedScript.GetComponent<BedController>().IsReserved = true;
            TargetBed = bedScript.gameObject;
            NavMeshAgent.SetDestination(GetGuidePoint(TargetBed));
        }
        else if (SlotManager.AvailableSlots().Any())
        {
            //SlotManager.AvailableSlots()[0];
            var slotScript = SlotManager.AvailableSlots()[0];
            slotScript.PatientWaiting = gameObject;
            NavMeshAgent.SetDestination(slotScript.transform.position);            
        }
	}

    private Vector3 GetGuidePoint(GameObject targetBed)
    {
        return targetBed.transform.Find(Constants.GuidePoints).transform.position;
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

        var patientPlacement = TargetBed.transform.Find("PatientPlacement");
        transform.SetPositionAndRotation(patientPlacement.position, patientPlacement.rotation);
        transform.SetParent(TargetBed.transform.Find("Highlightable"));

        PatientStatus.IsInBed = true;
        TargetBed.GetComponent<BedController>().PatientInBed = gameObject;

        TargetBed.GetComponent<BedStation>().LerpDirtyBarUIWhenPatientEntersBed(gameObject.GetComponent<HealthController>().HydrationUI);
    }

    public void GetOutOfBed()
    {
        GetComponentInChildren<Animator>().SetBool("IsWalking", true);
        NavMeshAgent.enabled = true;
        PatientStatus.IsInBed = false;

        transform.SetParent(null);
        var patientPlacement = new Vector3(TargetBed.transform.position.x, TargetBed.transform.position.y, TargetBed.transform.position.z + 2f);
        transform.SetPositionAndRotation(patientPlacement, TargetBed.transform.rotation);

        TargetBed.GetComponent<BedController>().IsReserved = false;
        TargetBed.GetComponent<BedController>().PatientInBed = null;

        TargetBed.GetComponent<BedStation>().LerpDirtyBarUIWhenPatientLeavesBed();

        NavMeshAgent.stoppingDistance = 0;

        NavMeshAgent.SetDestination(ExitPoint.transform.position);
    }    
}
