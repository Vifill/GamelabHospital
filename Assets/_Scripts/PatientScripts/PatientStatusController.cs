using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientStatusController : MonoBehaviour
{
    public GameObject DeathParticles;
    public AudioClip PatientDeathClothSound;

    private AudioSource AudioSource;
    private bool mIsHealed;
    public bool IsHealed
    {
        get
        {
            return mIsHealed;
        }
        set
        {
            mIsHealed = value;
            if (LevelManager.TimeOver)
            {
                LevelManager.CheckIfAllPatientsAreDone();
            }
            if (mIsHealed)
            {
                var hydrationController = GetComponent<HydrationController>();
                hydrationController.IsActionActive = false;
                hydrationController.StopAllHydrations();
            }
        }
    }
    public bool IsInBed = false;
    public bool IsDead = false;

    private HydrationController HydrationController;
    private PatientMovementController MovementController;
    private StretchersController StretchersController;
    private LevelManager LevelManager;
    private HealthController HealthConrtoller;

    // Use this for initialization
    private void Start()
    {
        IsHealed = false;
        HydrationController = GetComponent<HydrationController>();
        MovementController = GetComponent<PatientMovementController>();
        StretchersController = GetComponent<StretchersController>();
        LevelManager = FindObjectOfType<LevelManager>();
        AudioSource = GetComponent<AudioSource>();
        HealthConrtoller = GetComponent<HealthController>();
    }
    
    
    public void CheckOut()
    {
        LevelManager.AddHealed();
        MovementController.GetOutOfBed(IsHealed);
        HealthConrtoller.DestroyHydrationUI();
        if (LevelManager.TimeOver)
        {
            LevelManager.CheckIfAllPatientsAreDone();
        }
    }

    public virtual void Death()
    {
        PlayDeathParticles();
        IsDead = true;
        LevelManager?.AddDeath();
        HealthConrtoller.DestroyHydrationUI();
        //LevelManager?.AddPoints(-(ailmentConfig.PointsWhenHealed));
        //AilmentUIController.CreateScorePopUpText(-(ailmentConfig.PointsWhenHealed));
        foreach (var actionable in GetComponents<Actionable>())
        {
            actionable.RemoveHighlight();
            actionable.IsActionActive = false;
        }
        HydrationController.StopAllHydrations();
        PlayDeathClothSound();
        MovementController.GetOutOfBed(IsHealed);
        StretchersController.IsDead = true;
        StretchersController.OnStretchers = true;

        if (LevelManager.TimeOver)
        {
            LevelManager.CheckIfAllPatientsAreDone();
        }
    }

    public void PlayDeathClothSound()
    {
        if (PatientDeathClothSound != null)
        {
            AudioSource.PlayOneShot(PatientDeathClothSound, 0.1f);
        }
    }

    private void PlayDeathParticles()
    {
        GameObject DeathPar = Instantiate(DeathParticles, transform.position, Quaternion.identity);
        Destroy(DeathPar, 4f);
    }
}
