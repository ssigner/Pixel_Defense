using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneChanger
{
    public enum LoadingScene
    {
        FadeInOut,
        DiamondPattern
    }

    public string fromScene, toScene, loadingScene;

    public void LoadScene(string toScene, LoadingScene loadingScene )
    {
        fromScene = SceneManager.GetActiveScene().name;
        this.toScene = toScene;
        this.loadingScene = loadingScene.ToString();
        SceneManager.LoadScene(this.loadingScene, LoadSceneMode.Additive);

    }
}

public class GlobalInstaller 
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static public void GameStart()
    {
        Debug.Log("GameStart");

        var gamedata = Addressables.LoadAssetAsync<GameData>("Assets/Prefabs/data.asset").WaitForCompletion();
        gamedata.Initialize();
        DIContainer.Global.Regist(gamedata);
        //var stageConfig = Addressables.LoadAssetAsync<StageConfig>("Assets/Prefabs/StageConfig.asset").WaitForCompletion();
        //DIContainer.Global.Regist(stageConfig);

     

        var settingData = SettingData.Load();
        if (settingData == null)
        {
            settingData = GetDefaultSettingData();
            Debug.Log(settingData);
            settingData.SaveFile();
        }

        DIContainer.Global.Regist(settingData);

        Screen.SetResolution(1920, 1080, settingData.GetFullScreenMode());

        DIContainer.Global.Regist(new UserData());
        DIContainer.Global.Regist(new SceneChanger());

        Screen.fullScreenMode = settingData.GetFullScreenMode();
        

        PlayData playData = PlayData.LoadFromFile();
        if (playData == null)
        {
            playData = new PlayData();
            SaveLoader.SaveFile<PlayData>("playdata.data", playData);
        }

        DIContainer.Global.Regist(playData);

    }

    private static SettingData GetDefaultSettingData()
    {
        return new SettingData()
        {
            bgmIdx = 0,
            tutorialPlayed = 0,
            screentTypeIdx = 0,
            bgmVolume = 2f,
            sfxVolume = 2f,
        };
    }
}
