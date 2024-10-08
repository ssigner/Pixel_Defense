using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLobby : DIMono
{
    [Inject]
    SceneChanger sceneChanger;

    [Inject]
    PlayData playData;

    public void Click()
    {
        Time.timeScale = 1f;
        sceneChanger.LoadScene("lobby", SceneChanger.LoadingScene.DiamondPattern);
        SaveLoader.SaveFile(SaveLoader.playdataFileName, playData);
    }
}
