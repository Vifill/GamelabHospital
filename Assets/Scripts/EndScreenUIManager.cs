using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class EndScreenUIManager : MonoBehaviour 
{
    private SceneLoader SceneLoader;
    private LevelController LvlCtrl;
    private int MaxLevelScore;
    private int StarCount = 0;
    private float CurrFillPoint = 0;
    private float DisplayedScore;

    public Text SavedText;
    public Text LostText;
    public Text ScoreText;
    public Sprite YellowStar;
    public Sprite GrayStar;
    public Image Star1;
    public Image Star2;
    public Image Star3;
    public Image ProgressBar;
    public Button LevelSelectionBtnObj;
    public Button StaticLvlSelectBtnObj;
    public Button RestartLevelBtnObj;
    public Button NextLvlBtnObj;
    public float FillSpeed;
    public Animator StarAnimator;

    public void InitializeUI(StarConfig pStarConfig, int pSaved, int pScore, int pDead, bool pPassed)
    {
        GameController.InMenuScreen = true;
        SceneLoader = FindObjectOfType<SceneLoader>();
        LvlCtrl = new LevelController(SceneLoader);
        SavedText.text = pSaved.ToString();
        LostText.text = pDead.ToString();
        StarAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        StartCoroutine(ProgressBarFill(pStarConfig, pScore));

        if (pPassed)
        {

            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                NextLvlBtnObj.gameObject.SetActive(true);
                LevelSelectionBtnObj.gameObject.SetActive(false);
                RestartLevelBtnObj.gameObject.SetActive(false);
                StaticLvlSelectBtnObj.gameObject.SetActive(true);
            }
            else
            {
                NextLvlBtnObj.gameObject.SetActive(false);
                LevelSelectionBtnObj.gameObject.SetActive(true);
                RestartLevelBtnObj.gameObject.SetActive(false);
                StaticLvlSelectBtnObj.gameObject.SetActive(false);
            }
        }
        else
        {
            RestartLevelBtnObj.gameObject.SetActive(true);
            NextLvlBtnObj.gameObject.SetActive(false);
            LevelSelectionBtnObj.gameObject.SetActive(false);
            StaticLvlSelectBtnObj.gameObject.SetActive(true);
        }

        
    }

    private IEnumerator ProgressBarFill(StarConfig pStarConfig, int pScore)
    {
        ProgressBar.fillAmount = CurrFillPoint;
        MaxLevelScore = (int)(pStarConfig.PointsForGold * 1.1f);
        while (true)
        {
            CurrFillPoint = ProgressBar.fillAmount * MaxLevelScore;
            ProgressBar.fillAmount += FillSpeed * Time.unscaledDeltaTime;
            ScoreText.text = CurrFillPoint.ToString("0");

            if (CurrFillPoint >= pStarConfig.PointsForBronze && StarCount == 0)
            {
                GiveOneStar();
                StarCount += 1;
            }
            if (CurrFillPoint >= pStarConfig.PointsForSilver && StarCount == 1)
            {
                GiveTwoStars();
                StarCount += 1;
            }
            if (CurrFillPoint >= pStarConfig.PointsForGold && StarCount == 2)
            {
                GiveThreeStars();
                StarCount += 1;
            }

            if (CurrFillPoint >= MaxLevelScore || CurrFillPoint >= pScore)
            {
                ScoreText.text = pScore.ToString();
                break;
            }



            yield return null;
        }
    }

    public void GiveOneStar()
    {
        Star1.sprite = YellowStar;
        Star2.sprite = GrayStar;
        Star3.sprite = GrayStar;
        // Add animation/particle effect/sounds
        StarAnimator.SetTrigger("Star1");
    }

    public void GiveTwoStars()
    {
        Star1.sprite = YellowStar;
        Star2.sprite = YellowStar;
        Star3.sprite = GrayStar;
        // Add animation/particle effect/sounds
        StarAnimator.SetTrigger("Star2");
    }

    public void GiveThreeStars()
    {
        Star1.sprite = YellowStar;
        Star2.sprite = YellowStar;
        Star3.sprite = YellowStar;
        // Add animation/particle effect/sounds
        StarAnimator.SetTrigger("Star3");
    }
    
    public void RestartLevelButton()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        LvlCtrl.RestartCurrentScene();
        GameController.InMenuScreen = false;
    }

    public void NextLevelButton()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        int currentLevelNumber = FindObjectOfType<LevelManager>().LevelConfig.LevelNumber;
        LvlCtrl.LoadNextLevel(currentLevelNumber);
        GameController.InMenuScreen = false;
    }

    public void LevelSelectionBtn()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        LvlCtrl.GoToLevelSelection();
        GameController.InMenuScreen = false;
    }
}
