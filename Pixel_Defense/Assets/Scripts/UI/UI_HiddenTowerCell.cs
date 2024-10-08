using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UI_HiddenTowerCell : DIMono
{
    public Image towerImg;
    public GameObject hiddenTowerUI;
    public GameObject menuUI;

    [Inject]
    TowerPlacer TowerPlacer;

    [Inject]
    UI_TowerInfo UI_TowerInfo;

    [Inject]
    TowerManager towerManager;

    [Inject]
    AudioManager audioManager;

    Tower tower;

    internal void SetData(Tower tower)
    {
        CheckInject();

        this.tower = tower;
        towerImg.sprite = Addressables.LoadAssetAsync<Sprite>(tower.spritePath).WaitForCompletion();
    }


    public void Click()
    {
        audioManager.Play("BtnClickAndBuyUnit");
        var selectedTower = UI_TowerInfo.towerUnit;
        menuUI.SetActive(false);
        TowerPlacer.StartPlace(tower, towerImg.sprite);
        Debug.Log("selected hidden Tower : " + tower.name);
        towerManager.hiddenComb(selectedTower, tower);

        UI_TowerInfo.isHiddenBtnClicked = false;
        hiddenTowerUI.gameObject.SetActive(false);
        UI_TowerInfo.Hide();
    }
}

