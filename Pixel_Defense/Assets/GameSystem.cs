using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameSystem : DIMono
{
    [Inject]
    ObjectPoolManager poolManager;

    [Inject]
    MobPath mobPathl;

    [Inject]
    GameData gameData;

    [Inject]
    StageConfig stageConfig;

    [Inject]
    StagePlayData stagePlayData;

    protected override void Init()
    {
        base.Init();
        
        var mobInfo = gameData.monsters[0];

        poolManager.InitializeObjectPool(new ObjectPoolManager.PrefabInfo()
        {
            key= mobInfo.prefabPath,
            poolSize=40,
            prefab= Addressables.LoadAssetAsync<GameObject>(mobInfo.prefabPath).WaitForCompletion(),

        });
        StartCoroutine(CallMethodRepeatedly());
    }

    public float StagePlayTime
    {
        get => stagePlayData.playStageTime;
        set => stagePlayData.playStageTime = value;
    }

    private IEnumerator CallMethodRepeatedly()
    {
        float totalTime = stageConfig.stageDuraction;
        float MobTime = 40f; // 호출을 원하는 총 시간

        
        while (StagePlayTime < totalTime)
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            if(StagePlayTime < MobTime && stagePlayData.isFightRound)
            {
                SummonMob(gameData.monsters[0]);
            }                                     // 1초 간격으로 실행할 코드를 여기에 작성합니다.

            StagePlayTime += 1f; // 경과 시간 업데이트
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space))
        {
            SummonMob(gameData.monsters[0]);

           // mobObj
        }
        
    }

    private void SummonMob(Monster mob)
    {
        string prefabKey = "MobProto";
        var mobObj=poolManager.GetObjectFromPool(prefabKey);

        mobObj.GetComponent<Mob>().SetData(mob);


    }
}
