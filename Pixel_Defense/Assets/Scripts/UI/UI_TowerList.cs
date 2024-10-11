using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TowerList : DIMono
{

    [Inject]
    GameData gameData;
    public Tower.Grade grade;
    public GridLayoutGroup gridLayout;
    GameObject protoItem;
    public List<UI_TowerCell> UI_TowerCells;

    protected override void Init()
    {
        base.Init();

        protoItem = gridLayout.transform.GetChild(0).gameObject;

        int index = 0;

        foreach (var tower  in  gameData.towers.Where(l=>l.grade == grade))
        {
            var cell = GetCellItem(index);
            cell.SetData(tower);
            cell.gameObject.SetActive(true);
            if(tower.grade == Tower.Grade.Normal)
            {
                cell.shortcut_key.text = (index + 1).ToString();
            }
            UI_TowerCells.Add(cell);
            index++;

        }
    }

    private UI_TowerCell GetCellItem(int idx)
    {
        if( idx >= gridLayout.transform.childCount)
        {
            return  Instantiate(protoItem,gridLayout.transform).GetComponent<UI_TowerCell>();            

        }
        return gridLayout.transform.GetChild(idx).GetComponent<UI_TowerCell>();
    }

    private void Update()
    {
        for (int i = 0; i < 6; i++)  // 0부터 5까지 반복 (Alpha1 ~ Alpha6에 대응)
        {
            if (i >= UI_TowerCells.Count || UI_TowerCells[i] == null)
                continue;

            // 키 입력 체크
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UI_TowerCells[i].Click();  // 해당 UI_TowerCells 인덱스의 Click() 호출
            }
        }
    }
}
