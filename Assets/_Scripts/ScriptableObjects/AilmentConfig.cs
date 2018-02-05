using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Ailments/AilmentConfig")]
public class AilmentConfig : ScriptableObject 
{
    public List<ConditionConfig> Conditions;
    public string NameOfAilment;
    public int PointsWhenHealed;
}