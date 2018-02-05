using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController
{
    public const string LevelScoreKey = "ScoreLevel";
    public const string LevelsCompletedKey = "LevelsCompleted";
    public const string PlayerIDKey = "PlayerIDKey";
    public const string VersionKey = "Version";
    
    public void CheckForVersion(int pVersion)
    {
        if (!PlayerPrefs.HasKey(VersionKey) || PlayerPrefs.GetInt(VersionKey) != pVersion)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(VersionKey, pVersion);
        }
    }

    public void SaveLevelData(LevelModel pLvlModel)
    {
        string levelScoreKey = LevelScoreKey + pLvlModel.LevelNo;

        if (PlayerPrefs.HasKey(levelScoreKey))
        {
            int oldScore = PlayerPrefs.GetInt(levelScoreKey);

            if (pLvlModel.Score > oldScore)
            {
                PlayerPrefs.SetInt(levelScoreKey, pLvlModel.Score);
            }
        }
        else
        {
            PlayerPrefs.SetInt(levelScoreKey, pLvlModel.Score);
        }

        if (PlayerPrefs.HasKey(LevelsCompletedKey))
        {
            if (pLvlModel.LevelNo > PlayerPrefs.GetInt(LevelsCompletedKey))
            {
                PlayerPrefs.SetInt(LevelsCompletedKey, pLvlModel.LevelNo);
            }
        }
        else
        {
            PlayerPrefs.SetInt(LevelsCompletedKey, pLvlModel.LevelNo);
        }
    }

    public LevelModel GetLevelData(int pLevelNo)
    {
        if (PlayerPrefs.HasKey(LevelScoreKey + pLevelNo))
        {
            return new LevelModel(pLevelNo, PlayerPrefs.GetInt(LevelScoreKey + pLevelNo));
        }
        else
        {
            return null;
        }
    }

    public int? GetLevelsCompleted()
    {
        if (PlayerPrefs.HasKey(LevelsCompletedKey))
        {
            return PlayerPrefs.GetInt(LevelsCompletedKey);
        }
        else
        {
            return null;
        }
    }

    internal string GetUserID()
    {
        if(PlayerPrefs.HasKey(PlayerIDKey))
        {
            return PlayerPrefs.GetString(PlayerIDKey);
        }
        else
        {
            string newID = CreateUserID();
            PlayerPrefs.SetString(PlayerIDKey, newID);
            return newID;
        }
    }

    private string CreateUserID()
    {
        return Guid.NewGuid().ToString();
    }

    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void UnlockAllLevels()
    {
        PlayerPrefs.SetInt(LevelsCompletedKey, 5);
    }
}

public class LevelModel
{
    public int LevelNo;
    public int Score;

    public LevelModel(int pLvlNo, int pHighScore)
    {
        LevelNo = pLvlNo;
        Score = pHighScore;
    }

    //public static LevelModel GetDefaultModel(int pLvlNo)
    //{
    //    return new LevelModel(pLvlNo, null);
    //}
}
