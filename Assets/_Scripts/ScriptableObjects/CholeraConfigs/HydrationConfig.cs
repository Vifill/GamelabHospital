using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Configs/Patient/HydrationConfig")]
public class HydrationConfig : ScriptableObject
{
    public AnimationCurve HydrationLossModifier;
}
