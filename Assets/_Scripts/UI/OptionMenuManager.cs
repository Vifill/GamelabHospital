using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenuManager : MonoBehaviour 
{
    public AudioMixer AudioMixer;
    public Dropdown ResolutionDropdown;

    private GameObject PauseMenu;
    private Resolution[] Resolutions;

	public void Initialize(GameObject pPausemenu)
    {
        PauseMenu = pPausemenu;
        PauseMenu.SetActive(false);
        GetResolutions();
    }

    //private void Start()
    //{
    //    GetResolutions();
    //}

    private void GetResolutions()
    {
        Resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        var currentResolutionIndex = 0;

        for (int i = 0; i < Resolutions.Length; i++)
        {
            string option = Resolutions[i].width + " x " + Resolutions[i].height;
            options.Add(option);

            if (Resolutions[i].width == Screen.currentResolution.width &&
                Resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);

        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int pResolutionIndex)
    {
        Resolution resolution = Resolutions[pResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool pIsFullscreen)
    {
        Screen.fullScreen = pIsFullscreen;
    }

    public void SetQuality(int pQualityIndex)
    {
        QualitySettings.SetQualityLevel(pQualityIndex);
    }

    public void SetMasterVolume(float pVolume)
    {
        AudioMixer.SetFloat("MasterVolume", pVolume);
    }

    public void SetMusicVolume(float pVolume)
    {
        AudioMixer.SetFloat("MusicVolume", pVolume);
    }

    public void SetSfxVolume(float pVolume)
    {
        AudioMixer.SetFloat("SfxVolume", pVolume);
    }


    public void ButtonBack()
    {
        PauseMenu.SetActive(true);
        Destroy(gameObject);
    }
}
