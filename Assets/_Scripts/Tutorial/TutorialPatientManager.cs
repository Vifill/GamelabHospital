using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPatientManager : MonoBehaviour 
{
    private PatientStatusController StatusCtrl;
    private TutorialUIController TutorialUI;
    private ToolController ToolCtrl;
    private GameObject SpaceBarImg;
    public Vector3 UIOffset = new Vector3();

	// Use this for initialization
	private void Start () 
	{
        StatusCtrl = GetComponent<PatientStatusController>();
        TutorialUI = FindObjectOfType<TutorialUIController>();
        ToolCtrl = FindObjectOfType<ToolController>();
        SpaceBarImg = Instantiate(TutorialUI.HoldSpaceImg, FindObjectOfType<Canvas>().transform);
        SpaceBarImg.SetActive(false);
	}
	
	// Update is called once per frame
	private void Update () 
	{
        if (StatusCtrl.IsInBed /*&& AilmentCtrl.GetCurrentCondition().ToolNeeded == ToolCtrl.GetCurrentToolName()*/)
        {
            SpaceBarImg.transform.position = Camera.main.WorldToScreenPoint(transform.position + UIOffset);
            SpaceBarImg.SetActive(true);
        }
        else
        {
            SpaceBarImg.SetActive(false);
        }

        if (StatusCtrl.IsHealed || StatusCtrl.IsDead)
        {
            SpaceBarImg.SetActive(false);
        }
	}

    private void OnDestroy()
    {
        Destroy(SpaceBarImg);
    }
}
