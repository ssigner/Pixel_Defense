using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuyPlace : DIMono
{

    [Inject]
    GameData gameData;

    public GridLayoutGroup gridLayout;

    GameObject protoItem;

    protected override void Init()
    {
        base.Init();

        protoItem = gridLayout.transform.GetChild(0).gameObject;
        var protoItemCell = protoItem.GetComponent<UI_TowerCell>();
        protoItemCell.SetData(gameData.towers.First());

    }

}
