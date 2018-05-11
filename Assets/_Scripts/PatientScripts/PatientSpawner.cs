using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatientSpawner : MonoBehaviour 
{
    public SpawnConfig SpawnConfig;
    public Transform SpawnPoint;

    public BedManager BedManager;

    private List<PatientSpawnModel> PatientsToSpawn = new List<PatientSpawnModel>();
    private float TotalChance = 0;

    private LevelManager LevelManager;
    private Coroutine CurrentCoroutine;

    // Use this for initialization
    private void Start () 
	{
        LevelManager = FindObjectOfType<LevelManager>();
        InitializeSpawnDataModel();
        StartSpawnCoroutine();
	}

    public Coroutine GetCurrentCoroutine()
    {
        return CurrentCoroutine;
    }

    public void StartSpawnCoroutine()
    {
        CurrentCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private void InitializeSpawnDataModel()
    {
        float totalPatients = LevelManager.LevelConfig.LevelTimeSecs / SpawnConfig.SpawnRate;        
        TotalChance = SpawnConfig.ListOfPatientConfigs.Sum(a=> a.ChanceOfSpawn);

        foreach (var dataModel in SpawnConfig.ListOfPatientConfigs)
        {
            int numberOfPatientsThisType = Mathf.RoundToInt(dataModel.ChanceOfSpawn / TotalChance * totalPatients);
            for(int i = 0; i < numberOfPatientsThisType; i++)
            {
                PatientsToSpawn.Add(dataModel);
            }
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForEndOfFrame();
        while(true)
        {
            if(BedManager.GetAvailableBeds().Any())
            {
                SpawnPatient();
            }
            else
            {
                Debug.Log("Beds full, patient sent elsewhere");
            }
            yield return new WaitForSeconds(SpawnConfig.SpawnRate + UnityEngine.Random.Range(-SpawnConfig.RandomVariance, SpawnConfig.RandomVariance));
        }
    }

    private void SpawnPatient()
    {
        //Pick random patient in the list to spawn.
        int index = UnityEngine.Random.Range(0, (PatientsToSpawn.Count - 1));
        GameObject patient = (GameObject)Instantiate(SpawnConfig.PatientPrefab, SpawnPoint.position, SpawnPoint.rotation);

        var healthController = patient.GetComponent<HealthController>();
        var patientModel = PatientsToSpawn[index];
        healthController.HydrationHealingConfig = patientModel.HydrationHealingConfig;
        healthController.ThresholdOddsConfig = patientModel.ThresholdOddsConfig;
        healthController.Initialize();
        healthController.Health = UnityEngine.Random.Range(SpawnConfig.CholeraSeverityRange.x, SpawnConfig.CholeraSeverityRange.y);
        //healthController.HydrationMeter = UnityEngine.Random.Range(SpawnConfig.HydrationRange.x, SpawnConfig.HydrationRange.y);
        healthController.SetHydration(UnityEngine.Random.Range(SpawnConfig.HydrationRange.x, SpawnConfig.HydrationRange.y));
        healthController.HydrationConfig = patientModel.HydrationConfig;
        healthController.CholeraConfig = patientModel.CholeraConfig;
        //healthController.BedSanitationConfig = patientModel.BedSanitationThresholdConfig;
        healthController.DoctorSanitationThresholdConfig = patientModel.DoctorSanitationThresholdConfig;

        //Remove that patient
        PatientsToSpawn.RemoveAt(index);
    }

    public void StopSpawning()
    {
        StopCoroutine(CurrentCoroutine);
    }
}
