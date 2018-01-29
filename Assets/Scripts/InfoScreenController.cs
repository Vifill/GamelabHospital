using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScreenController : MonoBehaviour 
{
    private GameController GC;

    public GameObject InfoScreenPrefab;

    // Use this for initialization
    private void Start () 
	{
        GC = FindObjectOfType<GameController>();
        //GC.PauseGame(InfoScreenPrefab);

        StartCoroutine(InstantiateScreen());
	}

    private IEnumerator InstantiateScreen()
    {
        yield return new WaitForSeconds(1);

        GC.PauseGame(InfoScreenPrefab);
    }

    public void CloseButton()
    {
        GC.ResumeGame();
    }
}
