using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanitationController : MonoBehaviour
{
    private GameObject SanitationBar;
    private SanitationUI SanitationUI;
    private Transform MainCanvas;

    public float CurrentSanitationLevel;
    public float MaxSanitationLevel;

    public GameObject SanitationBarUIPrefab;
    public string UIPosition;
    public Sprite SanitationUIPicture;
    public Color SanitationUIColor;

	// Use this for initialization
	private void Start() 
	{
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        InitializeSanitationUI();
	}
	
	// Update is called once per frame
	private void Update() 
	{

    }

    private void InitializeSanitationUI()
    {
        SanitationBar = Instantiate(SanitationBarUIPrefab, MainCanvas);
        SanitationUI = SanitationBar.GetComponent<SanitationUI>();
        SanitationUI.Initialize(SanitationUIPicture, SanitationUIColor, this, UIPosition);
    }

    public void MakePlayerDirty(float pDirt)
    {
        CurrentSanitationLevel += pDirt;
        SanitationUI.UpdateSanitationUI();
    }

    public void ClearSanitation()
    {
        CurrentSanitationLevel = 0;
        SanitationUI.UpdateSanitationUI();
    }

}
