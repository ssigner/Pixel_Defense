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
        for (int i = 0; i < 6; i++)  // 0���� 5���� �ݺ� (Alpha1 ~ Alpha6�� ����)
        {
            if (i >= UI_TowerCells.Count || UI_TowerCells[i] == null)
                continue;

            // Ű �Է� üũ
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UI_TowerCells[i].Click();  // �ش� UI_TowerCells �ε����� Click() ȣ��
            }
        }
    }
}
