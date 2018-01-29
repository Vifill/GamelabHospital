using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour 
{
    public AudioSource AudioSource;
    public AudioSource ButtonSource;
    public AudioClip MenuMusic;
    public AudioClip LevelMusic;
    public AudioClip ButtonSound;

    private static MusicController MusicControllerInstance;
    public static MusicController Instance
    {
        get { return MusicControllerInstance; }
    }

    private string CurrentSceneName;

    private void Awake()
    {
        if (MusicControllerInstance != null && MusicControllerInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            MusicControllerInstance = this;
            CurrentSceneName = SceneManager.GetActiveScene().name;
            if (CurrentSceneName.ToLower().Contains("level") && CurrentSceneName.ToLower() != "levelselection")
            {
                AudioSource.clip = LevelMusic;
                AudioSource.Play();
            }
            else
            {
                AudioSource.clip = MenuMusic;
                AudioSource.Play();
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene pScene, LoadSceneMode pMode)
    {
        if (CurrentSceneName.ToLower().Contains("level") && CurrentSceneName.ToLower() != "levelselection" && pScene.name.ToLower().Contains("levelselection"))
        {
            AudioSource.clip = MenuMusic;
            AudioSource.Play();
        }
        else if (CurrentSceneName.ToLower() == "levelselection" && pScene.name.ToLower().Contains("level"))
        {
            AudioSource.clip = LevelMusic;
            AudioSource.Play();
        }

        CurrentSceneName = SceneManager.GetActiveScene().name;      
    }

    public void PlayButtonSound()
    {
        ButtonSource.PlayOneShot(ButtonSound);
    }
}
