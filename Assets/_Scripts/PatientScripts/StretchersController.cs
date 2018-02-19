using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchersController : MonoBehaviour 
{
    public GameObject Stretchers;
    public GameObject StandingPatient;
    public GameObject DeathCloth;
    public Transform LyingPosition;

    public bool OnStretchers;
    public bool IsDead;

    private void Start()
    {
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
