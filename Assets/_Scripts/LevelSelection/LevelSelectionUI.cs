using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour 
{
    public Text ThreeStarsTxt;
    public Text TwoStarsTxt;
    public Text OneStarTxt;
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
            HighScoreText.text = "High Score: " + pLvlModel.Score;
            FillStars(pLvlModel.Score, pCfg);
        }

        ThreeStarsTxt.text = pCfg.StarConfig.PointsForGold.ToString();
        TwoStarsTxt.text = pCfg.StarConfig.PointsForSilver.ToString();
        OneStarTxt.text = pCfg.StarConfig.PointsForBronze.ToString();
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
