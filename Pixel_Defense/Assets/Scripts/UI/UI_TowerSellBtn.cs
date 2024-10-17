using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TowerSellBtn : DIMono
{
    
    [Inject]
    UI_TowerInfo ui_TowerInfo;

    [Inject]
    TowerManager towerManager;

    public GameObject towerRangeUI;
    protected override void Init()
    {
        base.Init();
        CheckInject();
    }

    // Update is called once per frame
    public void Click()
    {
        
        towerManager.SellTower(ui_TowerInfo.towerUnit);
        ui_TowerInfo.Hide();
        towerRangeUI.SetActive(false);
    }
}
