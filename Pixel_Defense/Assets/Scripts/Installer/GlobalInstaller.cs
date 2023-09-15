using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GlobalInstaller 
{


    [RuntimeInitializeOnLoadMethod]
    static public void GameStart()
    {
        Debug.Log("GameStart");

        var gamedata = Addressables.LoadAssetAsync<GameData>("Assets/Prefabs/data.asset").WaitForCompletion();
        DIContainer.Global.Regist(gamedata);
        var stageConfig = Addressables.LoadAssetAsync<StageConfig>("Assets/Prefabs/StageConfig.asset").WaitForCompletion();
        DIContainer.Global.Regist(stageConfig);

    }


}
