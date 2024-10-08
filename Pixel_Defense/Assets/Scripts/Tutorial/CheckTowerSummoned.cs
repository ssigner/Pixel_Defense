using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTowerSummoned : DIMono
{
    [Inject]
    TowerManager towerManager;

    [Inject]
    TutorialManager tutorialManager;

    bool triggerFlag = false;
    protected override void Init()
    {
        base.Init();
    }
    // Update is called once per frame
    void Update()
    {
        if (towerManager.towerUnits.Count == 0) return;
        else if (towerManager.towerUnits.Count == 1 && !triggerFlag)
        {
            tutorialManager.trigger = true;
            triggerFlag = true;
        }
        else if (towerManager.towerUnits.Count == 2 && !triggerFlag)
        {
            tutorialManager.trigger = true;
            triggerFlag = true;
        }
    }
}
