using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour 
{
    private LevelController LvlCtrl;
    public Transform LevelNodesParent;
    public int Version;
    private List<LevelNode> LevelNodes = new List<LevelNode>();
    private PlayerDataController DataCtrl = new PlayerDataController();

	// Use this for initialization
	private void Start () 
	{
        LvlCtrl = new LevelController(FindObjectOfType<SceneLoader>());
        Time.timeScale = 1;
        DataCtrl.CheckForVersion(Version);

        foreach (Transform node in LevelNodesParent)
        {
            LevelNodes.Add(node.GetComponent<LevelNode>());
        }

        if (DataCtrl.GetLevelsCompleted() != null)
        {
            foreach (var node in LevelNodes)
            {
                if (node.LevelNo <= DataCtrl.GetLevelsCompleted() + 1)
                {                    
                    node.Initialize(DataCtrl.GetLevelData(node.LevelNo));
                    if (node.LevelNo == DataCtrl.GetLevelsCompleted() + 1)
                    {
                        node.ActivateArrowParticleEffect();
                    }
                }
            }
        }
        else
        {
            var firstNode = LevelNodes.Where(a => a.LevelNo == 0).FirstOrDefault();
            firstNode.Initialize(null);
            firstNode.ActivateArrowParticleEffect();
        }
       

        //if (PlayerPrefs.HasKey(PlayerDataController.LevelProgressKey))
        //{
        //    for (int i = 0; i < (PlayerPrefs.GetInt(PlayerDataController.LevelProgressKey) + 1); i++)
        //    {
        //        var lvlNode = LevelNodes.Where(a => a.LvlNumber == i).FirstOrDefault();
        //        if (PlayerPrefs.HasKey(PlayerDataController.LevelScoreKey + i))
        //        {
        //            //LevelNodes[i].Initialize(PlayerPrefs.GetInt(DataCtrl.LevelScoreKey + i));
        //            lvlNode.Initialize(PlayerPrefs.GetInt(PlayerDataController.LevelScoreKey + i));
        //        }
        //        else
        //        {
        //            lvlNode.Initialize(0);
        //        }
        //    }
        //}
        //else
        //{
        //    LevelNodes.Where(a => a.LvlNumber == 0).FirstOrDefault().Initialize(0);
        //}
        
	}
    
    public void BackBtn()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        LvlCtrl.LoadMainMenu();
    }

}
