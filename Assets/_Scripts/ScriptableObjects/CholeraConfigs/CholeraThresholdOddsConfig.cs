using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Configs/Patient/CholeraThresholdOddsConfig")]
public class CholeraThresholdOddsConfig : ScriptableObject
{
    public AnimationCurve TestCurve;
    public float RandomRangeOffset = 1.5f;
    public List<CholeraThresholdOddModel> ListOfThresholds;
}

[System.Serializable]
public class CholeraThresholdOddModel
{
    public float ThresholdOfActivation;
    public float TimeToExcrete;
}