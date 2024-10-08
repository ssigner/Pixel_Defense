using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class GameSystem : DIMono
{

    [Inject]
    GameData gameData;

    [Inject]
    PlayData playData;

    [Inject]
    StagePlayData stagePlayData;

    [Inject]
    MobManager mobManager;

    [Inject]
    UI_TotalTime totalTime;

#if UNITY_EDITOR
    [Serializable]
    public class TestSetting
    {
        public bool nowTest;
        public int stageCode;
        public int gold;
        public int emelard;
        public int iron;
        public int hp;
        

        public void CheckAndApplySetting(GameSystem GameSystem)
        {
            if (nowTest == true)
            {
                GameSystem.StartCoroutine(ApplySettingIE(GameSystem));
                setMoney(GameSystem);
                GameSystem.stagePlayData.userHp = hp;
            }
        }

        IEnumerator ApplySettingIE(GameSystem GameSystem)
        {
            yield return null;  
            GameSystem.stagePlayData.currentStage = GameSystem.gameData.stages.Find(l => l.code == stageCode);
            GameSystem.mobManager.setMobPool();

        }

        void setMoney(GameSystem gameSystem)
        {
            gameSystem.stagePlayData.gold = gold;
            gameSystem.stagePlayData.emelard = emelard;
            gameSystem.stagePlayData.iron = iron;
        }

    }

    [Header("Test용 설정")]
    public TestSetting testSetting;
#endif

    public GameObject winUI, loseUI;
    const int EndStageNum = 40;
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI lostRecordText;
    private bool isEnd;

    protected override void Init()
    {
        base.Init();
        Debug.Log($"code : {stagePlayData.currentStage.code} ");
        isEnd = false;
#if UNITY_EDITOR
        testSetting.CheckAndApplySetting(this);
#endif
    }

    public float StagePlayTime
    {
        get => stagePlayData.playStageTime;
        set => stagePlayData.playStageTime = value;
    }
    float MobSummonDuration;

    void StagePlayTimeProcess()
    {
        if (isEnd) return;
        if(stagePlayData.userHp <= 0)
        {
            GameLoseProcess();
        }
        //0. 일시정지 버튼 눌렀을 시 return
        if (stagePlayData.isPause)
            return;

        //1. 다음 스테이지로 진행시
        if (stagePlayData.needGoToNextStage)
        {
            if (stagePlayData.currentStage.code == EndStageNum && stagePlayData.userHp > 0)
            {
                GameWinProcess();
                isEnd = true;
                return;
            }
            ToNextStage();
            stagePlayData.needGoToNextStage = false;
        }

        //2. 1초 증가
        var preSec = (int)StagePlayTime;
        StagePlayTime += Time.deltaTime;
        var afterSec = (int)StagePlayTime; 

        //3. 증가한 초가 5, 스테이지 시간 둘에 따라 분류

        if(preSec != afterSec && afterSec == 20 && stagePlayData.isFightRound == false)
        {
            //4. 쉬는시간 끝
            stagePlayData.isFightRound = true;
            stagePlayData.playStageTime = 1f;
            return;
        }
        CheckSummonMob(preSec);
    }


    #region Next Stage Process
    private void ToNextStage() 
    {
        stagePlayData.gold += 300;
        if(stagePlayData.currentStage.code % 10 == 0 || stagePlayData.currentStage.code == 35)
        {
            stagePlayData.emelard += 2;
        }
        stagePlayData.currentStage = gameData.stages.Find(l => l.code == stagePlayData.currentStage.code + 1);
        if (stagePlayData.currentStage == null)
        {
            throw new NotImplementedException("스테이지 없을때 처리!");
        }
        mobManager.setMobPool();
        stagePlayData.isFightRound = false;
        StagePlayTime = 1f;
    }
    #endregion
    /*    private void StageEndProcess()
        {
            int sumOfAtk=0;

            var mobList = mobManager.mobs.ToList();
            foreach (var m in mobList)
            {
                sumOfAtk+=m.Monster.atk;
                m.ReturnToPool();
            }

            stagePlayData.userHp -= sumOfAtk;

            Debug.Log($"HP : {stagePlayData.userHp}");
        }*/
    #region Set Mob Process
    private void CheckSummonMob(int preSec)
    {
        MobSummonDuration = stagePlayData.currentStage.mobNum;
        var nowSec = (int)StagePlayTime;

        if (CheckMobSummonCondition(preSec, nowSec))
            return;
        //Debug.Log($"CheckSummonMob current stage : {stagePlayData.currentStage.code}");
        mobManager.SummonMob(stagePlayData.currentStage.monster);
    }

    private bool CheckMobSummonCondition(int preSec, int nowSec)
    {
        return preSec == nowSec ||
                    StagePlayTime > MobSummonDuration+2 || stagePlayData.isFightRound == false;
    }
    //||stagePlayData.isFightRound == false;
    #endregion

    #region Game Win Lose Process
    private void GameWinProcess()
    {
        if(playData.gameTimeRecord > totalTime.totalPlayTime)
        {
            playData.gameTimeRecord = totalTime.totalPlayTime;
            playData.gameTimeRecordText = totalTime.totalTimeText;
        }
        setWinTimeText();
        playData.bestStage = 41;
        SaveLoader.SaveFile(SaveLoader.playdataFileName, playData);
        winUI.SetActive(true);
    }

    private void setWinTimeText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("기록");
        sb.AppendLine();
        sb.Append(totalTime.totalTimeText);
        totalTimeText.text = sb.ToString();
    }
    private void GameLoseProcess()
    {
        loseUI.SetActive(true);
        setLoseRecordText();
        playData.bestStage = stagePlayData.currentStage.code;
        SaveLoader.SaveFile(SaveLoader.playdataFileName, playData);
        isEnd = true;
    }
    
    private void setLoseRecordText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("기록");
        sb.AppendLine();
        sb.Append(stagePlayData.currentStage.code.ToString() + "스테이지");
        lostRecordText.text = sb.ToString();
    }
    #endregion

    void Update()
    {
        StagePlayTimeProcess();
        if (stagePlayData.isFightRound && mobManager.mobs.Count <= 0 && StagePlayTime > MobSummonDuration+2 && !isEnd)
        {
            //Debug.Log("main update to next stage");
            stagePlayData.needGoToNextStage = true;
            stagePlayData.isFightRound = false;
        }
    }
}
