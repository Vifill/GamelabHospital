﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    static public bool InMenuScreen;
    static public bool InPauseMenu;
    static public bool InOptionMenu;
    static public bool OrderlyInScene { get; private set; }
    static public List<OrderlyController> GetOrderliesInScene { get; private set; }
    public BedManager BedManager;
    public GameObject PauseMenuPrefab;
    public GameObject PauseGameButton;
    private Transform MainCanvas;
    private GameObject CurrentUIScreen;

    public DoctorSanitationConfig DoctorSanitationConfig;

	// Use this for initialization
	private void Start () 
	{
        InMenuScreen = false;
        InPauseMenu = false;
        Datalogic dl = new Datalogic();
        dl.Initialize();
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        BedManager.InitializeBeds();
        PauseGameButton = GameObject.FindGameObjectWithTag("PauseButton");
        if (FindObjectOfType<OrderlyController>() != null)
        {
            OrderlyInScene = true;
            GetOrderliesInScene = FindObjectsOfType<OrderlyController>().ToList();
        }
        else
        {
            OrderlyInScene = false;
        }
	}

    public bool ShouldSpawnBucketUI()
    {
        return DoctorSanitationConfig.SanitationModels.Any(a => a.ToolName == ToolName.Bucket && a.DesanitationAmount > 0);
    }

    public bool ShouldSpawnSanitationUI()
    {
        return DoctorSanitationConfig.SanitationModels.Any(a => a.ToolName == ToolName.Water && a.DesanitationAmount > 0);
    }

    public void PauseGame()
    {
        PauseGame(PauseMenuPrefab);
    }

    public void PauseGame(GameObject pUIScreen)
    {
        if (PauseGameButton != null)
        {
            PauseGameButton.SetActive(false);
        }
        if(pUIScreen == null)
        {
            return;
        }
        if (CurrentUIScreen != null)
        {
            Destroy(CurrentUIScreen);
        }
        
        Time.timeScale = 0;
        InMenuScreen = true;
        InPauseMenu = true;
        GetComponent<MouseCursorController>().SetCursorToIdle();
        CurrentUIScreen = UISpawner.SpawnUIWithNoPos(pUIScreen, UIHierarchy.UIScreens);

        CurrentUIScreen.GetComponent<PauseMenuManager>()?.Initialize(this);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Destroy(CurrentUIScreen);
        InPauseMenu = false;
        InMenuScreen = false;
        if (PauseGameButton != null)
        {
            PauseGameButton.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
