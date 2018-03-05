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
            if (LevelManager?.TimeOver ?? false)
            {
                LevelManager.CheckIfAllPatientsAreDone();
            }
        }
    }
    public bool IsInBed = false;
    public bool IsDead = false;

    private PatientMovementController MovementController;
    private StretchersController StretchersController;
    private LevelManager LevelManager;


    // Use this for initialization
    private void Start()
    {
        IsHealed = false;
        MovementController = GetComponent<PatientMovementController>();
        StretchersController = GetComponent<StretchersController>();
        LevelManager = FindObjectOfType<LevelManager>();
        AudioSource = GetComponent<AudioSource>();
    }
    
    
    public void CheckOut()
    {
        LevelManager.AddHealed();
        MovementController.GetOutOfBed();
        if (LevelManager.TimeOver)
        {
            LevelManager.CheckIfAllPatientsAreDone();
        }
    }

    public void Death()
    {
        PlayDeathParticles();
        IsDead = true;
        LevelManager?.AddDeath();
        //LevelManager?.AddPoints(-(ailmentConfig.PointsWhenHealed));
        //AilmentUIController.CreateScorePopUpText(-(ailmentConfig.PointsWhenHealed));
        foreach (var actionable in GetComponents<Actionable>())
        {
            actionable.RemoveHighlight();
            actionable.IsActionActive = false;
        }
        PlayDeathClothSound();
        MovementController.GetOutOfBed();
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
