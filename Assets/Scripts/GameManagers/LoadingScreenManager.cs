using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    private AsyncOperation Async;

    public static void LoadSceneId(int pSceneId)
    {
        
    }

    public void Initialize(string pSceneToLoad)
    {
         Async = SceneManager.LoadSceneAsync(pSceneToLoad);
    }

}
