using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageTimer : DIMono
{
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI roundText;
    [Inject]
    StagePlayData StagePlayData;

    int preVal;
    int printVal = 0;
    // Update is called once per frame
    void Update()
    {
        var curVal= (int)StagePlayData.playStageTime * 1;

        if(curVal != preVal)
        {
            if (StagePlayData.isFightRound)
            {
                printVal = 90 - curVal;
            }
            else
            {
                printVal = 45 - curVal;
            }
            timerText.text = (printVal / 60).ToString() + ":" + (printVal % 60).ToString();
            roundText.text = StagePlayData.stageCount.ToString();
            if (StagePlayData.isFightRound && printVal == 0)
            {
                StagePlayData.isFightRound = false;
                StagePlayData.playStageTime = 1f;
            }
            else if (!StagePlayData.isFightRound && printVal == 0)
            {
                StagePlayData.stageCount++;
                StagePlayData.isFightRound = true;
                StagePlayData.playStageTime = 1f;
            }
            
        }

        preVal=curVal;

    }
}
