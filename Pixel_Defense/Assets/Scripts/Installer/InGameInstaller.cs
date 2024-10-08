using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameInstaller : MonoBehaviour
{
    GameData gameData;

    public ObjectPoolManager poolManager;
    public MobPath mobPath;
    public MobManager mobManager;

    public TowerManager towerManager;
    public TowerPlacer towerPlacer;
    public UI_TowerInfo UI_TowerInfo;
    public UI_TotalTime totalTime;
    public GameObject atkRange;

    public StagePlayData stagePlayData;
    public int stageCode;

    public Material originalMaterial,hitMaterial;
    
    public Camera mainCam;

    [Inject]
    public PlayData playData;

    public AudioManager audioManagerPrefab;

    public TutorialManager tutorialManager;

    private void Awake()
    {
        var container= new DIContainer(); ;
        DIContainer.Current = container;

        if (DIContainer.Global.Has(typeof(AudioManager),"") ==false)
        {
            Instantiate(audioManagerPrefab);
        }

        gameData = DIContainer.GetValue(typeof(GameData)) as GameData;
        initStagePlayData();
        stagePlayData.currentStage = gameData.stages.First(l => l.code == stageCode);
        Debug.Log($"currentStage : {stagePlayData.currentStage.code}, mob : {stagePlayData.currentStage.monster.prefabPath}");

        container.Regist(totalTime);
        container.Regist(stagePlayData);
        container.Regist(poolManager);
        container.Regist(mobPath);
        container.Regist(towerPlacer);
        container.Regist(towerManager);
        container.Regist(mobManager);
        container.Regist(mainCam);
        container.Regist(UI_TowerInfo);
        container.Regist(tutorialManager);
        container.Regist(originalMaterial, "SpriteDefault");
        container.Regist(hitMaterial, "Hit");
        container.Regist(atkRange, "atkRange");
        
        prepareSkillProjectile();
    }

    void prepareSkillProjectile()
    {
        var pathes = gameData.towers.Where(l => string.IsNullOrEmpty(l.projectilePrefabPath) == false).Select(l => l.projectilePrefabPath).Distinct();

        foreach(var path in pathes)
        {
            var SkillPrefab = Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();

            poolManager.InitializeObjectPool(new ObjectPoolManager.PrefabInfo()
            {
                prefab = SkillPrefab,
                key = path,
                poolSize = 20

            });
        }
    }

    void initStagePlayData()
    {
        gameData.Initialize();
        stagePlayData.userHp = 50;
        
        stagePlayData.pauseCount = 3;
        stagePlayData.isFightRound = false;
        stagePlayData.isPause = false;
        stagePlayData.needGoToNextStage = false;
        stagePlayData.humanLevel = 0;
        stagePlayData.spiritLevel = 0;
        stagePlayData.priestLevel = 0;
        stagePlayData.notHumanLevel = 0;
        stagePlayData.gold = 500;
        stagePlayData.emelard = 0;
        stagePlayData.iron = 50;
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            stagePlayData.gold = 200;
            stagePlayData.emelard = 2;
            stagePlayData.iron = 50;
        }
    }

}
