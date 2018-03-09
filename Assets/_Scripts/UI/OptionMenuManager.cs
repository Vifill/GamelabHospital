using System;
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
    private bool Fullscreen;
    private int ResolutionIndex;
    private int QualityIndex;

	public void Initialize(GameObject pPausemenu)
    {
        PauseMenu = pPausemenu;
        PauseMenu.SetActive(false);
        GetResolutions();
        GetGraphicQualityIndexes();
    }

    private void GetGraphicQualityIndexes()
    {
        QualityIndex = QualitySettings.GetQualityLevel();
        Fullscreen = Screen.fullScreen;
    }

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
                ResolutionIndex = i;
                options.RemoveAt(currentResolutionIndex);
            }
        }

        ResolutionDropdown.AddOptions(options);

        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int pResolutionIndex)
    {
        ResolutionIndex = pResolutionIndex;
        //Resolution resolution = Resolutions[pResolutionIndex];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool pIsFullscreen)
    {
        Screen.fullScreen = pIsFullscreen;
    }

    public void SetQuality(int pQualityIndex)
    {
        QualityIndex = pQualityIndex;
        //QualitySettings.SetQualityLevel(pQualityIndex);
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

    public void ApplyButton()
    {
        Resolution resolution = Resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        QualitySettings.SetQualityLevel(QualityIndex);
    }

    public void ButtonBack()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        GameController.InOptionMenu = false;
        PauseMenu.SetActive(true);
        Destroy(gameObject);
    }
}
