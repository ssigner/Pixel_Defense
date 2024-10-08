using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MoneyExchangeBtn : DIMono
{
    [Inject]
    StagePlayData stagePlayData;

    public GameObject exchangeFailedUI;
    protected override void Init()
    {
        base.Init();
        CheckInject();
    }

    // Update is called once per frame
    public void Click()
    {
        var parentName = transform.parent.gameObject.name;
        if(parentName == "gold2iron")
        {
            if(stagePlayData.gold < 100)
            {
                popupExchangeFailedUI();
                return;
            }
            int randomIron = (int)Random.Range(10f, 30f);
            stagePlayData.gold -= 100;
            stagePlayData.iron += randomIron;
        }
        else if(parentName == "emerald2gold")
        {
            if (stagePlayData.emelard < 1)
            {
                popupExchangeFailedUI();
                return;
            }
            stagePlayData.gold += 200;
            stagePlayData.emelard -= 1;
        }
        else if(parentName == "emerald2iron")
        {
            if (stagePlayData.emelard < 1)
            {
                popupExchangeFailedUI();
                return;
            }
            stagePlayData.iron += 60;
            stagePlayData.emelard -= 1;
        }
    }

    void FailedUIShow()
    {
        exchangeFailedUI.SetActive(true);
    }
    void FailedUIHide()
    {
        exchangeFailedUI.SetActive(false);
    }
    void popupExchangeFailedUI()
    {
        StartCoroutine(IsInvalidExchange());
    }
    WaitForSeconds UIPopUpDuration = new WaitForSeconds(1f);
    IEnumerator IsInvalidExchange()
    {
        FailedUIShow();
        // 1초 대기
        yield return UIPopUpDuration;

        // 원래의 머티리얼로 복원
        FailedUIHide();
    }
}
