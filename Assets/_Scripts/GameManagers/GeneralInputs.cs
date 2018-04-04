﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralInputs : MonoBehaviour 
{
    private LevelController LevelController;
    private GameController GC;
    private OptionMenuManager OptionMenuManager;
    private PlayerDataController DataCtrl = new PlayerDataController();

    private void Start() 
	{
        LevelController = new LevelController(FindObjectOfType<SceneLoader>());
        GC = FindObjectOfType<GameController>();
    }
	
	private void Update() 
	{
        if (Input.GetKeyDown(KeyCode.N))
        {
            LevelController.LoadNextLevel(FindObjectOfType<LevelManager>().LevelConfig.LevelNumber);
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (GameController.InOptionMenu)
            {
                OptionMenuManager = FindObjectOfType<OptionMenuManager>();
                OptionMenuManager.ButtonBack();
            }
            else if (!GameController.InPauseMenu)
            {
                GC.PauseGame(GC.PauseMenuPrefab);
            }
            else
            {
                GC.ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            DataCtrl.UnlockAllLevels();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            DataCtrl.ClearPrefs();
        }
    }
}
