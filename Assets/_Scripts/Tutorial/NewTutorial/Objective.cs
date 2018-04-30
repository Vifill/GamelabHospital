using Assets._Scripts.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Objective")]
public class Objective : ScriptableObject
{
    public string ObjectiveDescription;
    public EventManager.EventCodes OnFinishEvent;

    private List<Transform> ArrowPositions;

    public List<Transform> GetArrowPositions()
    {
        string identifier = GetObjectiveIdentifier();
        
        return GameObject.FindGameObjectsWithTag(Constants.ArrowPlacementTag).
            Where(a => a.name.StartsWith(identifier)).
            Select(a => a.transform).ToList();
    }

    private string GetObjectiveIdentifier()
    {
        return name.Split(' ')[0];
    }
}
