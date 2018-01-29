using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsUIManager : MonoBehaviour 
{
    public Color ScoreTextColor;
    private Text DisplayedScoreText;
    private int DisplayedScore;
    private int Score;
    private Animator ScoreAnimator;

    public void Initialize()
    {
        Score = 0;
        ScoreAnimator = GetComponent<Animator>();
        DisplayedScoreText = transform.GetComponentInChildren<Text>();
        DisplayedScoreText.text = "0";
        DisplayedScoreText.color = ScoreTextColor;
    }

    public IEnumerator UpdateUI(int pPoints)
    {
        ScoreAnimator.SetBool("GivingPoints", true);
        Score += pPoints;
        float t = 0;
        if (Score < 0)
        {
            Score = 0;
        }
        while (DisplayedScore != Score)
        {
            DisplayedScore = (int)Mathf.Lerp(DisplayedScore, Score, t);
            t += Time.deltaTime;
            DisplayedScoreText.text = DisplayedScore.ToString();
            yield return null;
        }
        ScoreAnimator.SetBool("GivingPoints", false);
    }

}
