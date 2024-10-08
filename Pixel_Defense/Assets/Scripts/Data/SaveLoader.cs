using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SaveLoader
{
    public const string playdataFileName = "playdata.data";
    public const string settingdataFileName = "settingdata.data";
    private static string GetSavePath(string fileName)
    {
        var basePath = Application.persistentDataPath;

        var playDataPath = Path.Join(basePath, fileName);
        return playDataPath;
    }

    public static void SaveFile<T>(string fileName,T obj)
    {
        var savePath = GetSavePath(fileName);
        if (File.Exists(savePath) == true)
        {
            File.Delete(savePath);
        }

        var json = JsonConvert.SerializeObject(obj);
        File.WriteAllText(savePath, json);
    }

    public static T LoadFromFile<T>(string fileName) where T: class
    {
        string playDataPath = GetSavePath(fileName);

        if (File.Exists(playDataPath) == false)
        {
            return null;
        }
        var json = File.ReadAllText(playDataPath);

        //   JsonConvert.SerializeObject(playdata);
        return JsonConvert.DeserializeObject<T>(json);

    }
}