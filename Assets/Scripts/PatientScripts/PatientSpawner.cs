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

    private List<GameObject> PatientsToSpawn = new List<GameObject>();
    private float TotalChance = 0;

    private LevelManager LevelManager;
    private Coroutine CurrentCoroutine;

    // Use this for initialization
    private void Start () 
	{
        LevelManager = FindObjectOfType<LevelManager>();
        InitializeSpawnDataModel();
        CurrentCoroutine = StartCoroutine(SpawnCoroutine());
	}

    private void InitializeSpawnDataModel()
    {
        float totalPatients = LevelManager.LevelConfig.LevelTimeSecs / SpawnConfig.SpawnRate;        
        TotalChance = SpawnConfig.ListOfPatientPrefabs.Sum(a=> a.ChanceOfSpawn);

        foreach (var dataModel in SpawnConfig.ListOfPatientPrefabs)
        {
            int numberOfPatientsThisType = Mathf.RoundToInt(dataModel.ChanceOfSpawn / TotalChance * totalPatients);
            for(int i = 0; i < numberOfPatientsThisType; i++)
            {
                PatientsToSpawn.Add(dataModel.PatientPrefab);
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
        int index = UnityEngine.Random.Range(0, PatientsToSpawn.Count-1);
        Instantiate(PatientsToSpawn[index], SpawnPoint.position, SpawnPoint.rotation);
        //Remove that patient
        PatientsToSpawn.RemoveAt(index);
    }

    public void StopSpawning()
    {
        StopCoroutine(CurrentCoroutine);
    }
}
