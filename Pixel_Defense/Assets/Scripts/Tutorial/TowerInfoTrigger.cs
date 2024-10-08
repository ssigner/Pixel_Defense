using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfoTrigger : DIMono
{
    [Inject]
    TutorialManager tutorialManager;
    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        tutorialManager.trigger = true;
    }
}
