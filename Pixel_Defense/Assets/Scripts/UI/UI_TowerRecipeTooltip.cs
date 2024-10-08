using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UI_TowerRecipeTooltip : DIMono
{
    public TextAutoResizer autoResizer;
    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
    }

    // Update is called once per frame
    public void setStatusText(TowerUnit tower)
    {
        StringBuilder sb = new StringBuilder();
        if (tower.Tower.grade == Tower.Grade.Hidden || tower.Tower.grade == Tower.Grade.HyperHidden)
            sb.AppendLine(tower.Tower.recipe);
        else
            sb.AppendLine("조합법이 없습니다.");
        autoResizer.SetText(sb.ToString());
    }

    public void setStatusText(Tower tower)
    {
        StringBuilder sb = new StringBuilder();
        if (tower.grade == Tower.Grade.Hidden || tower.grade == Tower.Grade.HyperHidden)
            sb.AppendLine(tower.recipe);
        else
            sb.AppendLine("조합법이 없습니다.");
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
