using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

public class UI_TowerStatusTooltip : DIMono
{

    public TextAutoResizer autoResizer;
    protected override void Init()
    {
        base.Init();
    }

    public void setStatusText(TowerUnit tower)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("공격력 : " + tower.GetStatus(Status.Atk).ToString("F1"));
        sb.AppendLine("공격속도 : " + tower.GetStatus(Status.AtkSpeed).ToString("F2"));
        sb.AppendLine("공격범위 : " + tower.Tower.atkRange.ToString());
        autoResizer.SetText(sb.ToString());
    }

    public void setStatusText(Tower tower)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("공격력 : " + tower.atk.ToString());
        sb.AppendLine("공격속도 : " + tower.atkSpeed.ToString());
        sb.AppendLine("공격범위 : " + tower.atkRange.ToString());
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

