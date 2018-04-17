using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour 
{
    public GameObject OptionsMenuPrefab;
    public GameObject ResumeButton;
    private GameController GC;
    private LevelController LvlCtrl;

    public void Initialize(GameController pGC)
    {
        LvlCtrl =new LevelController(FindObjectOfType<SceneLoader>());
        SetSelectedButton();
        GC = pGC;
    }

    public void SetSelectedButton()
    {
        var CanvasEventSystem = FindObjectOfType<EventSystem>();
        CanvasEventSystem.SetSelectedGameObject(ResumeButton);
    }

    public void ButtonResume()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        GC.ResumeGame();
    }

    public void ButtonMenu()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        //LvlCtrl.GoToLevelSelection();
        LvlCtrl.GoToMainMenu();
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
