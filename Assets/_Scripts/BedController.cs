using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : MonoBehaviour
{
    public bool IsReserved;
    public bool HasPatient
    {
        get
        {
            return PatientInBed != null;
        }
    }
    
    public GameObject PatientInBed;
}
