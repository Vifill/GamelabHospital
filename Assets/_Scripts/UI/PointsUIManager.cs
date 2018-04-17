using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PointsUIManager : MonoBehaviour 
{
    public GameObject PopupScoreText;
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
    Vector2 WorldToCanvas(Vector3 pVector)
    {
        RectTransform CanvasRect = DisplayedScoreText.transform.parent.parent.GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(pVector);
        return new Vector2(((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                           ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
    }
       
    public IEnumerator UpdateUI(int pPoints, Vector3 pPosition)
    {

        GameObject GO = (GameObject)Instantiate(PopupScoreText, DisplayedScoreText.transform.parent.parent);
        RectTransform GORect = GO.GetComponent<RectTransform>();
        Text tempText = GO.GetComponentInChildren<Text>();
        GORect.anchoredPosition = WorldToCanvas(pPosition);

        int TempScore = 0;

        Score += pPoints;
        float t = 1 + pPoints/50;
        if (Score < 0)
        {
            Score = 0;
        }
        while (TempScore != pPoints)
        {
            TempScore = (int)Mathf.MoveTowards(TempScore, pPoints, t);
            t += Time.deltaTime;
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
            pGO.transform.position = Vector3.Lerp(pGO.transform.position, BezierCurve[0], Time.deltaTime * Screen.height * 1.5f);
            if ((pGO.transform.position - BezierCurve[0]).sqrMagnitude < 2)
            {
                BezierCurve.Remove(BezierCurve[0]);
            }

            if ((pGO.transform.position - DisplayedScoreText.transform.position).sqrMagnitude < 1000)
            {
                ScoreAnimator.SetBool("GivingPoints", true);
            }

            if (BezierCurve.Count <= 0)
            {
                break;
            }
            yield return null;
        }
        DisplayedScore += pPoints;
        DisplayedScoreText.text = DisplayedScore.ToString();
        ScoreAnimator.SetBool("GivingPoints", false);
        Destroy(pText.gameObject);
        ParticleSystem.EmissionModule emmision = pGO.GetComponent<ParticleSystem>().emission;
        emmision.enabled = false;
        Destroy(pGO, 2);
        yield return null;
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
