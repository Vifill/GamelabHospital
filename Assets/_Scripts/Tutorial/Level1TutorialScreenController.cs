using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TutorialScreenController : MonoBehaviour 
{
    private bool CheckoutHasBeenShown = false;
    private bool HydrationHasBeenShown = false;

    private InfoScreenController InfoScreenController;
    public GameObject HydrationScreenPrefab;
    public GameObject CheckoutScreenPrefab;

    private void Start()
    {
        InfoScreenController = GetComponent<InfoScreenController>();
    }

    public void DisplayCheckoutScreen()
    {
        if(!CheckoutHasBeenShown)
        {
            CheckoutHasBeenShown = true;
            InfoScreenController.InstantiateInfoScreen(CheckoutScreenPrefab);
        }
    }

    public void DisplayHydrationScreen()
    {
        if (!HydrationHasBeenShown)
        {
            HydrationHasBeenShown = true;
            InfoScreenController.InstantiateInfoScreen(HydrationScreenPrefab);
        }
    }
}
