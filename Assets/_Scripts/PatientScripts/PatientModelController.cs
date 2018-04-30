using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientModelController : MonoBehaviour 
{
    public PatientModelsConfig ModelsConfig;

	// Use this for initialization
	private void Start () 
	{
        var modelPrefabToUse = ModelsConfig.PatientModels[Random.Range(0, ModelsConfig.PatientModels.Count)];
        Instantiate(modelPrefabToUse, transform.Find(Constants.Highlightable).transform);
	}
}
