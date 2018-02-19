using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour 
{
    private GameController GC;
    private LevelController LvlCtrl;

    public void Initialize(GameController pGC)
    {
        LvlCtrl =new LevelController(FindObjectOfType<SceneLoader>());
        GC = pGC;
    }

    public void ButtonResume()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        GC.ResumeGame();
    }

    public void ButtonMenu()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        LvlCtrl.GoToLevelSelection();
    }

    public void ButtonRestart()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        LvlCtrl.RestartCurrentScene();
    }
}
