using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TutorialScreenController : MonoBehaviour 
{
    private bool HasBeenShown = false;

    private InfoScreenController InfoScreenController;
    public GameObject CheckoutScreenPrefab;

    private void Start()
    {
        InfoScreenController = GetComponent<InfoScreenController>();
    }

    public void DisplayCheckoutScreen()
    {
        if(!HasBeenShown)
        {
            HasBeenShown = true;
            InfoScreenController.InstantiateInfoScreen(CheckoutScreenPrefab);
        }
    }
}
