using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AilmentController : Actionable 
{
    public AilmentConfig AilmentConfig;
    public AudioClip PatientHealedSuccessSound;
    public AudioClip PatientDeathClothSound;
    private PointsUIManager PointsUIManager;
    private LevelManager LevelManager;
    private PatientStatusController PatientStatusCtrl;
    private AilmentUIController AilmentUI;
    private float TotalPointsToGive;

    private GameObject CurrentOngoingParticle;
    private AudioSource AudioSrc;
    public float ConditionTimer { get; private set; }

    protected override void Initialize()
    {
        InstantiateScriptableObjects();
        LevelManager = FindObjectOfType<LevelManager>();
        PointsUIManager = FindObjectOfType<PointsUIManager>();
        PatientStatusCtrl = GetComponent<PatientStatusController>();
        AilmentUI = GetComponent<AilmentUIController>();
        StartCoroutine(PlayOngoingParticles());
        AudioSrc = GetComponents<AudioSource>()[1]; 
        IsActionActive = false;
    }

    private void Update()
    {
        if (GetCurrentCondition() != null && GetCurrentCondition().TimeToHeal != 0)
        {
            ConditionTimer += Time.deltaTime;
            AilmentUI.UpdateConditionTimerUI(ConditionTimer);

            if (ConditionTimer >= GetCurrentCondition().TimeToHeal && !PatientStatusCtrl.IsDead)
            {
                PatientStatusCtrl.Death();
                GivePointsToPlayer(-(AilmentConfig.PointsWhenHealed));
            }
        }
    }

    private void InstantiateScriptableObjects()
    {
        AilmentConfig = Instantiate(AilmentConfig);
        AilmentConfig.Conditions = AilmentConfig.Conditions.Select(a => Instantiate(a)).ToList();
    }

    public override ActionableParameters GetActionableParameters(GameObject pObjectActioning = null)
    {
        return GetCurrentCondition()?.GetActionableParameters();
    }

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        var status = GetComponent<PatientStatusController>();
        var condition = GetCurrentCondition();
        //Sanitation check
        if(condition != null && condition.ToolNeedsToBeSanitized)
        {
            ToolBase tool = pObjectActioning.GetComponent<ToolController>().GetToolBase();            
            if(tool != null ? tool.IsDirty : false)
            {
                return false;
            }
        }

        return IsActionActive && !status.IsDead && !status.IsHealed && status.IsInBed && condition.ToolNeeded == pCurrentTool;
    }
    

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        var currentCondition = GetCurrentCondition();
        CalculatePointsGained();
        currentCondition.Healed = true;
        ConditionTimer = 0;
        pObjectActioning.GetComponent<SanitationController>().MakePlayerDirty(currentCondition.MakePlayerDirtyValue);
        GetComponent<AilmentUIController>().UpdateConditionUI();
        var toolController = pObjectActioning.GetComponent<ToolController>();
        if (currentCondition.ToolNeedsToBeSanitized)
        {
            toolController.GetToolBase().ToolUsed();
        }
        if(currentCondition.UseUpTool)
        {
            toolController.DestroyTool();
        }
        //TODO: Play success sound?

        if(GetCurrentCondition() == null)
        {
            PlaySuccessHealSound();
            GivePointsToPlayer(AilmentConfig.PointsWhenHealed + TotalPointsToGive);
            GetComponent<PatientStatusController>().IsHealed = true;
            StopEmmittingOngoing();
        }
        else
        {
            StartCoroutine(PlayOngoingParticles());
        }
    }

    private void CalculatePointsGained()
    {
        var CurrCondition = GetCurrentCondition();
        var pointsGained = 0.0;

        if (CurrCondition.TimeToHeal != 0)
        {
            pointsGained = CurrCondition.PointsWhenHealed / CurrCondition.TimeToHeal * (CurrCondition.TimeToHeal - ConditionTimer);
        } 
        else
        {
            pointsGained = CurrCondition.PointsWhenHealed;
        }
        TotalPointsToGive += (int)Math.Round(pointsGained);
        //Debug.Log("Points gained: " + pointsGained + " adds up to: " + TotalPointsToGive + " total points");
    }

    private void GivePointsToPlayer(float pPointsToGive)
    {
        if (pPointsToGive < 0)
        {
            LevelManager?.AddDeath();
        }
        else if (pPointsToGive > 0)
        {
            LevelManager?.AddHealed();
        }
          
        LevelManager?.AddPoints((int)pPointsToGive);
        AilmentUI.CreateScorePopUpText((int)pPointsToGive);
        StartCoroutine(PointsUIManager.UpdateUI((int)pPointsToGive));
    }

    private IEnumerator PlayOngoingParticles()
    {
        yield return new WaitForEndOfFrame();
        //Destroy(CurrentOngoingParticle?.gameObject);
        StopEmmittingOngoing();
        var currentCondition = GetCurrentCondition();
        CurrentOngoingParticle = PlayParticleEffects(currentCondition?.OngoingParticles);
    }

    private GameObject PlayParticleEffects(GameObject pParticles)
    {
        if(pParticles != null)
        {
            var particle = Instantiate(pParticles, transform, false);
            particle.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            return particle;
        }
        return null;
    }

    public void StopEmmittingOngoing()
    {
        if(CurrentOngoingParticle != null)
        {
            CurrentOngoingParticle.GetComponentsInChildren<ParticleSystem>().ToList().ForEach(a => a.Stop());
            CurrentOngoingParticle.transform.parent = null;
        }        
    }

    public void PlaySuccessHealSound()
    {
        if (PatientHealedSuccessSound != null)
        {
            AudioSrc.PlayOneShot(PatientHealedSuccessSound, 0.1f);
        }
    }

    public void PlayDeathClothSound()
    {
        if (PatientDeathClothSound != null)
        {
            AudioSrc.PlayOneShot(PatientDeathClothSound, 0.1f);
        }
    }

    public ConditionConfig GetCurrentCondition()
    {
        return AilmentConfig.Conditions.Where(a=> !a.Healed).FirstOrDefault();
    }    
}
