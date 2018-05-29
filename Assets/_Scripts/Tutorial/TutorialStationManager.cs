using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TutorialStationManager : MonoBehaviour 
{
    private GameObject SpaceBarUI;
    public Vector3 UIOffset = new Vector3();
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
    }

    //private List<AilmentController> GetAilmentControllersWithCondition(string pConditionName)
    //{
    //    var controllers = FindObjectsOfType<AilmentController>();
        
    //    //var list = new List<AilmentController>(filteredControllers);

    //    if (controllers != null)
    //    {
    //        return null;
    //    }
    //    else
    //    {
    //        var filteredControllers = controllers.Where(a => a.GetCurrentCondition().name.Contains(pConditionName));

    //        if (filteredControllers == null)
    //        {
    //            return null;
    //        }
    //        else
    //        {
    //            var list = new List<AilmentController>(filteredControllers.Where(a => a.GetCurrentCondition().name.Contains(pConditionName)));
    //            return list;
    //        }
    //    }
    //}
}
