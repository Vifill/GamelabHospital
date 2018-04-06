﻿using UnityEngine;

public class PatientCheckoutController : Actionable
{
    public GameObject CanBeCheckedOutVisual;

    private GameObject CanBeCheckedOutParticleInstance;
    private PatientStatusController PatientStatusController;
    private HydrationController HydrationController;
    private Level1TutorialScreenController TutorialScreenController;
    public bool IsCheckingOut = false;

    protected override void Initialize()
    {
        PatientStatusController = GetComponent<PatientStatusController>();
        HydrationController = GetComponent<HydrationController>();
        TutorialScreenController = FindObjectOfType<Level1TutorialScreenController>() ?? null;
    }
    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        return PatientStatusController.IsHealed && !IsCheckingOut;
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        IsCheckingOut = true;
        PatientStatusController.CheckOut();
        Destroy(CanBeCheckedOutParticleInstance);
        IsActionActive = false;
    }

    private void Update()
    {
        if ((PatientStatusController?.IsHealed ?? false) && (HydrationController?.enabled ?? false))
        {

            if(TutorialScreenController != null)
            {
                TutorialScreenController.DisplayCheckoutScreen();
            }

            if (CanBeCheckedOutVisual != null)
            {
                CanBeCheckedOutParticleInstance = (GameObject)Instantiate(CanBeCheckedOutVisual, new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), Quaternion.identity);
            }
            HydrationController.enabled = false;
        }
    }
}