using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
    static public bool InMenuScreen;
    static public bool InPauseMenu;
    static public bool InOptionMenu;
    static public bool InShiftOver;
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
        InShiftOver = false;
        InMenuScreen = false;
        InPauseMenu = false;
        Datalogic dl = new Datalogic();
        dl.Initialize();
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        BedManager.InitializeBeds();
        PauseGameButton = GameObject.FindGameObjectWithTag("PauseButton");
        if (PauseGameButton != null)
        {
            PauseGameButton.GetComponent<Button>().onClick.AddListener(PauseGame);
            StartCoroutine(DelayedSpawnButton());
        }

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

    private IEnumerator DelayedSpawnButton()
    {
        yield return new WaitForSeconds(.1f);
        if (PauseGameButton != null)
        {
            PauseGameButton.SetActive(true);
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
            PauseGameButton.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
