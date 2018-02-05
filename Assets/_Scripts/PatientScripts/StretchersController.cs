using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchersController : MonoBehaviour 
{
    private GameObject Stretchers;
    private GameObject StandingPatient;
    private GameObject DeathCloth;
    private Transform LyingPosition;

    public bool OnStretchers;
    public bool IsDead;

    private void Start()
    {
        Stretchers = transform.GetChild(0).gameObject;
        StandingPatient = transform.GetChild(1).gameObject;
        LyingPosition = Stretchers.transform.GetChild(0);
        DeathCloth = Stretchers.transform.GetChild(1).gameObject;
        OnStretchers = true;
        IsDead = false;
    }

    private void Update()
    {
        if (OnStretchers)
        {
            StretchersOn();
        }
        else
        {
            StrechersOff();
        }
        if (IsDead)
        {
            DeathCloth.SetActive(true);
        }
        else
        {
            DeathCloth.SetActive(false);
        }
    }

    public void StretchersOn()
    {
        Stretchers.SetActive(true);
        StandingPatient.transform.localPosition = LyingPosition.localPosition;
        StandingPatient.transform.localRotation = LyingPosition.localRotation;
    }

    public void StrechersOff()
    {
        Stretchers.SetActive(false);
        StandingPatient.transform.localPosition = Vector3.zero;
        StandingPatient.transform.localRotation = Quaternion.identity;
    }
}
