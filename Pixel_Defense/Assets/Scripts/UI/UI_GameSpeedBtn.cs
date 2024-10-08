using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameSpeedBtn : DIMono
{
    public int cnt = 0;
    public TextMeshProUGUI speedText;
    [Inject]
    StagePlayData stagePlayData;

    protected override void Init()
    {
        this.CheckInject();
        cnt = 0;
        SetTimeScaleByClickCnt(cnt);
    }

/*    public void Click()
    {
        if(cnt == 0)
        {
            Time.timeScale = 1.5f;   //1,1.5
            speedText.text = "1.5";
            cnt++;
            return;
        }
        if(cnt == 1)
        {
            Time.timeScale = 2.0f;  //2 2
            speedText.text = "2";
            cnt++;
            return;
        }
        if(cnt == 2)
        {
            Time.timeScale = 1f;  //0 1                            
            speedText.text = "1";
            cnt = 0;
            return;
        }
    }*/

    public void OnClick()
    {
        if (stagePlayData.isPause) return;
        cnt++;
        cnt %= 4;
        SetTimeScaleByClickCnt(cnt);

    }

    public void SetTimeScaleByClickCnt(int cnt)
    {   //0 1
        //1 2
        //2 3
        //3 4

        float timeScale = 1 + cnt;
        speedText.text = "x"+timeScale.ToString("f1");
        Time.timeScale = timeScale;
        stagePlayData.currentTimeScale = timeScale;
            
    }
}
