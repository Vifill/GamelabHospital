using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController
{
    private SceneLoader SceneLoader = new SceneLoader();

    public LevelController(SceneLoader pSceneLoader)
    {
        SceneLoader = pSceneLoader;
    }

	public void LoadNextLevel(int pCurrentLevel)
    {
        SceneLoader.LoadScene($"Level{pCurrentLevel+1}");
        //GameController.InMenuScreen = false;
    }

    public void GoToLevelSelection()
    {
        SceneLoader.LoadScene("LevelSelection");
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
