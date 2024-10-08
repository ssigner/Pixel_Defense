using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BestRecord : DIMono
{
    [Inject]
    PlayData playData;

    public TextMeshProUGUI bestRecord;
    protected override void Init()
    {
        base.Init();
        if(playData.bestStage < 41)
        {
            bestRecord.text = "최고기록 : "+ playData.bestStage.ToString() + "스테이지";

        }
        else
        {
            bestRecord.text = "최고기록 : " + playData.gameTimeRecordText;
        }
    }
}
