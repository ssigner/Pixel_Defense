using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PauseCount : DIMono
{
    [Inject]
    StagePlayData stagePlayData;

    public TextMeshProUGUI countText;
    int preCount;
    protected override void Init()
    {
        base.Init();
        CheckInject();
        preCount = stagePlayData.pauseCount;
        countText.text = "³²Àº È½¼ö : " + stagePlayData.pauseCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(preCount != stagePlayData.pauseCount)
        {
            countText.text = "³²Àº È½¼ö : " + stagePlayData.pauseCount.ToString();
        }
    }
}
