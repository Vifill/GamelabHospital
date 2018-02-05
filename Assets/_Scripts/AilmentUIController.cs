using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AilmentUIController : MonoBehaviour 
{
    private AilmentController AilmentController;
    private GameObject ConditionUI;
    private Image ConditionImg;
    private Image ConditionTimer;
    private float TimeForCondition;
    private Canvas Canvas;
    private Animator ConditionUIAnimator;

    public Image PatientStatusUIPrefab;
    public Text ScorePopUpTextPrefab;
    public Sprite HealedImg;
    public Sprite DeathImg;
    public float UIOffset = 2;
    public PatientStatusColorConfig PatientColorConfig;


    // Use this for initialization
    private void Start () 
	{
        AilmentController = GetComponent<AilmentController>();

        var currentCondition = AilmentController.GetCurrentCondition();
        //var canvas = GameObject.FindGameObjectWithTag("WorldUICanvas").GetComponent<Canvas>();
        //var canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        Canvas = FindObjectOfType<Canvas>();
        ConditionUI = Instantiate(PatientStatusUIPrefab, Canvas.transform).gameObject;
        ConditionUI.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, UIOffset);
        ConditionImg = ConditionUI.transform.GetChild(0).GetComponent<Image>();
        SetImage(currentCondition.ImageToShow);
        ConditionTimer = ConditionUI.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        ConditionUIAnimator = ConditionUI.GetComponent<Animator>();

        TimeForCondition = currentCondition.TimeToHeal;
    }
	
	// Update is called once per frame
	private void Update () 
	{
        ConditionUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, UIOffset, UIOffset));
        //UpdateConditionTimer();
	}

    public void UpdateConditionTimerUI(float pCurrentTime)
    {

        ConditionTimer.fillAmount = 1 - pCurrentTime / TimeForCondition;

        if (ConditionTimer.fillAmount >= 0.6f)
        {
            ConditionTimer.color = PatientColorConfig.StatusGreen;
            ConditionUIAnimator.SetBool("IsPulsating", false);
        }
        else if (ConditionTimer.fillAmount >= 0.3f)
        {
            ConditionTimer.color = PatientColorConfig.StatusYellow;
            ConditionUIAnimator.SetBool("IsPulsating", false);
        }
        else
        {
            ConditionTimer.color = PatientColorConfig.StatusRed;
            ConditionUIAnimator.SetBool("IsPulsating", true);
        }

        if (ConditionTimer.fillAmount <= 0.0f)
        {
            //PatientStatusController.Death();
            SetImage(DeathImg);
            ConditionUIAnimator.SetBool("IsPulsating", false);
            //ConditionsCleared = true;
        }
    }

    public void UpdateConditionUI()
    {
        var currentCondition = AilmentController.GetCurrentCondition();

        if (currentCondition != null)
        {
            SetImage(currentCondition.ImageToShow);
            TimeForCondition = currentCondition.TimeToHeal;
            ConditionTimer.fillAmount = 1;
        }
        
        else
        {
            SetImage(HealedImg);
            ConditionTimer.fillAmount = 1;
            ConditionUIAnimator.SetBool("IsPulsating", false);
            ConditionTimer.color = PatientColorConfig.StatusGreen;
        }
    }

    public void CreateScorePopUpText(int pScore)
    {
        var ScoreText = Instantiate(ScorePopUpTextPrefab, Canvas.transform).gameObject;
        
        if (pScore <= 0)
        {
            ScoreText.GetComponent<Text>().text = pScore.ToString();
            ScoreText.GetComponent<Text>().color = Color.red;
        }
        else
        {
            ScoreText.GetComponent<Text>().text = "+" + pScore.ToString();
            ScoreText.GetComponent<Text>().color = Color.green;
        }
        ScoreText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, UIOffset, UIOffset));
        StartCoroutine(AnimateScoreText(ScoreText));
    }

    private IEnumerator AnimateScoreText(GameObject pScoreText)
    {
        var Counter = 0f;
        var Rect = pScoreText.GetComponent<RectTransform>();
        pScoreText.GetComponent<Text>().CrossFadeAlpha(0, 2f, false);
        while (Counter <= 2)
        {
            Counter += Time.deltaTime;            
            Rect.localPosition = new Vector3(Rect.localPosition.x, Rect.localPosition.y + Counter, Rect.localPosition.z);

            yield return null;
        }
        Destroy(pScoreText.gameObject);
    }

    private void SetImage(Sprite pSprite)
    {
        ConditionImg.GetComponent<Image>().sprite = pSprite;
    }

    public void OnDestroy()
    {
        Destroy(ConditionUI);
    }
}
