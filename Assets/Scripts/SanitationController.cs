using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanitationController : MonoBehaviour
{
    private GameObject SanitationBar;
    private Image SanitationProgressBar;
    private Transform MainCanvas;
    public float CurrentSanitationLevel;
    public float MaxSanitationLevel;

    public GameObject SanitationBarPrefab;
    public Transform ProgressBarWorldPosition;
    public float UIOffset;

	// Use this for initialization
	private void Start() 
	{
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        SanitationBar = Instantiate(SanitationBarPrefab, MainCanvas);
        SanitationProgressBar = SanitationBar.transform.GetChild(0).GetComponent<Image>();
	}
	
	// Update is called once per frame
	private void Update() 
	{
        SanitationProgressBar.fillAmount = CurrentSanitationLevel / MaxSanitationLevel;
        SanitationBar.transform.position = Camera.main.WorldToScreenPoint(ProgressBarWorldPosition.position) + new Vector3(0, UIOffset, 0);
        if (CurrentSanitationLevel >= MaxSanitationLevel)
        {
            CurrentSanitationLevel = MaxSanitationLevel;
        }
    }

    public void MakePlayerDirty(float pDirt)
    {
        CurrentSanitationLevel += pDirt;
    }

}
