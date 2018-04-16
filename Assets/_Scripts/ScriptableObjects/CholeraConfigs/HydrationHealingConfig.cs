using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Patient/HydrationHealingConfigs")]
public class HydrationHealingConfig : ScriptableObject
{
    public List<HealingThresholdModel> ListOfThresholds;
}

[System.Serializable]
public class HealingThresholdModel
{
    public float ThresholdOfActivation;
    public float HealthIncreasePerSecond;
}
