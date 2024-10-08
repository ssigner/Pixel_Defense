using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PlayData
{
    public List<int> towerDictionary = new List<int>();
    public float gameTimeRecord = 99999999f;
    public int bestStage = 0;
    public string gameTimeRecordText = "";

    const string FileName = "playdata.data";

    internal static PlayData LoadFromFile()
    {
        return SaveLoader.LoadFromFile<PlayData>(FileName);

    }

    public void AddData(int code)
    {
        towerDictionary.Add(code);
        towerDictionary.Sort();
    }


    public bool checkDistinct(int inputTowerCode)
    {
        foreach(var code in towerDictionary)
        {
            if (code == inputTowerCode) return false;
        }
        return true;
    }
}

[Serializable]
public class StagePlayData
{
    public Stage currentStage;
    public float playStageTime;
    public int stageCount;
    public bool isFightRound;
    public bool isPause;
    public int userHp;
    public bool needGoToNextStage;
    public int gold;
    public int emelard;
    public int iron;
    public int pauseCount;
    public float currentTimeScale;

    public int humanLevel;
    public int spiritLevel;
    public int notHumanLevel;
    public int priestLevel;

    public int normalCombCount;
    public int hiddenCombCount;

}