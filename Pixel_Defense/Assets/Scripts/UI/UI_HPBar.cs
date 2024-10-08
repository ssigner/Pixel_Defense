using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : DIMono
{
    [Inject]
    public StagePlayData stagePlayData;

    public Image gageImg;

    const int MaxHP = 50;

    int preHP;
    protected override void Init()
    {
        base.Init();

        preHP = stagePlayData.userHp;
        gageImg.fillAmount = preHP / (float)MaxHP;

    }


    private void Update()
    {
        if (preHP != stagePlayData.userHp)
        {
            gageImg.fillAmount = stagePlayData.userHp / (float)MaxHP;
            preHP=stagePlayData.userHp;
            //Debug.Log("gageImg.fillAmount " + gageImg.fillAmount + " "+ preHP);
        }
    }
}
