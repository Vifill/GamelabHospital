using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Configs/Patient/SanitationThresholdConfig")]
public class SanitationThresholdConfig : ScriptableObject
{
    public List<SanitationThresholdModel> ListOfThresholds;
}

[System.Serializable]
public class SanitationThresholdModel
{
    public float ThresholdOfActivation;
    public float CholeraSeverityIncreasePerSecond;
}

