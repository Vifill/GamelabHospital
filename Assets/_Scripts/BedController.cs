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
            mBedStation.IsActionActive = value == null;
        }
    }

    public BedStation BedStation
    {
        get
        {
            return mBedStation;
        }
    }
    private BedStation mBedStation;

    private void Start()
    {
        mBedStation = GetComponent<BedStation>();
    }
}
