using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenInfo : TutorialBase
{
    
    [SerializeField]
    private List<GameObject> ScreenInfoView;
    int screenInfoIdx;
    [Inject]
    TutorialManager tutorialManager;
    protected override void Init()
    {
        base.Init();
        screenInfoIdx = 0;
    }
    protected override void Enter()
    {
        ;
    }

    protected override void Execute()
    {
        ScreenInfoView[screenInfoIdx].SetActive(true);
        if (screenInfoIdx != 0)
        {
            ScreenInfoView[screenInfoIdx - 1].SetActive(false);
        }
    }
    protected override void Exit()
    {
        gameObject.SetActive(false);
        NextStep.SetActive(true);
    }
    

    private void Update()
    {
        if (tutorialManager.trigger == false) return;
        if (screenInfoIdx == ScreenInfoView.Count)
        {
            tutorialManager.trigger = false;
            Exit();
            return;
        }
        Execute();
        screenInfoIdx++;
        tutorialManager.trigger = false;
    }
}
