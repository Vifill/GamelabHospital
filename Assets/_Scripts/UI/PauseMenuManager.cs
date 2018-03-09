using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour 
{
    public GameObject OptionsMenuPrefab;
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

    public void ButtonOptions()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        var optionMenu = Instantiate(OptionsMenuPrefab, transform.parent);
        GameController.InOptionMenu = true;
        optionMenu.GetComponent<OptionMenuManager>().Initialize(gameObject);
    }
}
