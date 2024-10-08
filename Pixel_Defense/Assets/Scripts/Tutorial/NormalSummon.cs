using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSummon : TutorialBase
{
    [Inject]
    TutorialManager tutorialManager;

    [Inject]
    TowerManager towerManager;

    List<GameObject> btns;

    bool firstViewFlag = false;
    protected override void Enter()
    {
        for(int i = 0; i < btns.Count; i++)
        {
            if (i == tutorialManager.step)
            {
                btns[i].SetActive(true);
                continue;
            }
            btns[i].SetActive(false);
        }
    }

    protected override void Execute()
    {
        tutorialManager.scriptIdx++;
    }

    protected override void Exit()
    {
        gameObject.SetActive(false);
        NextStep.SetActive(true);
    }

    protected override void Init()
    {
        base.Init();
        btns = tutorialManager.menuBtns;
        Enter();
        //Exit();
    }
    private void Update()
    {
        
        if (tutorialManager.trigger == false) return;
        if (tutorialManager.trigger && !firstViewFlag)
        {
            firstViewFlag = true;
            tutorialManager.trigger = false;
            return;
        }
        Execute();
        tutorialManager.trigger = false;
    }

}
