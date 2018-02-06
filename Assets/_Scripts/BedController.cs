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

    private GameObject mPatientInBed;
    public GameObject PatientInBed
    {
        get
        {
            return mPatientInBed;
        }
        set
        {
            mPatientInBed = value;
            BedStation.IsActionActive = value == null;
        }
    }
    private BedStation BedStation;

    private void Start()
    {
        BedStation = GetComponent<BedStation>();
    }
}
