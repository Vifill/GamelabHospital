using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Actionable : MonoBehaviour 
{
    private AudioSource ActionSoundSource;
    private ActionableParameters Parameters;
    private MouseCursorController MouseCursorController;

    [Header("Actionable Parameters")]
    public float RadiusOfActivation;

    public Actionable GetMostRelevantAction(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        var actionables = GetComponentsInChildren<Actionable>().ToList();
        var actionablesThatCanBeActioned = actionables.Where(a => a.CanBeActioned(pCurrentTool, pObjectActioning));
        return actionablesThatCanBeActioned.FirstOrDefault() ?? this;
    }

    public float ActionTime;
    public GameObject ActionParticles;
    public GameObject ActionSuccessParticles;
    public AudioClip ActionSoundEvent;
    public AudioClip ActionFinishedSoundEvent;
    public string AnimatorParameter;
    public bool IsPickupable;
    public bool IsActionActive;
    public bool DirtiesTool;
    public bool NeedsSanitizedTool;
    public float PlayerDesanitationAmount;

    public abstract bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning);
    public virtual void OnFinishedAction(GameObject pObjectActioning) { }
    public virtual void OnStartAction(GameObject pObjectActioning) { }
    protected virtual void Initialize() { }
    public virtual void OnStopAction() { }

    [Header("Other stuff")]
    public float ActionProgress;

    public virtual ActionableParameters GetActionableParameters(GameObject pObjectActioning = null)
    {
        return new ActionableParameters() { ActionParticles = ActionParticles, ActionSoundClip = ActionSoundEvent, ActionFinishedSoundClip = ActionFinishedSoundEvent, IsPickupable = IsPickupable, RadiusOfActivation = RadiusOfActivation, TimeToTakeAction = ActionTime, AnimationParameter = AnimatorParameter, ActionSuccessParticles = ActionSuccessParticles };
    }

    public bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        bool canUseTool = pCurrentTool == ToolName.NoTool || !NeedsSanitizedTool || (NeedsSanitizedTool && !pObjectActioning.GetComponent<ToolController>().GetToolBase().IsDirty);
        return canUseTool && CanBeActionedExtended(pCurrentTool, pObjectActioning);
    }

    private void Start()
    {
        MouseCursorController = FindObjectOfType<MouseCursorController>();
        Physics.queriesHitTriggers = false;
        IsActionActive = true;
        Initialize();
    }

    public virtual bool IsClose(Transform pPlayer)
    {
        Vector3 tempPos = transform.position;
        tempPos.y = pPlayer.position.y;
        if(GetActionableParameters() != null)
        {
            bool isClose = Vector3.Distance(pPlayer.position, tempPos) <= GetActionableParameters().RadiusOfActivation;
            bool isLookingAt = Vector3.Angle(tempPos - pPlayer.position, pPlayer.forward) <= 80;
            return isClose && isLookingAt;
        }
        return false;
    }

    public virtual void PlayActionSFX()
    {
        ActionSoundSource = GetComponent<AudioSource>();
        var clip = GetActionableParameters().ActionSoundClip;

        if (clip != null)
        {
            ActionSoundSource.PlayOneShot(clip);
        }
    }

    public virtual void PlayFinishedActionSFX()
    {
        ActionSoundSource = GetComponent<AudioSource>();
        var FinishClip = GetActionableParameters()?.ActionFinishedSoundClip;

        if (FinishClip != null)
        {
            //Debug.Log("Action Finished sound");
            ActionSoundSource.PlayOneShot(FinishClip);
        }
    }

    public virtual void StopActionSFX()
    {
        if (ActionSoundSource != null && ActionSoundSource.isPlaying)
        {
            ActionSoundSource.Stop();
        }
    }

    public void SetHighlight(Shader pHighlightShader)
    {
        var renderers = transform.Find("Highlightable")?.GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            List<Material> mats = new List<Material>();

            foreach (Renderer rend in renderers)
            {
                //rend.material.shader = pHighlightShader;
                mats.AddRange(rend.materials);
            }

            foreach (Material mat in mats)
            {
                mat.shader = pHighlightShader;
            }
        }
    }

    public void RemoveHighlight()
    {
        if(gameObject == null)
        {
            return;
        }

        var renderers = transform?.Find("Highlightable")?.GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            List<Material> mats = new List<Material>();

            foreach (Renderer rend in renderers)
            {
                //rend.material.shader = pHighlightShader;
                mats.AddRange(rend.materials);
            }

            foreach (Material mat in mats)
            {
                mat.shader = Shader.Find("Standard");
            }
        }
    }

    public void OnMouseEnter()
    {
        if (GameController.OrderlyInScene && IsActionActive && !GameController.InMenuScreen)
        {
            SetHighlight(FindObjectOfType<HighlightController>().HighlightShader);
            MouseCursorController.SetCursorToClickable();
        }
    }

    public void OnMouseExit()
    {
        if (GameController.OrderlyInScene && IsActionActive && !GameController.InMenuScreen)
        {
            RemoveHighlight();
            MouseCursorController.SetCursorToIdle();
        }
    }

    public void OnMouseDown()
    {
        MouseCursorController.OnClickDown();
    }

    public void OnMouseUp()
    {
        MouseCursorController.OnClickUp();
    }
}

public class ActionableParameters
{
    public float RadiusOfActivation;
    public float TimeToTakeAction;
    public GameObject ActionParticles;
    public GameObject ActionSuccessParticles;
    public AudioClip ActionSoundClip;
    public AudioClip ActionFinishedSoundClip;
    public string AnimationParameter; 
    public bool IsPickupable;
    public float MakePlayerDirty;
}
