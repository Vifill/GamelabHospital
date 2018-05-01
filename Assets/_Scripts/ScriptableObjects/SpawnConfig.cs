using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Levels/SpawnConfig")]
public class SpawnConfig : ScriptableObject
{
    public float SpawnRate;
    [Tooltip("How much the spawn rate varies, in seconds")]
    public float RandomVariance;

    public Vector2 CholeraSeverityRange;
    public Vector2 HydrationRange;

    public GameObject PatientPrefab;

    public List<PatientSpawnModel> ListOfPatientConfigs;
}

[System.Serializable]
public class PatientSpawnModel
{
    public HydrationConfig HydrationConfig;
    public CholeraConfig CholeraConfig;
    public CholeraThresholdOddsConfig ThresholdOddsConfig;
    //public SanitationThresholdConfig BedSanitationThresholdConfig;
    public HydrationHealingConfig HydrationHealingConfig;
    public SanitationThresholdConfig DoctorSanitationThresholdConfig;
    public float ChanceOfSpawn;
}
