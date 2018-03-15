using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Configs/DoctorSanitationConfig")]
public class DoctorSanitationConfig : ScriptableObject
{
    public List<SanitationModel> SanitationModels;
}

[System.Serializable]
public class SanitationModel
{
    public ToolName ToolName;
    public float DesanitationAmount;
}