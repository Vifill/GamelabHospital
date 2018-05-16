using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Assets._Scripts.Utilities;

public class ActionableActioner : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    protected Actionable CurrentAction;
    private GameObject ActionableParticles;
    private Image ProgressBar;
    private Canvas Canvas;

    public GameObject ProgressBarPrefab;
    public Transform ProgressBarWorldPosition;
    public AudioSource Asource;
    public AudioClip PickUpSound;
    public AudioClip DropSound;
    public AudioClip InvalidActionSound;
    public Animator Animator;
    public GameObject ParticlePrefab;


    public float TotalTime { get; private set; }
    public float CurrentTime { get { return CurrentAction.ActionProgress; } private set { CurrentAction.ActionProgress = value; } }
    private Action<GameObject> ActionAfterFinishing;
    private Action ExternalActionWhenSuccessful;
    private Action ExternalActionWhenFailed;
    private bool IsActioning;
    private bool HasSpawnedFloatingText;
    private MovementController MovementController;
    private SanitationController SanitationController;
    private GameController GC;
    private ToolController ToolController;
    private List<Actionable> HighlightedActions;

    // Use this for initialization
    private void Start () 
	{
        Asource = GetComponent<AudioSource>();
        Canvas = FindObjectOfType<Canvas>();
        SanitationController = GetComponent<SanitationController>();
        GC = FindObjectOfType<GameController>();
        ToolController = GetComponent<ToolController>();

        //ToolController.OnToolSet.AddListener(HighlightPossibleActions);
        //ToolController.OnToolRemove.AddListener(RemoveHiglightedPossibleActions);
    }

    // Update is called once per frame
    private void Update () 
	{
        if (IsActioning)
        {
            if (!CurrentAction.IsActionActive)
            {
                IsActioning = false;
                CurrentTime = 0;
                StopAction();
                ExternalActionWhenFailed?.Invoke();
            }

            CurrentTime += Time.deltaTime;

            if (CurrentTime >= TotalTime)
            {
                OnSuccess();
            }
        }
    }

    private IEnumerator UpdateProgressBar()
    {
        while (IsActioning)
        {
            ProgressBar.fillAmount = CurrentTime / TotalTime;
            ProgressBar.transform.parent.position = Camera.main.WorldToScreenPoint(ProgressBarWorldPosition.position);

            yield return null;
        }
    }



    private IEnumerator UpdateProgressBarOrderly()
    {
        while (IsActioning)
        {
            ProgressBar.fillAmount = CurrentTime / TotalTime;
            yield return null;
        }
        //Debug.Break();
    }

    protected virtual void OnSuccess()
    {
        TransferReasource();
        PlayParticleEffects(CurrentAction.GetActionableParameters().ActionSuccessParticles, CurrentAction.transform);

        CurrentTime = 0;
        StopAction();
        
        ActionAfterFinishing?.Invoke(gameObject);
        ExternalActionWhenSuccessful?.Invoke();
        CurrentAction.PlayFinishedActionSFX();
        ProcessPlayerSanitation();
        ProcessToolAfterSuccess();
    }

    private void ProcessToolAfterSuccess()
    {
        if ((ToolController.GetToolBase()?.IsUsedUpAfterUse ?? false) && CurrentAction.ConsumesTool)
        {
            ToolController.DestroyTool();
        }
        if (CurrentAction.DirtiesTool && ToolController.GetToolBase().NeedsToBeSanitized)
        {
            ToolController.GetToolBase().ToolUsed();
        }
    }

    private void ProcessPlayerSanitation()
    {
        if (CurrentAction.GetActionableParameters().MakesPlayerDirty)
        {
            var model = GC.DoctorSanitationConfig.SanitationModels.SingleOrDefault(a => a.ToolName == ToolController.GetCurrentToolName());
            if(model != null)
            {
                SanitationController.MakePlayerDirty(model.DesanitationAmount);
            }
        }
    }

    internal void AttemptAction(Actionable pAction, MovementController pMovementController = null, Action pExternalActionWhenSuccessful = null, Action pExternalActionWhenFailed = null)
    {
        if (IsActioning)
        {
            return;
        }
        MovementController = pMovementController;
        ExternalActionWhenSuccessful = pExternalActionWhenSuccessful;
        ExternalActionWhenFailed = pExternalActionWhenFailed;
        ActionAfterFinishing = pAction.OnFinishedAction;
        CurrentAction = pAction;
        var parameters = pAction.GetActionableParameters(gameObject);

        if (parameters.ActionParticles != null)
        {
            ActionableParticles = Instantiate(parameters.ActionParticles, pAction.transform); //Start particles
            ActionableParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }

        PlayActionSound();
        PlayAnimation();

        pAction.OnStartAction(gameObject);

        TotalTime = parameters.TimeToTakeAction;
        IsActioning = true;
        pAction.IsBeingActioned = true;
        MovementController?.StopMovement();

        var orderly = GetComponent<OrderlyController>();
        if (orderly != null)
        {
            StartOrderlyActionUI(orderly);
        }
        else
        {
            StartDoctorActionUI(pAction);
        }

    }

    private void StartDoctorActionUI(Actionable pAction)
    {
        CreateProgressBar(pAction);
        StartCoroutine(UpdateProgressBar());
    }

    public void StartOrderlyActionUI(OrderlyController pOrderly)
    {
        ProgressBar = pOrderly.GetCurrentActionIcon();
        StartCoroutine(UpdateProgressBarOrderly());
    }

    private void CreateProgressBar(Actionable pAction)
    {
        var progressBar = Instantiate(ProgressBarPrefab);
        if (pAction.ActionIcon != null)
        {
            progressBar.GetComponent<Image>().sprite = pAction.ActionIcon.GetComponent<Image>().sprite;
        }
        progressBar.transform.SetParent(Canvas.transform);

        DestroyProgressBar();

        ProgressBar = progressBar.transform.GetChild(0).GetComponent<Image>();
    }

    private void PlayAnimation()
    {
        var parameter = CurrentAction.GetActionableParameters()?.AnimationParameter;
        if (Animator != null && !string.IsNullOrEmpty(parameter))
        {
            Animator.SetBool(CurrentAction.GetActionableParameters()?.AnimationParameter, true);
        }
    }

    private void StopAnimation()
    {
        if (Animator != null && !string.IsNullOrEmpty(CurrentAction?.GetActionableParameters()?.AnimationParameter))
        {
            Animator.SetBool(CurrentAction.GetActionableParameters()?.AnimationParameter, false );
        }
    }

    public virtual void StopAction()
    {
        if (ActionableParticles != null)
        {
            Destroy(ActionableParticles);
        }

        StopAnimation();
        CurrentAction?.StopActionSFX();
        CurrentAction?.OnStopAction();

        IsActioning = false;
        if (CurrentAction != null)
        {
            CurrentAction.IsBeingActioned = false;
        }
        DestroyProgressBar();
        MovementController?.StartMovement();

        
    }

    private void DestroyProgressBar()
    {
        if (ProgressBar != null)
        {
            var parent = ProgressBar.transform.parent.gameObject;
            Destroy(ProgressBar.gameObject);
            Destroy(parent);
        }
    }

    public void PlayActionSound()
    {
        if (CurrentAction.IsPickupable)
        {
            Asource.PlayOneShot(PickUpSound);
        }
        else
        {
            CurrentAction.PlayActionSFX();
        }
    }

    public void PlayInvalidActionSound()
    {
        Asource.PlayOneShot(InvalidActionSound);
    }

    private GameObject PlayParticleEffects(GameObject pParticles, Transform pTransform)
    {
        if (pParticles != null)
        {
            var particle = Instantiate(pParticles, pTransform.position, Quaternion.LookRotation(Vector3.forward));
            //particle.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            return particle;
        }
        return null;
    }

    private void HighlightPossibleActions()
    {
        HighlightedActions = FindObjectsOfType<Actionable>().Where(a => a.CanBeActioned(ToolController.GetCurrentToolName(), gameObject) && a.GetComponent<TableStation>() == null).ToList();

        for (int i = 0; i < HighlightedActions.Count; i++)
        {
            HighlightedActions[i].SetHighlight(Shader.Find("Custom/Pulse"));
        }
    }

    private void RemoveHiglightedPossibleActions()
    {
        foreach(var highlightedAction in HighlightedActions.Where(a => a != null))
        {
            highlightedAction.RemoveHighlight();
        }

        //for (int i = 0; i < HighlightedActions.Count; i++)
        //{
        //    HighlightedActions[i]?.RemoveHighlight();
        //}
        HighlightedActions.Clear();
    }

    public void SpawnFloatingText(string pInputText = "I need empty hands to do this")
    {
        if (HasSpawnedFloatingText || FloatingTextPrefab == null)
        {
            return;
        }

        HasSpawnedFloatingText = true;
        GameObject GO = (GameObject) Instantiate(FloatingTextPrefab, FindObjectOfType<Canvas>().transform);
        Text goText = GO.GetComponentInChildren<Text>();
        GO.transform.position = Camera.main.WorldToScreenPoint(transform.position + transform.up * 2);
        goText.text = pInputText;
        Destroy(GO, 5f);
        StartCoroutine(FloatingTextDelay());
    }

    private IEnumerator FloatingTextDelay()
    {
        yield return new WaitForSeconds(1f);
        HasSpawnedFloatingText = false;
    }

    private void TransferReasource()
    {

        if (ParticlePrefab == null)
        {
            return;
        }

        if (CurrentAction is HydrationController)
        {
            int transferAmount = /*(int)UnityEngine.Random.Range(5, 10)*/ 1;
            Image hydrationMeter = CurrentAction.GetComponent<HealthController>().HydrationUI.GetComponent<HydrationUIManager>().HydrationMeterUI;
            Vector3 target = hydrationMeter.transform.position;
            Color color = hydrationMeter.color;
            for (int i = 0; i < transferAmount; i++)
            {
                StartCoroutine(TransferItemCoroutine(i / 10.0f, hydrationMeter.transform, true, transform, false, color));
            }
        }
        else if (CurrentAction is BedStation)
        {
            int transferAmount = /*(int)UnityEngine.Random.Range(5, 10)*/ 1;
            Image dirtyMeter = CurrentAction.GetComponent<BedStation>().BarFill;
            Vector3 start = dirtyMeter.transform.position;
            Color color = dirtyMeter.color;
            for (int i = 0; i < transferAmount; i++)
            {
                StartCoroutine(TransferItemCoroutine(i / 10.0f, transform, false, dirtyMeter.transform, true, color));
            }
        }
        else
        {
            return;
        }
    }

    private IEnumerator TransferItemCoroutine(float pDelay, Transform pTargetTrans, bool pTargetIsUI, Transform pStartTrans, bool pStartIsUI, Color pColor)
    {
        yield return new WaitForSeconds(pDelay);

        GameObject ParticleInstance = Instantiate(ParticlePrefab, Canvas.transform);
        ParticleInstance.GetComponent<Image>().color = pColor;
        if (pTargetIsUI)
        {
            ParticleSystem.MainModule main = ParticleInstance.GetComponentInChildren<ParticleSystem>().main;
            main.startColor = pColor;
        }
        else
        {
            ParticleSystem.MainModule main = ParticleInstance.GetComponentInChildren<ParticleSystem>().main;
            main.startColor = pColor;
        }
        List<Vector3> BezierCurve = GetBezierApproximation(CalculateControlPoints(pTargetTrans, pTargetIsUI, pStartTrans, pStartIsUI), 5);
        ParticleInstance.transform.position = BezierCurve[0];
        
        while ((ParticleInstance.transform.position - BezierCurve.Last()).sqrMagnitude > 1)
        {

            ParticleInstance.transform.position = Vector3.MoveTowards(ParticleInstance.transform.position, BezierCurve[0], Time.deltaTime * Screen.height * .5f );
            if ((ParticleInstance.transform.position - BezierCurve[0]).sqrMagnitude < 2)
            {
                BezierCurve.Remove(BezierCurve[0]);
            }
      
            if (BezierCurve.Count <= 0)
            {
                break;
            }
            yield return null;
        }
        ParticleSystem.EmissionModule emission = ParticleInstance.GetComponentInChildren<ParticleSystem>().emission;
        emission.enabled = false;
        Destroy(ParticleInstance, 3);
    }

    Vector3[] CalculateControlPoints(Transform pTargetTrans, bool pTargetIsUI, Transform pStartTrans, bool pStartIsUI)
    {
        Vector3 uiTargetPos = Camera.main.WorldToScreenPoint(pTargetTrans.position);
        Vector3 uiStartPos = Camera.main.WorldToScreenPoint(pStartTrans.position);
        if (pTargetIsUI)
        {
            uiTargetPos = pTargetTrans.position;
        }
        if (pStartIsUI)
        {
            uiStartPos = pStartTrans.position;
        }

        Vector3 secondPoint = new Vector3(uiTargetPos.x + 2 * (Screen.height/10 * Math.Sign(uiStartPos.x - uiTargetPos.x)), uiTargetPos.y, 0);
        if (pStartIsUI)
        {
            secondPoint = new Vector3(uiStartPos.x + 2 * (Screen.height / 10 * Math.Sign(uiStartPos.x - uiTargetPos.x)), uiStartPos.y, 0);
        }
        Vector3[] controlPoints = { uiStartPos, secondPoint, uiTargetPos };
        return controlPoints;
    }

    List<Vector3> GetBezierApproximation(Vector3[] controlPoints, int outputSegmentCount)
    {
        Vector3[] points = new Vector3[outputSegmentCount + 1];
        for (int i = 0; i <= outputSegmentCount; i++)
        {
            float t = (float)i / outputSegmentCount;
            points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
        }
        return points.ToList();
    }

    Vector3 GetBezierPoint(float t, Vector3[] controlPoints, int index, int count)
    {
        if (count == 1)
            return controlPoints[index];

        var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
        var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
        return new Vector3((1 - t) * P0.x + t * P1.x, (1 - t) * P0.y + t * P1.y, 0);
    }
}
