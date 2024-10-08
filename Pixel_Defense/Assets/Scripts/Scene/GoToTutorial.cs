using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTutorial : DIMono
{
    [Inject]
    SceneChanger sceneChanger;
    [Inject]
    AudioManager audioManager;
    [Inject]
    SettingData settingData;
    public void Click()
    {
        sceneChanger.LoadScene("Tutorial", SceneChanger.LoadingScene.DiamondPattern);
        audioManager.PlayBGM(SettingData.BgmNames[settingData.bgmIdx]);
    }
}
