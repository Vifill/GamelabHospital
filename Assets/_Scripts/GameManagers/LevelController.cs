﻿using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController
{
    private SceneLoader SceneLoader;
    public LevelController(SceneLoader pSceneLoader)
    {
        SceneLoader = pSceneLoader;
    }

	public void LoadNextLevel(int pCurrentLevel)
    {
        SceneLoader.LoadScene($"{Constants.SceneNamePrefix}{pCurrentLevel+1}");
        //GameController.InMenuScreen = false;
    }

    public void GoToLevelSelection()
    {
        SceneLoader.LoadScene(Constants.LevelSelect);
    }

    public void GoToMainMenu()
    {
        SceneLoader.LoadScene(Constants.MainMenu);
    }

    public void RestartCurrentScene()
    {
        SceneLoader.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //GameController.InMenuScreen = false;
    }
    
    public void LoadMainMenu()
    {
        SceneLoader.LoadScene("MainMenu");
    }
}
