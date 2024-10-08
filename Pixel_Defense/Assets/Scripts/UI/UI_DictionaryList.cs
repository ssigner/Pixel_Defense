using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class UI_DictionaryList : DIMono
{

    [Inject]
    GameData gameData;

    [Inject]
    PlayData playData;

    public VerticalLayoutGroup verticalLayout;
    public GameObject unlockedTower;

    public GameObject protoItem;

    public ScrollRect scrollRect;

    /*  private void OnEnable()
      {
          EventBus.Subscribe<TowerSummonEvent>(OnTowerSummon);
      }

      private void OnDisable()
      {
          EventBus.Unsubscribe<TowerSummonEvent>(OnTowerSummon);
      }

      private void OnTowerSummon(TowerSummonEvent obj)
      {
          Debug.Log("TowerSummoned " + obj.towerUnit.name);
      }*/

    private void OnEnable()
    {
        
        if (scrollRect != null)
        {
            scrollRect.normalizedPosition = new Vector2(0, 1);
        }

        if (unlockTowerCode.Count == 0)
        {
            return;
        }
        playData.towerDictionary.Sort();
        var idx = 0;
        for(idx=unlockTowerCode.Count-1;idx>=0; idx--)
        {
            var code = unlockTowerCode[idx];
            if (!playData.towerDictionary.Contains(code))
            {
                continue;
            }
            Debug.Log("Unlocked! "+code);
            unlockTowerCode.Remove(code);
            var unlockObj=GetCellItem(idx);

            Destroy(unlockObj.gameObject);

            var cell = Instantiate(protoItem, verticalLayout.transform);
            var tower = gameData.towers.Find(l => l.code == code);
            cell.GetComponent<UI_TowerInfoBase>().SetTower(tower);
            cell.gameObject.SetActive(true);
            cell.transform.SetSiblingIndex(idx);
        }
    }

    List<int> unlockTowerCode = new List<int>();


    protected override void Init()
    {
        base.Init();
        if(playData.towerDictionary.Count >= 1)
        {
            playData.towerDictionary.Sort();
        }
        List<Tower.Grade> gradeList = new();
        gradeList.Add(Tower.Grade.Normal);
        gradeList.Add(Tower.Grade.Rare);
        gradeList.Add(Tower.Grade.Unique);
        gradeList.Add(Tower.Grade.Hidden);
        gradeList.Add(Tower.Grade.HyperHidden);
        //TODO: 도감 업데이트 되게

        var index = 0;
        for(int i = 0; i < gradeList.Count; i++)
        {
            foreach (var tower in gameData.towers.Where(l => l.grade == gradeList[i]))
            {
                if (!playData.towerDictionary.Contains(tower.code))
                {
                    unlockTowerCode.Add(tower.code);
                    var unlockedCell = Instantiate(unlockedTower, verticalLayout.transform);
                    unlockedCell.gameObject.SetActive(true);
                    index++;
                    continue;
                }
                var cell = Instantiate(protoItem, verticalLayout.transform);
                cell.GetComponent<UI_TowerInfoBase>().SetTower(tower);
                cell.gameObject.SetActive(true);
                index++;
            }
        }
    }
    private Transform GetCellItem(int idx)
    {
     
        return verticalLayout.transform.GetChild(idx);
    }

    private void Update()
    {
    }
}
