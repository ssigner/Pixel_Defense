using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePause : DIMono
{
    [Inject]
    StagePlayData stagePlayData;

    public void gamePause()
    {
        if (stagePlayData.isPause)
        {
            Time.timeScale = stagePlayData.currentTimeScale;
            stagePlayData.isPause = false;
            if (stagePlayData.pauseCount == 0)
            {
                this.GetComponent<Button>().interactable = false;
                return;
            }
        }
        else
        {
            stagePlayData.pauseCount--;
            Time.timeScale = 0;
            stagePlayData.isPause = true;
        }
    }
}
