using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TowerInfoCloseBtn : DIMono
{

    [Inject]
    UI_TowerInfo UI_towerInfo;

    private TowerUnit selectedTower;
    private UI_TowerInfoTrigger selectedTowerTrigger;

    protected override void Init()
    {
        base.Init();
    }

    public void Click()
    {
        selectedTower = UI_towerInfo.towerUnit;
        selectedTowerTrigger = selectedTower.GetComponent<UI_TowerInfoTrigger>();
        selectedTowerTrigger.HideAllUI();
    }

}
