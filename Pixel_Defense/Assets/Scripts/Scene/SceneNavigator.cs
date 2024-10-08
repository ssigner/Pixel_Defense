using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNavigator : DIMono
{
    [Inject]
    SceneChanger sceneChanger;

    [Inject]
    AudioManager audioManager;

    [Inject]
    SettingData settingData;

    public enum SceneType
    {
        Ingame,
        TowerDictionary,
        Tutorial
    }

    public SceneType sceneType;

    public void GoToScene()
    {
        switch (sceneType)
        {
            case SceneType.Ingame:
                sceneChanger.LoadScene("Ingame", SceneChanger.LoadingScene.DiamondPattern);
                if (audioManager != null && settingData != null)
                {
                    audioManager.PlayBGM(SettingData.BgmNames[settingData.bgmIdx]);
                }
                break;

            case SceneType.TowerDictionary:
                sceneChanger.LoadScene("TowerDictionary", SceneChanger.LoadingScene.DiamondPattern);
                break;

            case SceneType.Tutorial:
                sceneChanger.LoadScene("Tutorial", SceneChanger.LoadingScene.DiamondPattern);
                if (audioManager != null && settingData != null)
                {
                    audioManager.PlayBGM(SettingData.BgmNames[settingData.bgmIdx]);
                }
                break;
        }
    }
}
