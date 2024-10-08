using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;

public class UserData
{
  
 
}
[Serializable]
public class SettingData
{
    public int bgmIdx;
    public int? tutorialPlayed = 0;
    public int screentTypeIdx;
    public float bgmVolume;
    public float sfxVolume;
    public readonly static List<string> BgmNames = new()
    {
        "Climbing The Citadel",
        "Elemental Duel",
        "Entering The Citadel",
        "Finale",
        "Frozen Lake",
        "Hidden Area 29",
        "Hunt For Success",
        "Raindrop",
        "The Tower",
        "Up a Tree",
        "West Forest"
    };

    public readonly static List<string> ScreentType = new()
    {
        "FullScreen",
        "Window"
    };

    const string FileName = "SettingData.data";
    public static readonly FullScreenMode[] fullScreenModes = new FullScreenMode[]
    {
        FullScreenMode.FullScreenWindow,
        FullScreenMode.Windowed,
    };

    public FullScreenMode GetFullScreenMode()
    {
        return fullScreenModes[screentTypeIdx];
    }

    public static SettingData Load()
    {
        return SaveLoader.LoadFromFile<SettingData>(FileName);

    }

    public void SaveFile()
    {
        SaveLoader.SaveFile(FileName, this);
    }


}