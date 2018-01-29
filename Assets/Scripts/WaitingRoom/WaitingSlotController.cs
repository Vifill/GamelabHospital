using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingSlotController : MonoBehaviour 
{
    public GameObject PatientWaiting; 
    public bool HasPatient { get { return PatientWaiting != null; } }
}
