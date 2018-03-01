using UnityEngine;

class PatientCheckoutController : Actionable
{
    public GameObject CanBeCheckedOutVisual;

    private GameObject CanBeCheckedOutParticleInstance;
    private PatientStatusController PatientStatusController;
    private HydrationController HydrationController;
    private bool IsCheckingOut = false;

    protected override void Initialize()
    {
        PatientStatusController = GetComponent<PatientStatusController>();
        HydrationController = GetComponent<HydrationController>();
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
    }

    private void Update()
    {
        if ((PatientStatusController?.IsHealed ?? false) && (HydrationController?.enabled ?? false))
        {
            if (CanBeCheckedOutVisual != null)
            {
                CanBeCheckedOutParticleInstance = (GameObject)Instantiate(CanBeCheckedOutVisual, new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), Quaternion.identity);
            }
            HydrationController.enabled = false;
        }
    }
}