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
            LevelName.text = pLvlNo.ToString();
        }
        else
        {
            LevelName.text = "Main Menu";
        }

        if (pLvlModel != null)
        {
            //HighScoreText.text = pLvlModel.Score.ToString();
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
                        Destroy(CollectableOne.gameObject.transform.parent.gameObject);
                        continue;
                    case 1:
                        Destroy(CollectableTwo.gameObject.transform.parent.gameObject);
                        continue;
                }
                continue;
            }

            if (collectables[i].IsFound)
            {
                Texture2D tex2D = collectables[i].Texture2D;
                switch (i)
                {
                    case 0:
                        CollectableOne.sprite = Sprite.Create(tex2D, new Rect(0,0,tex2D.width,tex2D.height), CollectableOne.sprite.pivot, CollectableOne.sprite.pixelsPerUnit);
                        break;
                    case 1:
                        CollectableTwo.sprite = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), CollectableOne.sprite.pivot, CollectableOne.sprite.pixelsPerUnit);
                        break;
                    case 2:
                        CollectableThree.sprite = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), CollectableOne.sprite.pivot, CollectableOne.sprite.pixelsPerUnit);
                        break;
                }
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
