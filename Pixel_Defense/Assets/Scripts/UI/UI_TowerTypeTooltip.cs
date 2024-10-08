using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

public class UI_TowerTypeTooltip : DIMono
{
    public TextMeshProUGUI classText;
    protected override void Init()
    {
        base.Init();
    }

    public void setStatusText(Tower tower)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("등급 : ");
        switch (tower.grade)
        {
            case Tower.Grade.Normal:
                sb.Append("노말");
                break;
            case Tower.Grade.Rare:
                sb.Append("레어");
                break;
            case Tower.Grade.Unique:
                sb.Append("유니크");
                break;
            case Tower.Grade.Hidden:
                sb.Append("히든");
                break;
            case Tower.Grade.HyperHidden:
                sb.Append("레전드");
                break;
        }

        sb.Append("\n클래스 : ");
        switch (tower.towerClass)
        {
            case Tower.TowerClass.human:
                sb.Append("인간");
                break;
            case Tower.TowerClass.priest:
                sb.Append("사제");
                break;
            case Tower.TowerClass.spirit:
                sb.Append("정령");
                break;
            case Tower.TowerClass.not_human:
                sb.Append("이종족");
                break;
        }
        classText.text = sb.ToString();
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

