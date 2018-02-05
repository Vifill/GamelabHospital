using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float CholeraSeverity;
    public float HydrationMeter;

    public float DehydrationSpeed;

    private PatientStatusController PatientStatusController;

    private void Start()
    {
        PatientStatusController = GetComponent<PatientStatusController>();
    }

    private void Update()
    {
        if(!PatientStatusController.IsDead)
        {
            HydrationMeter -= DehydrationSpeed * Time.deltaTime;

            if (!PatientStatusController.IsDead && HydrationMeter <= 0)
            {
                PatientStatusController.Death();
            }
        }
    }
}
