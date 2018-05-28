using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour 
{
    private LevelController LvlCtrl;
    public Transform LevelNodesParent;
    public List<Collectable> CollectablesPrefabs;
    public int Version;
    private List<LevelNode> LevelNodes = new List<LevelNode>();
    private PlayerDataController DataCtrl = new PlayerDataController();

	// Use this for initialization
	private void Start () 
	{
        UpdateCollectables();
        LvlCtrl = new LevelController(FindObjectOfType<SceneLoader>());
        Time.timeScale = 1;
        //DataCtrl.CheckForVersion(Version);

        foreach (Transform node in LevelNodesParent)
        {
            LevelNodes.Add(node.GetComponent<LevelNode>());
        }

        if (DataCtrl.GetLevelsCompleted() != null)
        {
            foreach (var node in LevelNodes)
            {
                if (node.LevelNo <= DataCtrl.GetLevelsCompleted())
                {                    
                    node.Initialize(DataCtrl.GetLevelData(node.LevelConfig.LevelNumber));
                    if (node.LevelNo == DataCtrl.GetLevelsCompleted())
                    {
                        node.ActivateArrowParticleEffect();
                    }
                }
            }
        }
        else
        {
            var firstNode = LevelNodes.FirstOrDefault(a => a.LevelNo == 1);
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

    void UpdateCollectables()
    {
        Collectables collectables = new Collectables();
        if (PlayerPrefs.HasKey(PlayerDataController.CollectableKey))
        {
            collectables = JsonUtility.FromJson<Collectables>(PlayerPrefs.GetString(PlayerDataController.CollectableKey));
            collectables.CollectableList = collectables.CollectableList.Where(a => CollectablesPrefabs.Select(b=> b.CollectableModel).Contains(a)).ToList();
            var newItems = CollectablesPrefabs.Where(a => !collectables.CollectableList.Contains(a.CollectableModel)).Select(a=> a.CollectableModel);// .Contains() collectables.CollectableList.First(b => b.ID == a.CollectableModel.ID && b.Level == a.CollectableModel.Level) == null).Select(a => a.CollectableModel).ToList();
            collectables.CollectableList.AddRange(newItems);
            collectables.CollectableList.OrderBy(a => a.Level);
            for (int i = 0; i < collectables.CollectableList.Count; i++)
            {
                print("is found? " + collectables.CollectableList[i].IsFound);
                print("ID: " + collectables.CollectableList[i].ID);
                print("Level " + collectables.CollectableList[i].Level);
            }
            PlayerPrefs.SetString(PlayerDataController.CollectableKey, JsonUtility.ToJson(collectables));
            print("SavedShit");
        }
        else
        {
            collectables.CollectableList = CollectablesPrefabs.Select(a => a.CollectableModel).ToList();
            PlayerPrefs.SetString(PlayerDataController.CollectableKey, JsonUtility.ToJson(collectables));
            print("Saved shit for the first time");
        }
    }
    
    public void BackBtn()
    {
        FindObjectOfType<MusicController>().PlayButtonSound();
        LvlCtrl.LoadMainMenu();
    }

}
