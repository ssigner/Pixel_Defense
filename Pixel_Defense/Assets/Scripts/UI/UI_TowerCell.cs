using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UI_TowerCell : DIMono
{
    public Image towerImg;
    public GameObject menuUI;

    [Inject]
    TowerPlacer TowerPlacer;
    [Inject]
    StagePlayData stagePlayData;

    [Inject]
    AudioManager audioManager;

    public GameObject buyFailedUI;

    public TextMeshProUGUI shortcut_key;

    Tower tower;
    internal void SetData(Tower tower)
    {
        CheckInject();

        this.tower = tower;
        var handle = Addressables.LoadAssetAsync<Sprite>(tower.spritePath);
        handle.Completed += (asyncOperationHandle) =>
        {
            towerImg.sprite = asyncOperationHandle.Result;
        };
    }


    public void Click()
    {
        CheckInject();

        bool canBuy = false;
        int requiredResource = 0;
        int playerResource = 0;

        if(tower.grade == Tower.Grade.Normal)
        {
            requiredResource = tower.price;
            playerResource = stagePlayData.gold;
        }
        else if (tower.grade == Tower.Grade.Rare || tower.grade == Tower.Grade.Unique)
        {
            requiredResource = tower.hyperPrice;
            playerResource = stagePlayData.emelard;
        }

        if(requiredResource <= playerResource)
        {
            canBuy = true;
        }
        if (canBuy)
        {
            audioManager.Play("BtnClickAndBuyUnit");
            menuUI.SetActive(false);
            TowerPlacer.StartPlace(tower, towerImg.sprite);
        }
        else
        {
            audioManager.Play("FailedPopup");
            PopupBuyFailedUI();
        }
    }
    WaitForSeconds UIPopUpDuration = new WaitForSeconds(1f);
    void FailedUIShow()
    {
        buyFailedUI.SetActive(true);
    }
    void FailedUIHide()
    {
        buyFailedUI.SetActive(false);
    }
    void PopupBuyFailedUI()
    {
        StartCoroutine(IsInvalidBuy());
    }
    IEnumerator IsInvalidBuy()
    {
        FailedUIShow();
        // 1초 대기
        yield return UIPopUpDuration;

        // 원래의 머티리얼로 복원
        FailedUIHide();
    }
}
