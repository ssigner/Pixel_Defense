using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToIngame : DIMono
{
    [Inject]
    SceneChanger sceneChanger;
    [Inject]
    AudioManager audioManager;
    [Inject]
    SettingData settingData;
    public void Click()
    {
        sceneChanger.LoadScene("Ingame", SceneChanger.LoadingScene.DiamondPattern);
        audioManager.PlayBGM(SettingData.BgmNames[settingData.bgmIdx]);
    }
}
