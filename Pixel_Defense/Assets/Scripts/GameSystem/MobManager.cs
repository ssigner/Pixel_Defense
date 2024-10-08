using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;

public class MobManager : DIMono
{
    [Inject]
    ObjectPoolManager poolManager;

    [Inject]
    StagePlayData stagePlayData;

    public List<Mob> mobs = new List<Mob>();

    int IDCounter = 1;

    protected override void Init()
    {
        base.Init();
        if (stagePlayData.currentStage.code == 1)
        {
            this.setMobPool();
        }
    }

    public void setMobPool()
    {

        var mobInfo = stagePlayData.currentStage.monster;

        //Debug.Log($"currentStage : {stagePlayData.currentStage.code}, mob : {stagePlayData.currentStage.monster.prefabPath}");

        var mobPrefab = Addressables.LoadAssetAsync<GameObject>(mobInfo.prefabPath).WaitForCompletion();

        poolManager.InitializeObjectPool(new ObjectPoolManager.PrefabInfo()
        {
            prefab = mobPrefab,
            key = mobInfo.prefabPath,
            poolSize = stagePlayData.currentStage.mobNum,
        });
    }

    public void SummonMob(Monster monsterData)
    {
        //Debug.Log("Summon Mob Step" + monsterData.name);
        string prefabKey = "MobProto";
        var mobObj = poolManager.GetObjectFromPool(prefabKey);
        var mob = mobObj.GetComponent<Mob>();
        mob.SetData(monsterData, IDCounter++);
        mobs.Add(mob);
    }



    internal void mobRemovedCalledByMob(Mob mob)
    {
        mobs.Remove(mob);
    }
}