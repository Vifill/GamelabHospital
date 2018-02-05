using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Patient")]
public class PatientConfig : ScriptableObject
{
    public List<AilmentConfig> Ailments;
}
