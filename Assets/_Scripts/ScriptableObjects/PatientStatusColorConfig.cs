using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Patient/PatientStatusColorConfig")]
public class PatientStatusColorConfig : ScriptableObject 
{
    public Color StatusGreen;
    public Color StatusYellow;
    public Color StatusRed;
}
