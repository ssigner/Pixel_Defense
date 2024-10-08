using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_TowerSkillTooltip : DIMono
{
    public TextAutoResizer autoResizer;
    [Inject]
    StagePlayData stagePlayData;
    [Inject]
    GameData gameData;

    protected override void Init()
    {
        base.Init();
    }

    public void setStatusText(Tower tower)
    {
        CheckInject();
        StringBuilder sb = new StringBuilder();
        if(tower.towerClass == Tower.TowerClass.priest)
        {
            float extraBuff = 0;

            if(stagePlayData.priestLevel != 0)
            {
                var row = gameData.upgradePriest.FindByLevel(stagePlayData.priestLevel);
                extraBuff = row.val;
            }
               
            sb.Append("공격 범위 내의\n아군타워의 공격력을\n" + (tower.skill.fxParam[0] + extraBuff).ToString() + "증가시킨다.");
        }
        else
        {
            sb.Append(tower.skill.script);
        }
        autoResizer.SetText(sb.ToString());
    }

    internal void Hide()
    {
        this.gameObject.SetActive(false);
    }

    internal void Show()
    {

        this.gameObject.SetActive(true);
    }
}
