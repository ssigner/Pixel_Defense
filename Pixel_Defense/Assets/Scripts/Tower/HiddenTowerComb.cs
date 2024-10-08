using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTowerComb : DIMono
{
    [Inject]
    TowerManager towerManager;

    [Inject]
    UI_TowerInfo UI_towerInfo;

    public GameObject combFailedUI;
    public GameObject hiddenCombFrame;
    TowerUnit selectedTower;
    public UI_HiddenTowerPlace ui_HiddenTowerPlace;

    // Update is called once per frame
    protected override void Init()
    {
        base.Init();
        CheckInject();
        Debug.Log("find check HiddenTowerPlace");
    }

    public void clickHiddenCombBtn()
    {
        if (hiddenCombFrame.activeSelf)
        {
            Hide();
        }
        selectedTower = UI_towerInfo.towerUnit;
        var canSummonHiddenList = towerManager.findHiddenCombTower(selectedTower);
        Debug.Log(canSummonHiddenList);
        if (canSummonHiddenList == null)
        {
            towerManager.popupCombFailedUI(combFailedUI);
            return;
        }
        Show();
        ui_HiddenTowerPlace.setTowerCell(canSummonHiddenList);
    }

    public void Show()
    {
        hiddenCombFrame.gameObject.SetActive(true);
        UI_towerInfo.isHiddenBtnClicked = true;
    }

    public void Hide()
    {
        hiddenCombFrame.gameObject.SetActive(false);
        UI_towerInfo.isHiddenBtnClicked = false;
    }
}
