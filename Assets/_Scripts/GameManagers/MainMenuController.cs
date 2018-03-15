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
        SceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void Play()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        //SceneLoader.LoadScene("LevelSelection");
        SceneLoader.LoadScene(Constants.SceneNamePrefix+"1");
    }

    public void Exit()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        Application.Quit();
    }

}
