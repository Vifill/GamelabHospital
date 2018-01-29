using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnConfig : ScriptableObject
{
    public float SpawnRate;
    [Tooltip("How much the spawn rate varies, in seconds")]
    public float RandomVariance;
    
    public List<PatientSpawnModel> ListOfPatientPrefabs;
}

[System.Serializable]
public class PatientSpawnModel
{
    public GameObject PatientPrefab;
    public float ChanceOfSpawn;
}
