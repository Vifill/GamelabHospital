using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanitationController : MonoBehaviour
{

    private Image SanitationProgressBar;
    public float CurrentSanitationLevel;
    public float MaxSanitationLevel;

    public GameObject SanitationBarPrefab;

	// Use this for initialization
	private void Start() 
	{
        SanitationProgressBar = SanitationBarPrefab.transform.GetChild(0).GetComponent<Image>();
	}
	
	// Update is called once per frame
	private void Update() 
	{
        SanitationProgressBar.fillAmount = CurrentSanitationLevel / MaxSanitationLevel;
	}
}
