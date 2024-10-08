using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_StageTimer : DIMono
{
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI roundText;
    [Inject]
    StagePlayData stagePlayData;

    [Inject]
    MobManager mobManager;

    [Inject]
    GameData gameData;

    int preVal;
    int printVal = 0;
    // Update is called once per frame
    void Update()
    {
        var curVal= (int)stagePlayData.playStageTime * 1;

        if(curVal != preVal)
        {
            if (stagePlayData.isFightRound)
            {
                timerText.text = "몬스터 : " + mobManager.mobs.Count.ToString();
            }
            else
            {
                printVal = 20 - curVal;
                timerText.text = (printVal / 60).ToString() + ":" + (printVal % 60).ToString();
            }
            roundText.text = stagePlayData.currentStage.code.ToString() +
                "라운드 : " +
                gameData.monsters.FirstOrDefault(I => I.code == stagePlayData.currentStage.code).name;
        }

        preVal=curVal;

    }
}
