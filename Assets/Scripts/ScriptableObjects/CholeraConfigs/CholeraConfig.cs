using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Configs/Patient/CholeraConfig")]
public class CholeraConfig : ScriptableObject
{
    public float CholeraCheckRate;
    public float ExcreteCooldown;
    public float ExcreteHydrationLoss;
    public float ExcreteHydrationLossVariance;
}
