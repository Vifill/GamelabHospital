using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TutorialStationManager : MonoBehaviour 
{
    private GameObject SpaceBarUI;
    public Vector3 UIOffset = new Vector3();
    public ConditionConfig ConditionTrigger;
    public GameObject ImageToDisplay;

    // Use this for initialization
    private void Start()
    {
        SpaceBarUI = Instantiate(ImageToDisplay, FindObjectOfType<Canvas>().transform);
        SpaceBarUI.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        var ailments = GetAilmentControllersWithCondition(ConditionTrigger.name);
        if (ailments != null && ailments.Any())
        {
            SpaceBarUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + UIOffset);
            SpaceBarUI.SetActive(true);
        }
        else if (ailments == null || !ailments.Any())
        {
            SpaceBarUI.SetActive(false);
        }
    }

    private List<AilmentController> GetAilmentControllersWithCondition(string pConditionName)
    {
        var controllers = FindObjectsOfType<AilmentController>();
        
        //var list = new List<AilmentController>(filteredControllers);

        if (controllers != null)
        {
            return null;
        }
        else
        {
            var filteredControllers = controllers.Where(a => a.GetCurrentCondition().name.Contains(pConditionName));

            if (filteredControllers == null)
            {
                return null;
            }
            else
            {
                var list = new List<AilmentController>(filteredControllers.Where(a => a.GetCurrentCondition().name.Contains(pConditionName)));
                return list;
            }
        }
    }
}
