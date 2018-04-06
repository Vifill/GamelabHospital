using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3TutorialScreenController : MonoBehaviour 
{
    private bool HasBeenShown = false;

    private InfoScreenController InfoScreenController;
    public GameObject BucketDirtyScreen;

    private void Start()
    {
        InfoScreenController = GetComponent<InfoScreenController>();
    }

    public void DisplayBucketDirtyScreen()
    {
        if (!HasBeenShown)
        {
            HasBeenShown = true;
            InfoScreenController.InstantiateInfoScreen(BucketDirtyScreen);
        }
    }
}
