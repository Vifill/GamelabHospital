using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTableManager : MonoBehaviour 
{
    private ToolController ToolCtrl;
    private TutorialUIController TutorialUI;
    private GameObject SpaceBarUI;
    private TableStation Table;
    public Vector3 YOffset = new Vector3();

    // Use this for initialization
    private void Start () 
	{
        ToolCtrl = FindObjectOfType<ToolController>();
        TutorialUI = FindObjectOfType<TutorialUIController>();
        Table = GetComponent<TableStation>();
        SpaceBarUI = Instantiate(TutorialUI.PressSpaceImg, FindObjectOfType<Canvas>().transform);
        SpaceBarUI.SetActive(false);
    }
	
	// Update is called once per frame
	private void Update () 
	{
		if (ToolCtrl.GetCurrentToolName() != ToolName.NoTool && Table.TableObject == null)
        {
            SpaceBarUI.transform.position = Camera.main.WorldToScreenPoint(transform.position) + YOffset;
            SpaceBarUI.SetActive(true);
        }
        else
        {
            SpaceBarUI.SetActive(false);
        }
	}
}
