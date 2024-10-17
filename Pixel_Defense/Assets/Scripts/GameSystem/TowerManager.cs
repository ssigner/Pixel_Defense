using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class TowerManager : DIMono
{ 
    public List<TowerUnit> towerUnits = new List<TowerUnit> ();
    int IDCounter = 1;

    public GameObject multiHiddenUI;

    List<Tower> RareTowers = new List<Tower>();
    List<Tower> UniqueTowers = new List<Tower>();
    List<Tower> HiddenTowers = new List<Tower>();
    [Inject]
    GameData gameData;

    [Inject]
    TowerPlacer towerPlacer;

    [Inject]
    PlayData playData;

    [Inject]
    AudioManager audioManager;

    [Inject]
    StagePlayData stagePlayData;

    protected override void Init()
    {
        base.Init();
        CheckInject();
        for(int i = 0; i < gameData.towers.Count; i++)
        {
            if (gameData.towers[i].grade == Tower.Grade.Normal) continue;
            else if (gameData.towers[i].grade == Tower.Grade.Rare) RareTowers.Add(gameData.towers[i]);
            else if (gameData.towers[i].grade == Tower.Grade.Unique) UniqueTowers.Add(gameData.towers[i]);
            else if (gameData.towers[i].grade == Tower.Grade.Hidden || gameData.towers[i].grade == Tower.Grade.HyperHidden) HiddenTowers.Add(gameData.towers[i]);
        }
    }

    private void towerUnitLog()
    {
        foreach(TowerUnit towerUnit in towerUnits)
        {
            Debug.Log($"{towerUnit.ID} : {towerUnit.Tower.name}, towerCODE : {towerUnit.Tower.code}");
        }
    }
    #region SummonTower
    public void SummonTower(Tower towerData, Vector3Int towerPlace, GameObject towerPrefab)
    {
        Vector3 realPlace = towerPlace + new Vector3(1, 0f);
        var towerObj = Instantiate(towerPrefab, realPlace, Quaternion.identity);
        var towerUnit = towerObj.GetComponent<TowerUnit>();
        towerUnit.SetData(towerData, IDCounter++, towerPlace);

        towerUnits.Add(towerUnit);
        foreach (var t in towerUnits)
        {
            t.PassiveBuffProcess();
        }
        if (playData.checkDistinct(towerData.code))
        {
            playData.AddData(towerData.code);
        }
    }
    #endregion

    #region NormalComb
    private TowerUnit FindNearGradeTower(TowerUnit selectedTower)
    {
        if (towerUnits.Count <= 1) return null;
        float minDistance = 99999f;
        var towerPosition = this.transform.position;
        TowerUnit neareastTower = null;
        foreach (TowerUnit towerUnit in towerUnits)
        {
            if (towerUnit.Tower.code != selectedTower.Tower.code || towerUnit.ID == selectedTower.ID)
            {
                continue;
            }
            else
            {
                var targetPosition = towerUnit.transform.position;
                var distV = targetPosition - towerPosition;
                var sqrDist = distV.sqrMagnitude;
                if (minDistance > sqrDist)
                {
                    minDistance = sqrDist;
                    neareastTower = towerUnit;
                }
            }
        }
        return neareastTower;
    }
    //normalComb
    public bool NormalCombTower(TowerUnit selectedTower, GameObject combFailedUI)
    {
        var targetTower = FindNearGradeTower(selectedTower);
        if (targetTower == null || selectedTower.Tower.grade == Tower.Grade.Unique || selectedTower.Tower.grade == Tower.Grade.Hidden)
        {
            PopupCombFailedUI(combFailedUI);
            return false;
        }

        var combTower = new List<Tower>();

        if(selectedTower.Tower.grade == Tower.Grade.Normal)
        {
            combTower = RareTowers.OrderBy(g => Guid.NewGuid()).Take(1).ToList();
        }
        else if(selectedTower.Tower.grade == Tower.Grade.Rare)
        {
            combTower = UniqueTowers.OrderBy(g => Guid.NewGuid()).Take(1).ToList();
        }
        var combTowerPrefab = Addressables.LoadAssetAsync<GameObject>(combTower[0].prefabPath).WaitForCompletion();
        SummonTower(combTower[0], selectedTower.currentPlace, combTowerPrefab);
        selectedTower.gameObject.GetComponent<UI_TowerInfoTrigger>().HideTowerAtkRange();

        RemoveTower(selectedTower);
        RemoveTower(targetTower, true);
        return true;
    }

    public void PopupCombFailedUI(GameObject combFailedUI)
    {
        audioManager.Play("FailedPopUp");
        StartCoroutine(IsInvalidComb(combFailedUI));
    }
    WaitForSeconds UIPopUpDuration = new WaitForSeconds(1f);
    public IEnumerator IsInvalidComb(GameObject combFailedUI)
    {
        combFailedUI.SetActive(true);
        // 0.2초 대기
        yield return UIPopUpDuration;

        // 원래의 머티리얼로 복원
        combFailedUI.SetActive(false);
    }
    #endregion

    #region HiddenComb
    public void HiddenComb(TowerUnit selectedTower, Tower selectedHiddenTower)
    {
        var partsList = FindHiddenPartsList(selectedTower, selectedHiddenTower);
        selectedTower.gameObject.GetComponent<UI_TowerInfoTrigger>().HideTowerAtkRange();
        foreach (TowerUnit part in partsList)
        {
            RemoveTower(part, true);
        }
        if (playData.checkDistinct(selectedHiddenTower.code))
        {
            playData.AddData(selectedHiddenTower.code);
        }
    }

    private IEnumerable<TowerUnit> FindHiddenPartsList(TowerUnit selectedTower, Tower selectedHiddenTower)
    {
        int recipeIdx = 0;
        List<int> hiddenRecipe = new();
        hiddenRecipe = selectedHiddenTower.hidden.ToList();
        List<TowerUnit> resultList = new();

        resultList.Add(selectedTower);
        var selectedTowerIdx = hiddenRecipe.IndexOf(selectedTower.Tower.code);
        hiddenRecipe.RemoveAt(selectedTowerIdx);

        List<Pair<int,int>> currentSummonedTowerCode = FindCurrentTowerCode();
        currentSummonedTowerCode.Sort((x, y) => x.First.CompareTo(y.First));
        for (int i = 0; i < currentSummonedTowerCode.Count; i++)
        {
            if (recipeIdx == hiddenRecipe.Count) return resultList;
            if (currentSummonedTowerCode[i].Second == selectedTower.ID) continue;
            if (hiddenRecipe[recipeIdx] == currentSummonedTowerCode[i].First)
            {
                recipeIdx++;
                resultList.Add(towerUnits.FirstOrDefault(I => I.ID == currentSummonedTowerCode[i].Second));
            }
        }
        return resultList;
    }

    public List<Tower> FindHiddenCombTower(TowerUnit selectedTower)
    {
        List<Tower> canCombList = new List<Tower>();
        List<Pair<int,int>> currentSummonedTowerCode = FindCurrentTowerCode();
        var selectedTowerCode = selectedTower.Tower.code;

        foreach(Tower hiddenTower in HiddenTowers)
        {
            if (!hiddenTower.hidden.Contains(selectedTowerCode)) continue;
            if(CanHiddenComb(hiddenTower.hidden.list, currentSummonedTowerCode)) canCombList.Add(hiddenTower);
        }

        if(canCombList.Count <= 0) return null;

        return canCombList;
    }
    //first : Tower.code
    //second : towerUnit.ID
    private List<Pair<int, int>> FindCurrentTowerCode()
    {
        List<Pair<int, int>> result = new();
        foreach (TowerUnit towerUnit in towerUnits)
        {
            result.Add(new Pair<int, int>(towerUnit.Tower.code, towerUnit.ID));
        }
        return result;
    }

    private bool CanHiddenComb(List<int> hiddenRecipe, List<Pair<int, int>> currentSummonedTowerCode)
    {
        hiddenRecipe.Sort();
        currentSummonedTowerCode.Sort((x, y) => x.First.CompareTo(y.First));
        int hiddenIdx = 0;
        int currentSummonedIdx = 0;

        if (currentSummonedTowerCode.Count <= 0 || hiddenRecipe.Count <= 0) return false;

        while(hiddenRecipe.Count > hiddenIdx)
        {
            if (currentSummonedIdx >= currentSummonedTowerCode.Count) return false;
            if (hiddenRecipe[hiddenIdx] == currentSummonedTowerCode[currentSummonedIdx].First)
            {
                hiddenIdx++;
                currentSummonedIdx++;
            }
            else
            {
                currentSummonedIdx++;
            }
        }
        return true;
    }
    #endregion

    #region RemoveTower
    private void RemoveTower(TowerUnit towerUnit, bool isTileFree=false)
    {
        towerUnits.Remove(towerUnit);
        if(isTileFree) towerPlacer.FreeTile(towerUnit.currentPlace.ToVector2Int(), new Vector2Int(2, 2)); 
        Destroy(towerUnit.gameObject);
    }

    public void SellTower(TowerUnit towerUnit)
    {
        var towerPrice = towerUnit.Tower.price;
        stagePlayData.gold += towerPrice / 2;
        RemoveTower(towerUnit, true);
    }
    #endregion
}
