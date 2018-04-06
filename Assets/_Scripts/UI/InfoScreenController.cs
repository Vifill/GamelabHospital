using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScreenController : MonoBehaviour 
{
    private GameController GC;

    public GameObject InfoScreenPrefab;
    public float SecondsBeforeScreenAppears;
    // Use this for initialization
    private void Start ()
	{
        GC = FindObjectOfType<GameController>();
        //GC.PauseGame(InfoScreenPrefab);

        //StartCoroutine(InstantiateScreen());
        Invoke("InstantiateScreen", SecondsBeforeScreenAppears);
	}

    internal void InstantiateInfoScreen(GameObject pCheckoutScreenPrefab)
    {
        GC.PauseGame(pCheckoutScreenPrefab);
    }

    // Instantiates the screen
    //private IEnumerator InstantiateScreen()
    //{
    //    yield return new WaitForSeconds(SecondsBeforeScreenAppears);

    //    GC.PauseGame(InfoScreenPrefab);
    //}

    private void InstantiateScreen()
    {
        GC.PauseGame(InfoScreenPrefab);
    }

    public void CloseButton()
    {
        GC.ResumeGame();
    }
}
