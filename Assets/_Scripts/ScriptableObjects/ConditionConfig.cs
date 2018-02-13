using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Configs/Ailments/ConditionConfig")]
public class ConditionConfig : ScriptableObject 
{
    public Sprite ImageToShow;
    public GameObject OngoingParticles;
    public GameObject ActionParticles;
    public GameObject SuccessParticles;
    public GameObject FailParticles;
    //public AnimationClip Animation;
    public string AnimatorParameter;
    public int PointsWhenHealed;

    public AudioClip ActionSoundEvent;
    public AudioClip ActionFinishedSoundEvent;
    public string NameOrDescription;
    [Header("Tool config")]
    public ToolName ToolNeeded;
    public bool ToolNeedsToBeSanitized;
    public bool UseUpTool;

    public float ActioningTime;
    public float RadiusOfActivation = 2;
    public float TimeToHeal;
    public float MakePlayerDirtyValue;

    public bool Healed = false;

    internal ActionableParameters GetActionableParameters()
    {
        return new ActionableParameters() { TimeToTakeAction = ActioningTime, RadiusOfActivation = RadiusOfActivation, ActionParticles = ActionParticles, ActionSoundClip = ActionSoundEvent, ActionFinishedSoundClip = ActionFinishedSoundEvent, AnimationParameter = AnimatorParameter, ActionSuccessParticles = SuccessParticles, MakePlayerDirty = MakePlayerDirtyValue };
    }
}
