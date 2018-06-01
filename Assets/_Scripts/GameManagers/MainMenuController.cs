using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour 
{
    private SceneLoader SceneLoader;

    private void Start()
    {
        Time.timeScale = 1;
        SceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void Play()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        //SceneLoader.LoadScene("LevelSelection");
        //SceneLoader.LoadScene(Constants.SceneNamePrefix+"1");
        SceneLoader.LoadScene("LevelSelect");
    }

    public void Exit()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        Application.Quit();
    }

}
