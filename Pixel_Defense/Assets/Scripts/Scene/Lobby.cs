using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : DIMono
{

    [Inject]
    SceneChanger SceneChanger;

    [Inject]
    AudioManager audioManager;

    [Inject]
    SettingData settingData;

    public GameObject gotoTutorial;

    public Button[] lobbyButtons;

    protected override void Init()
    {
        base.Init();
        if (settingData != null)
        {
            settingData = SettingData.Load();
        }
        audioManager.SetVolume(settingData);
        audioManager.PlayBGM("Intro");
    }

    // Update is called once per frame

    public void GameStart()
    {

        SceneChanger.LoadScene("InGame",  SceneChanger.LoadingScene.DiamondPattern);
        audioManager.PlayBGM(SettingData.BgmNames[settingData.bgmIdx]);

    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
