using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtn : DIMono
{
    public GameObject UnitPlace;
    [Inject]
    TutorialManager tutorialManager;
    bool firstCheck;
    protected override void Init()
    {
        base.Init();
        firstCheck = false;
    }
    public void Click()
    {
        if(!UnitPlace.activeSelf)
        {
            UnitPlace.SetActive(true);
            if (!firstCheck)
            {
                tutorialManager.trigger = true;
                firstCheck = true;
            }
        }
        else UnitPlace.SetActive(false);
    }

}
