using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2TutorialScreenController : MonoBehaviour 
{
    private bool HasBeenShown = false;

    private InfoScreenController InfoScreenController;
    public GameObject PlayerDirtyScreen;

    private void Start()
    {
        InfoScreenController = GetComponent<InfoScreenController>();
    }

    public void DisplayDirtyPlayerScreen()
    {
        if (!HasBeenShown)
        {
            HasBeenShown = true;
            InfoScreenController.InstantiateInfoScreen(PlayerDirtyScreen);
        }
    }

}
