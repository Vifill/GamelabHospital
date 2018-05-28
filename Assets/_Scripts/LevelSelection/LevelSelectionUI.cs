using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    public Image CollectableOne;
    public Image CollectableTwo;
    public Image CollectableThree;
    public Text LevelName;
    public Image StarOne;
    public Image StarTwo;
    public Image StarThree;
    public Text HighScoreText;
    public Sprite YellowStar;

    public void Initialize(LevelConfig pCfg, int pLvlNo, LevelModel pLvlModel)
    {
        if (pLvlNo != 0)
        {
            LevelName.text = "Level " + pLvlNo;
        }
        else
        {
            LevelName.text = "Tutorial Level";
        }

        if (pLvlModel != null)
        {
            HighScoreText.text = pLvlModel.Score.ToString();
            FillStars(pLvlModel.Score, pCfg);
        }
        PlayerDataController playerData = new PlayerDataController();
        var collectables =  playerData.GetCollectableModels().Where(a => a.Level == pCfg.LevelNumber).OrderBy(a => a.IsFound).ToList();
        for (int i = 0; i < 3; i++)
        {
            if (collectables == null || i >= collectables.Count())
            {
                switch (i)
                {
                    case 0:
                        Destroy(CollectableOne.gameObject);
                        continue;
                        break;
                    case 1:
                        Destroy(CollectableTwo.gameObject);
                        continue;
                        break;
                    case 2:
                        Destroy(CollectableThree.gameObject);
                        continue;
                        break;
                }
            }
            switch (i)
            {
                case 0:
                    CollectableOne = collectables[i].Image;
                    break;
                case 1:
                    CollectableTwo = collectables[i].Image;
                    break;
                case 2:
                    CollectableTwo = collectables[i].Image;
                    break;
            }
        }

    }

    private void FillStars(int pScore, LevelConfig pCfg)
    {
        if (pScore >= pCfg.StarConfig.PointsForBronze)
        {
            StarOne.sprite = YellowStar;
        }
        if (pScore >= pCfg.StarConfig.PointsForSilver)
        {
            StarTwo.sprite = YellowStar;
        }
        if (pScore >= pCfg.StarConfig.PointsForGold)
        {
            StarThree.sprite = YellowStar;
        }
    }
}
