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

        sb.Append("��� : ");
        switch (tower.grade)
        {
            case Tower.Grade.Normal:
                sb.Append("�븻");
                break;
            case Tower.Grade.Rare:
                sb.Append("����");
                break;
            case Tower.Grade.Unique:
                sb.Append("����ũ");
                break;
            case Tower.Grade.Hidden:
                sb.Append("����");
                break;
            case Tower.Grade.HyperHidden:
                sb.Append("������");
                break;
        }

        sb.Append("\nŬ���� : ");
        switch (tower.towerClass)
        {
            case Tower.TowerClass.human:
                sb.Append("�ΰ�");
                break;
            case Tower.TowerClass.priest:
                sb.Append("����");
                break;
            case Tower.TowerClass.spirit:
                sb.Append("����");
                break;
            case Tower.TowerClass.not_human:
                sb.Append("������");
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

