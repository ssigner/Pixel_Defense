using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameToGoToLobby : DIMono
{
    [Inject]
    SceneChanger sceneChanger;

    [Inject]
    PlayData playData;

    [Inject]
    StagePlayData stagePlayData;
    public void Click()
    {
        if (stagePlayData.isPause)
        {
            stagePlayData.isPause = false;
        }
        Time.timeScale = 1f;
        sceneChanger.LoadScene("lobby", SceneChanger.LoadingScene.DiamondPattern);
        SaveLoader.SaveFile(SaveLoader.playdataFileName, playData);
    }
}
