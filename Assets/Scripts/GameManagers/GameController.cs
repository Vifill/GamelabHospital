﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    static public bool InMenuScreen;
    static public bool InPauseMenu;
    static public bool OrderlyInScene { get; private set; }
    public BedManager BedManager;
    public GameObject PauseMenuPrefab;
    private Transform MainCanvas;
    private GameObject CurrentUIScreen;

	// Use this for initialization
	private void Start () 
	{
        InMenuScreen = false;
        InPauseMenu = false;
        Datalogic dl = new Datalogic();
        dl.Initialize();
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        BedManager.InitializeBeds();

        if (FindObjectOfType<OrderlyController>() != null)
        {
            OrderlyInScene = true;
        }
        else
        {
            OrderlyInScene = false;
        }
	}

    public void PauseGame(GameObject pUIScreen)
    {
        if (CurrentUIScreen != null)
        {
            Destroy(CurrentUIScreen);
        }

        Time.timeScale = 0;
        InMenuScreen = true;
        InPauseMenu = true;
        CurrentUIScreen = Instantiate(pUIScreen, MainCanvas);
        CurrentUIScreen.GetComponent<PauseMenuManager>()?.Initialize(this);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Destroy(CurrentUIScreen);
        InPauseMenu = false;
        InMenuScreen = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
