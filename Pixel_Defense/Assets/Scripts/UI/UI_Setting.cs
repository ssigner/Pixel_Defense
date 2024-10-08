using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Setting : DIMono
{
    [Inject]
    AudioManager audioManager;
    [Inject]
    SettingData settingData;

    public Scrollbar bgmVolume;
    public Scrollbar sfxVolume;
    public TMP_Dropdown bgmName;
    public TMP_Dropdown screenType;

    public TMP_Dropdown resolutionDropdown;
    List<Resolution> resolutions = new();
    int resolutionNum;

    protected override void Init()
    {
        base.Init();
        if (settingData != null)
        {
            settingData = SettingData.Load();
        }
        bgmVolume.value = settingData.bgmVolume;
        sfxVolume.value = settingData.sfxVolume;
        bgmName.value = settingData.bgmIdx;
        screenType.value = settingData.screentTypeIdx;
        InitUI();
    }

    public bool CheckCurrentScene(string sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        return (currentScene.name == sceneName);
    }

    private void OnDisable()
    {
        settingData.SaveFile();
    }


    public void SetSFXVoulume(float v)
    {
        audioManager.SetSFXVolume(v);
        settingData.sfxVolume = v;

    }

    public void SetBGMVolume(float v)
    {
        audioManager.SetBGMVolume(v);
        settingData.bgmVolume = v;
    }

    public void SetBGM(int idx)
    {
        var BGMName = SettingData.BgmNames[idx];

        settingData.bgmIdx = idx;

        audioManager.PlayBGM(BGMName);
    }

    public void SetBGMInLobby(int idx)
    {
        settingData.bgmIdx = idx;
    }

    public void SetScreenMode(int idx)
    {
        settingData.screentTypeIdx = idx;
        Screen.fullScreenMode = settingData.GetFullScreenMode();
    }
    void InitUI()
    {
        
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.ToString() == "60")
                resolutions.Add(Screen.resolutions[i]);
        }
        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new();
            option.text = item.width + " x " + item.height;
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, settingData.GetFullScreenMode());
        Screen.fullScreenMode = settingData.GetFullScreenMode();
    }

}
