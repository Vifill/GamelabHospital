using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    private Transform Canvas;
    private SceneFading Fader;
    //private GameObject LoadingScreenObject;
    private bool LoadingScreenReady = false;

    private void Start()
    {
        Fader = FindObjectOfType<SceneFading>();
        Canvas = FindObjectOfType<Canvas>().transform;
    }

    private IEnumerator LoadSceneCoroutine(string asyncOperation, bool pWithLoadingScreen)
    {
        //float fadeTime = Fader.BeginFade(1, 0.15f);
        if (pWithLoadingScreen)
        {
            LoadingScreenReady = false;
            StartCoroutine(InstantiateLoadinScreen());
        }
        else
        {
            LoadingScreenReady = true;
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(asyncOperation);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            Debug.Log(async.progress);
            if(async.progress >= 0.89 && LoadingScreenReady)
            {
                Fader.BeginFade(1, 0.15f);
                yield return new WaitForSecondsRealtime(0.1f);
                async.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
        
    }

    private IEnumerator InstantiateLoadinScreen()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        //LoadingScreenObject = Instantiate(LoadingScreen, Canvas);
        Instantiate(LoadingScreen, Canvas);
        Fader.BeginFade(-1, 0.15f);
        yield return new WaitForSecondsRealtime(0.1f);
        LoadingScreenReady = true;
    }
    

    public void LoadScene(string pLevelName, bool pWithLoadingScreen = true)
    {
        StartCoroutine(LoadSceneCoroutine(pLevelName, pWithLoadingScreen));
        //Instantiate(LoadingScreen, Canvas);
    }

    //public IEnumerator LoadScene(int pLevelId)
    //{
    //    AsyncOperation async = SceneManager.LoadSceneAsync(pLevelId);
    //    //yield return StartCoroutine(LoadSceneCoroutine(async));
    //}

}
