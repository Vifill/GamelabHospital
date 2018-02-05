using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialToolManager : MonoBehaviour 
{
    private ToolController ToolCtrl;
    private TutorialUIController TutorialUI;
    private GameObject SpaceBarUI;
    public Vector3 UIOffset = new Vector3();

	// Use this for initialization
	private void Start () 
	{
        ToolCtrl = FindObjectOfType<ToolController>();
        TutorialUI = FindObjectOfType<TutorialUIController>();
        SpaceBarUI = Instantiate(TutorialUI.PressSpaceImg, FindObjectOfType<Canvas>().transform);
        SpaceBarUI.SetActive(false);
	}
	
	// Update is called once per frame
	private void Update () 
	{
		if (ToolCtrl.GetToolBase()?.gameObject != gameObject)
        {
            SpaceBarUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + UIOffset);
            SpaceBarUI.SetActive(true);
        }
        else
        {
            SpaceBarUI.SetActive(false);
        }
	}

    private void OnDestroy()
    {
        Destroy(SpaceBarUI);
    }
}
