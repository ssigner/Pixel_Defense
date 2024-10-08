using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;


public class TowerPlacer : DIMono
{
    public Tilemap placerableTilemap;
    public SpriteRenderer previewTower;
    public Tile originalTile;
    
    public GameObject ShouldHiddenPlaceUI;
    public GameObject PlaceFailedUI;

    public GameObject menuUI;
    [Inject]
    TowerManager towerManager;
    [Inject]
    StagePlayData stagePlayData;
    [Inject]
    AudioManager audioManager;

    Tower selectedTower;

    Camera mainCam;
    GameObject towerPrefab;
    protected override void Init()
    {
        base.Init();
        mainCam = Camera.main;
    }

    float startTime;

    List<Vector3Int> tileInterval = new List<Vector3Int>()
    {
        new Vector3Int(1, 0), new Vector3Int(-1, 0),
        new Vector3Int(0, 1), new Vector3Int(0, -1),
        new Vector3Int(1, 1), new Vector3Int(-1, -1),
        new Vector3Int(-1, 1), new Vector3Int(1, -1)
    };


    IEnumerable<Vector2Int> IterTiles(Vector2Int startPos, Vector2Int size)
    {
        int i, j;
        for (i = 0;i< size.x; i++)
        {
            for (j = 0; j < size.y; j++)
            {
                yield return startPos + new Vector2Int(i,-j);
            }
        }
    }

    bool canPlace(Vector2Int startPos, Vector2Int size)
    {
        foreach(var pos in IterTiles(startPos, size))
        {
            if(placerableTilemap.HasTile(pos.ToVector3Int()) == false)
            {
                return false;
            }
        }
        return true;

    }

    public void freeTile(Vector2Int startPos, Vector2Int size)
    {
        foreach(var pos in IterTiles(startPos, size))
        {
            placerableTilemap.SetTile(pos.ToVector3Int(), originalTile);
        }
    }

    internal void StartPlace(Tower tower, Sprite sprite)
    {
        selectedTower = tower;
        previewTower.sprite = sprite;
        previewTower.gameObject.SetActive(true);
        startTime = Time.time;

    }

    public bool IsPreviewTowerVisible()
    {
        return previewTower.gameObject.activeSelf;
    }

    #region Popup UI
    void ShowShouldHiddenPlaceUI()
    {
        ShouldHiddenPlaceUI.SetActive(true);
    }
    void HideShouldHiddenPlaceUII()
    {
        ShouldHiddenPlaceUI.SetActive(false);
    }
    void popupShouldHiddenPlaceUI()
    {
        StartCoroutine(IsHiddenPlace());
    }
    IEnumerator IsHiddenPlace()
    {
        ShowShouldHiddenPlaceUI();
        // 1초 대기
        yield return UIPopUpDuration;

        HideShouldHiddenPlaceUII();
    }

    void ShowPlaceFailedUI()
    {
        PlaceFailedUI.SetActive(true);
    }
    void HidePlaceFailedUI()
    {
        PlaceFailedUI.SetActive(false);
    }
    void popupPlaceFailedUI()
    {
        StartCoroutine(IsInvalidPlace());
    }
    IEnumerator IsInvalidPlace()
    {
        ShowPlaceFailedUI();
        // 1초 대기
        yield return UIPopUpDuration;

        HidePlaceFailedUI();
    }
    #endregion

    WaitForSeconds UIPopUpDuration = new WaitForSeconds(1f);

    private void Update()
    {
        
        if (selectedTower == null)
            return;
        towerPrefab = Addressables.LoadAssetAsync<GameObject>(selectedTower.prefabPath).WaitForCompletion();

        if (Input.GetMouseButtonUp(1) && (selectedTower.grade != Tower.Grade.Hidden && selectedTower.grade != Tower.Grade.HyperHidden))
        {
            selectedTower = null;
            previewTower.gameObject.SetActive(false);
            menuUI.SetActive(true);
        }

        else if (Input.GetMouseButtonUp(1) && (selectedTower.grade == Tower.Grade.Hidden || selectedTower.grade == Tower.Grade.HyperHidden))
        {
            popupShouldHiddenPlaceUI();
        }


       Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
       var cellPos= placerableTilemap.WorldToCell(mousePosition);
       var cellWordPos= placerableTilemap.CellToWorld(cellPos);

        cellWordPos.z = previewTower.transform.position.z;
        var offset = new Vector3(placerableTilemap.cellSize.x, 0);
        previewTower.transform.position = cellWordPos + offset;

        var towerSize = new Vector2Int(2, 2);
        var isPlaceable = canPlace(cellPos.ToVector2Int(), towerSize);//placerableTilemap.HasTile(cellPos);

        if (isPlaceable)
        {
            previewTower.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {

            previewTower.color = new Color(1,0,0,0.5f);
        }
        
        //Summon Tower
        if (Time.time - startTime > 0.3f && Input.GetMouseButtonUp(0) && isPlaceable)
        {
            audioManager.Play("PlaceTower");
            towerManager.summonTower(selectedTower, cellPos, towerPrefab);
            if(selectedTower.grade == Tower.Grade.Normal) stagePlayData.gold -= selectedTower.price;
            else if(selectedTower.grade == Tower.Grade.Rare || selectedTower.grade == Tower.Grade.Unique) stagePlayData.emelard -= selectedTower.hyperPrice;
            foreach (var pos in IterTiles(cellPos.ToVector2Int(), towerSize))
            {
                placerableTilemap.SetTile(pos.ToVector3Int(), null);
            }

            selectedTower = null;
            previewTower.gameObject.SetActive(false);
            menuUI.SetActive(true);
        }
        else if(Time.time - startTime > 0.3f && Input.GetMouseButtonUp(0) && !isPlaceable)
        {
            popupPlaceFailedUI();
        }
    }
}
