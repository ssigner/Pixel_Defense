using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTowerComb : DIMono
{
    [Inject]
    TowerManager towerManager;

    [Inject]
    UI_TowerInfo UI_towerInfo;

    public GameObject combFailedUI;

    TowerUnit selectedTower;
    BtnSfx sfx;

    protected override void Init()
    {
        base.Init();
        CheckInject();
        sfx = this.gameObject.GetComponent<BtnSfx>();
    }

    public void clickCombBtn()
    {
        selectedTower = UI_towerInfo.towerUnit;
        var combCheck = towerManager.normalCombTower(selectedTower, combFailedUI);
        if (UI_towerInfo.gameObject.activeInHierarchy && combCheck)
        {
            UI_towerInfo.Hide();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)){
            clickCombBtn();
            sfx.Click();
        }
    }
}
