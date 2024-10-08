using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class UI_TutorialBtn : DIMono
{
    [Inject]
    TutorialManager tutorialManager;

    protected override void Init()
    {
        base.Init();
    }
    public void Click()
    {
        tutorialManager.scriptIdx++;
        tutorialManager.trigger = true;
    }
}
