using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Assets._Scripts.Utilities;

public class PointsUIManager : MonoBehaviour 
{
    public GameObject ScoreParticleSystemPrefab;
    public GameObject PopupScoreTextPrefabGreen;
    public GameObject PopupScoreTextPrefabRed;    
    public GameObject StarPlacementPrefab;
    public Color ScoreTextColor;
    public Text DisplayedScoreText;
    public Image BarFill;
    public float PointsLerpSpeed = 50;

    private int DisplayedScore;
    private int Score;
    private Animator ScoreAnimator;
    private float MaxBarFill;
    private float FillUnit;
    private LevelConfig LevelCfg;
    private List<Tuple<StarPlacementManager, float>> Stars = new List<Tuple<StarPlacementManager, float>>();
    private Coroutine CurrentLerpToTotalPointsCoroutine;

    public void Initialize(LevelConfig pLevelCfg)
    {
        Score = 0;
        ScoreAnimator = GetComponent<Animator>();
        //DisplayedScoreText = transform.GetComponentInChildren<Text>();
        DisplayedScoreText.text = "0";
        DisplayedScoreText.color = ScoreTextColor;
        LevelCfg = pLevelCfg;
        MaxBarFill = LevelCfg.StarConfig.PointsForGold * 1.1f;
        FillUnit = BarFill.rectTransform.rect.width / MaxBarFill;
        BarFill.fillAmount = 0;
        PlaceStars();
    }

    private void PlaceStars()
    {
        var firstStar = Instantiate(StarPlacementPrefab, BarFill.transform);
        firstStar.GetComponent<RectTransform>().anchoredPosition = new Vector2(FillUnit * LevelCfg.StarConfig.PointsForBronze, 0);
        Stars.Add(new Tuple<StarPlacementManager, float>(firstStar.GetComponent<StarPlacementManager>(), LevelCfg.StarConfig.PointsForBronze));
        
        var secondStar = Instantiate(StarPlacementPrefab, BarFill.transform);
        secondStar.GetComponent<RectTransform>().anchoredPosition = new Vector2(FillUnit * LevelCfg.StarConfig.PointsForSilver, 0);
        Stars.Add(new Tuple<StarPlacementManager, float>(secondStar.GetComponent<StarPlacementManager>(), LevelCfg.StarConfig.PointsForSilver));

        var thirdStar = Instantiate(StarPlacementPrefab, BarFill.transform);
        thirdStar.GetComponent<RectTransform>().anchoredPosition = new Vector2(FillUnit * LevelCfg.StarConfig.PointsForGold, 0);
        Stars.Add(new Tuple<StarPlacementManager, float>(thirdStar.GetComponent<StarPlacementManager>(), LevelCfg.StarConfig.PointsForGold));
    }

    Vector2 WorldToCanvas(Vector3 pVector)
    {
        RectTransform CanvasRect = DisplayedScoreText.transform.parent.parent.GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(pVector);
        return new Vector2(((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                           ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
    }
       
    public IEnumerator UpdateUI(int pPoints, Vector3 pPosition)
    {
        GameObject GO = UISpawner.SpawnUIFromWorldPosition(pPoints > 0 ? PopupScoreTextPrefabGreen : PopupScoreTextPrefabRed, pPosition, UIHierarchy.LerpingPoints);
        RectTransform GORect = GO.GetComponent<RectTransform>();
        Text tempText = GO.GetComponentInChildren<Text>();
        GORect.anchoredPosition = WorldToCanvas(pPosition);
        Destroy(GO, 10);
        int TempScore = 0;

        Score += pPoints;
        float t = 0;
        if (Score < 0)
        {
            Score = 0;
        }
        while (TempScore != pPoints)
        {
            TempScore = (int)Mathf.Lerp(0, pPoints, t);
            t += Time.unscaledDeltaTime/0.5f;
            tempText.text = TempScore.ToString();
            yield return null;
        }
        StartCoroutine(CombinePoints(TempScore, tempText, GO));
    }
    

    private IEnumerator CombinePoints(int pPoints, Text pText, GameObject pGO)
    {
        Vector3 secondPoint = new Vector3(pGO.transform.position.x, DisplayedScoreText.transform.position.y, 0);

        Vector3[] controlPoints = { pText.transform.position, secondPoint, DisplayedScoreText.transform.position };
        List<Vector3> BezierCurve = GetBezierApproximation(controlPoints, 20);
        while ((pGO.transform.position - DisplayedScoreText.transform.position).sqrMagnitude > 1)
        {
            pGO.transform.position = Vector3.Lerp(pGO.transform.position, BezierCurve[0], Time.unscaledDeltaTime * Screen.height * 1.5f);
            if ((pGO.transform.position - BezierCurve[0]).sqrMagnitude < 2)
            {
                BezierCurve.Remove(BezierCurve[0]);
            }

            if ((pGO.transform.position - DisplayedScoreText.transform.position).sqrMagnitude < 1000)
            {
                ScoreAnimator.SetBool(Constants.AnimationParameters.GivingPoints, true);
            }

            if (BezierCurve.Count <= 0)
            {
                break;
            }
            yield return null;
        }
        DisplayedScore += pPoints;
        if(DisplayedScore < 0)
        {
            DisplayedScore = 0;
        }
        
        if (CurrentLerpToTotalPointsCoroutine == null)
        {
            CurrentLerpToTotalPointsCoroutine = StartCoroutine(LerpToNewTotalPoints());
        }
       
        ScoreAnimator.SetBool(Constants.AnimationParameters.GivingPoints, false);
        Destroy(pText.gameObject);
        if (pGO?.GetComponent<ParticleSystem>() != null)
        {
            ParticleSystem.EmissionModule emmision = pGO.GetComponent<ParticleSystem>().emission;
            emmision.enabled = false;
            Destroy(pGO, 2);
        }

        if (ScoreParticleSystemPrefab != null)
        {
            GameObject GO = Instantiate(ScoreParticleSystemPrefab, Vector3.zero, Quaternion.identity, DisplayedScoreText.transform);
            Destroy(GO, 5);
        }
        
        yield return null;
    }

    private IEnumerator LerpToNewTotalPoints()
    {
        float tempPoints = Convert.ToInt32(DisplayedScoreText.text);
        while (tempPoints != DisplayedScore)
        {
            tempPoints = Mathf.Lerp(tempPoints, DisplayedScore, Time.deltaTime * PointsLerpSpeed);
            if(tempPoints < 0)
            {
                tempPoints = 0;
            }
            DisplayedScoreText.text = tempPoints.ToString("F0");
            BarFill.fillAmount = 1 / MaxBarFill * tempPoints;

            Stars.FirstOrDefault(a => Mathf.RoundToInt(tempPoints) >= a.Item2 && !a.Item1.Filled)?.Item1.FillStar();

            yield return null;
        }
        CurrentLerpToTotalPointsCoroutine = null;
    }

    List<Vector3> GetBezierApproximation(Vector3[] controlPoints, int outputSegmentCount)
    {
        Vector3[] points = new Vector3[outputSegmentCount + 1];
        for (int i = 0; i <= outputSegmentCount; i++)
        {
            float t = (float)i / outputSegmentCount;
            points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
        }
        return points.ToList();
    }

    Vector3 GetBezierPoint(float t, Vector3[] controlPoints, int index, int count)
    {
        if (count == 1)
            return controlPoints[index];

        var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
        var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
        return new Vector3((1 - t) * P0.x + t * P1.x, (1 - t) * P0.y + t * P1.y, 0);
    }
}
