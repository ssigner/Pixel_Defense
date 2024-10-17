using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using TMPro;
using UnityEngine;

public class UI_TowerSkillTooltipDictionary : DIMono
{
    public TextAutoResizer autoResizer;
    protected override void Init()
    {
        base.Init();
    }

    public void SetStatusText(Tower tower)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(tower.skill.script);
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

