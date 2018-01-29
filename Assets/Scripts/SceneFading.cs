using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFading : MonoBehaviour 
{
    public Texture2D FadeOutTexture;
    public float FadeSpeed;

    private int drawDepth = -1000;
    private float Alpha = 1;
    private float FadeDir = -1; // Direction to fade in = -1 or out = 1
    private bool DoneFading = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FadeInOnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= FadeInOnSceneLoad;
    }

    private void FadeInOnSceneLoad(Scene pScene, LoadSceneMode pLoadSceneMode) // Fades in when scene is loaded
    {
        BeginFade(-1, 0.2f);
    }

    private void OnGUI()
    {
        if (!DoneFading)
        {
            Alpha += FadeDir * FadeSpeed * Time.unscaledDeltaTime;
            Alpha = Mathf.Clamp01(Alpha);

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Alpha);
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeOutTexture);
            if (FadeDir == -1 && Alpha == 0)
            {
                DoneFading = true;
            }
            if (FadeDir == 1 && Alpha == 1)
            {
                //DoneFading = true;
            }
        }
    }

    public float BeginFade(float pFadeDirection, float pFadeTime) // Direction to fade in = -1 or out = 1
    {
        Debug.Log($"Starting fade in {pFadeDirection} direction, and alpha is: {Alpha}");
        FadeSpeed = 1 / pFadeTime;
        FadeDir = pFadeDirection;
        DoneFading = false;
        return FadeSpeed;
    }    
}
