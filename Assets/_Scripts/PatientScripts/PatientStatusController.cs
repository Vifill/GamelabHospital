using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientStatusController : MonoBehaviour
{
    public GameObject DeathParticles;
    private bool mIsHealed;
    public bool IsHealed
    {
        get
        {
            return mIsHealed;
        }
        set
        {
            if (value)
            {
                MovementController.GetOutOfBed();
            }
            mIsHealed = value;
            if (LevelManager?.TimeOver ?? false)
            {
                LevelManager.CheckIfAllPatientsAreDone();
            }
        }
    }
    public bool IsInBed = false;
    public float MaxHealth;
    public float CurHealth;
    public bool IsWounded;
    public bool IsDead = false;

    private PatientMovementController MovementController;
    private StretchersController StretchersController;
    private LevelManager LevelManager;
    private AilmentUIController AilmentUIController;
    private float DPS;

    // Use this for initialization
    private void Start()
    {
        IsHealed = false;
        MovementController = GetComponent<PatientMovementController>();
        StretchersController = GetComponent<StretchersController>();
        LevelManager = FindObjectOfType<LevelManager>();
        AilmentUIController = GetComponent<AilmentUIController>();
    }

    private void Update()
    {
        if (IsWounded)
        {
            CurHealth -= DPS * Time.deltaTime;
            Mathf.Clamp(CurHealth, 0, MaxHealth);
        }

        //if (CurHealth <= 0)
        //{
        //    Death();
        //}
    }

    public void AddDPS(float pDPS)
    {
        DPS += pDPS;
        IsWounded = true;
    }

    public void RemoveDPS(float pDPS)
    {
        DPS -= pDPS;

        if (DPS <= 0)
        {
            IsWounded = false;
        }
    }

    public void Heal (float pAmount)
    {
        CurHealth += pAmount;
    }

    public void Death()
    {
        PlayDeathParticles();
        FindObjectOfType<PlayerActionController>().StopAction(GetComponent<AilmentController>());
        var ailmentController = GetComponent<AilmentController>();
        ailmentController.StopEmmittingOngoing();
        ailmentController.IsActionActive = false;
        IsDead = true;
        LevelManager?.AddDeath();
        //LevelManager?.AddPoints(-(ailmentConfig.PointsWhenHealed));
        //AilmentUIController.CreateScorePopUpText(-(ailmentConfig.PointsWhenHealed));
        ailmentController.PlayDeathClothSound();
        MovementController.GetOutOfBed();
        StretchersController.IsDead = true;
        StretchersController.OnStretchers = true;

        if (LevelManager.TimeOver)
        {
            LevelManager.CheckIfAllPatientsAreDone();
        }
    }

    private void PlayDeathParticles()
    {
        GameObject DeathPar = Instantiate(DeathParticles, transform.position, Quaternion.identity);
        Destroy(DeathPar, 4f);
    }
}
