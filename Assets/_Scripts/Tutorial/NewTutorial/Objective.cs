using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Objective")]
public class Objective : ScriptableObject
{
    public string ObjectiveDescription;
    public EventManager.EventCodes OnFinishEvent;
}
