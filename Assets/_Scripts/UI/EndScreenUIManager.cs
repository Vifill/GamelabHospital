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
    public GameObject LevelButtonsWin;
    public GameObject LevelButtonsFail;
    public float FillSpeed;
    public Animator StarAnimator;

    public void InitializeUI(StarConfig pStarConfig, int pSaved, int pScore, int pDead, bool pPassed)
    {
        print("INIT");
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
                LevelButtonsWin.SetActive(true);
                LevelButtonsFail.SetActive(false);
            }
            else
            {
                LevelButtonsWin.SetActive(false);
                LevelButtonsFail.SetActive(true);
            }
        }
        else
        {
            LevelButtonsWin.SetActive(false);
            LevelButtonsFail.SetActive(true);
        }

        
    }

    private IEnumerator ProgressBarFill(StarConfig pStarConfig, int pScore)
    {
        print("Start");
        //ProgressBar.fillAmount = CurrFillPoint;
        MaxLevelScore = (int)(pStarConfig.PointsForGold * 1.1f);
        float StarTimer = 0;
        float CurrentScore = 0;
        int targetStars = 0;
        if (pScore >= pStarConfig.PointsForBronze)
        {
            targetStars = 1;
        }
        if (pScore >= pStarConfig.PointsForSilver)
        {
            targetStars = 2;
        }
        if (pScore >= pStarConfig.PointsForGold)
        {
            targetStars = 3;
        }
        StarCount = 0;
        print(targetStars+ " Stars");
        while (true)
        {
            //CurrFillPoint = ProgressBar.fillAmount * MaxLevelScore;
            //ProgressBar.fillAmount += FillSpeed * Time.unscaledDeltaTime;
            CurrentScore = Mathf.MoveTowards(CurrentScore, pScore, Time.unscaledDeltaTime * Mathf.Clamp(MaxLevelScore/1.5f, 50, float.MaxValue));
            ScoreText.text = CurrentScore.ToString("0000");
            StarTimer += Time.unscaledDeltaTime;
            if (StarTimer > .5f && StarCount == 0 && pScore >= pStarConfig.PointsForBronze)
            {
                GiveOneStar();
                StarCount += 1;
                StarTimer = 0;
            }
            if (pScore >= pStarConfig.PointsForSilver && StarCount == 1 && StarTimer > .5f)
            {
                StarTimer = 0;
                GiveTwoStars();
                StarCount += 1;
            }
            if (pScore >= pStarConfig.PointsForGold && StarCount == 2 && StarTimer > .5f)
            {
                GiveThreeStars();
                StarCount += 1;
            }

            if (CurrentScore >= pScore && targetStars == StarCount)
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
