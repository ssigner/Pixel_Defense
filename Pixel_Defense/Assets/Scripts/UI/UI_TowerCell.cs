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
        towerImg.sprite = Addressables.LoadAssetAsync<Sprite>(tower.spritePath).WaitForCompletion();
    }


    public void Click()
    {
        if (tower.price <= stagePlayData.gold && tower.grade == Tower.Grade.Normal)
        {
            audioManager.Play("BtnClickAndBuyUnit");
            menuUI.SetActive(false);
            TowerPlacer.StartPlace(tower, towerImg.sprite);
        }
        else if(tower.price > stagePlayData.gold && tower.grade == Tower.Grade.Normal)
        {
            audioManager.Play("FailedPopUp");
            PopupBuyFailedUI();
        }
        if (tower.hyperPrice <= stagePlayData.emelard && (tower.grade == Tower.Grade.Rare || tower.grade == Tower.Grade.Unique))
        {
            audioManager.Play("BtnClickAndBuyUnit");
            menuUI.SetActive(false);
            TowerPlacer.StartPlace(tower, towerImg.sprite);
        }
        else if (tower.hyperPrice > stagePlayData.emelard && (tower.grade == Tower.Grade.Rare || tower.grade == Tower.Grade.Unique))
        {
            audioManager.Play("FailedPopUp");
            PopupBuyFailedUI();
        }
    }

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
    WaitForSeconds UIPopUpDuration = new WaitForSeconds(1f);
    IEnumerator IsInvalidBuy()
    {
        FailedUIShow();
        // 1초 대기
        yield return UIPopUpDuration;

        // 원래의 머티리얼로 복원
        FailedUIHide();
    }
}
