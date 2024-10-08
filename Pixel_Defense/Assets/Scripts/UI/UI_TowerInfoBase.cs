using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class TowerInfoFrame
{
    public Tower.Grade grade;
    public Sprite mainFrame, subFrame, towerFrame;
}

[Serializable]
public class TowerInfoAbilityType
{
    public Tower.TowerClass towerClass;
    public Sprite icon;
}


[Serializable]
public class TowerInfoUIResouces
{
    public TowerInfoFrame[] frames;

    public TowerInfoAbilityType[] towerInfoAbilityTypes;


    public TowerInfoFrame GetTowerInfoFrame(Tower.Grade grade)
    {
        return frames.FirstOrDefault(l => l.grade == grade);
    }

    public Sprite GetTowerTypeSprite(Tower.TowerClass towerClass)
    {
        return towerInfoAbilityTypes.FirstOrDefault(l => l.towerClass == towerClass).icon;
    }
}

public class UI_TowerInfoBase : DIMono
{
    [Inject]
    public GameData gameData;

    [Inject]
    public PlayData playData;

    public TowerInfoUIResouces towerInfoResouces;
    public Image towerFrame;
    public Image towerImage;
    public TextMeshProUGUI towerName;

    public Image mainFrame, subFrame;
    public UI_TowerAbilityCell typeCell, statCell, skillCell, recipeCell;
    public Sprite statImg;

    public Tower selectedTower;
    public TowerUnit towerUnit;

    public UI_TowerStatusTooltip StatUI;
    public UI_TowerTypeTooltip TypeUI;
    public UI_TowerSkillTooltipDictionary SkillUI;
    public UI_TowerRecipeTooltip RecipeUI;
    public Skill skill;
    public virtual void SetTower(Tower tower)
    {
        CheckInject();
        setSprite(tower);
        towerName.text = tower.name;
        skill = gameData.skills.FirstOrDefault(I => I.code == tower.code);

        typeCell.SetMouseEnterCallback(OnTowerTypePointerEnter);
        typeCell.SetMouseExitCallback(OnTowerTypePointerExit);

        statCell.SetMouseEnterCallback(OnTowerStatPointerEnter);
        statCell.SetMouseExitCallback(OnTowerStatPointerExit);

        skillCell.SetMouseEnterCallback(OnTowerSkillPointerEnter);
        skillCell.SetMouseExitCallback(OnTowerSkillPointerExit);

        recipeCell.SetMouseEnterCallback(OnTowerRecipePointerEnter);
        recipeCell.SetMouseExitCallback(OnTowerRecipePointerExit);
    }

    

    protected void setSprite(Tower tower)
    {
        selectedTower = tower;
        var tRes = towerInfoResouces.GetTowerInfoFrame(tower.grade);
        mainFrame.sprite = tRes.mainFrame;
        subFrame.sprite = tRes.subFrame;
        var towerImg = Addressables.LoadAssetAsync<Sprite>(tower.spritePath).WaitForCompletion();
        towerImage.sprite = towerImg;
        towerFrame.sprite = tRes.towerFrame;

        var skillImg = Addressables.LoadAssetAsync<Sprite>(tower.skill.imagePath).WaitForCompletion();
        skillCell.SetImage(skillImg);

        var typeImg = Addressables.LoadAssetAsync<Sprite>(tower.typeSpritePath).WaitForCompletion();
        typeCell.SetImage(typeImg);

        statCell.SetImage(statImg);
    }
    
    protected virtual void OnTowerSkillPointerExit(PointerEventData pointerEventData)
    {
        SkillUI.Hide();
    }

    protected virtual void OnTowerSkillPointerEnter(PointerEventData pointerEventData)
    {
        SkillUI.setStatusText(selectedTower);
        SkillUI.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        SkillUI.Show();
    }

    protected void OnTowerStatPointerExit(PointerEventData pointerEventData)
    {
        StatUI.Hide();
    }

    protected virtual void OnTowerStatPointerEnter(PointerEventData pointerEventData)
    {
        StatUI.setStatusText(selectedTower);
        StatUI.Show();
    }

    protected void OnTowerTypePointerExit(PointerEventData pointerEventData)
    {
        TypeUI.Hide();
    }

    protected void OnTowerTypePointerEnter(PointerEventData pointerEventData)
    {
        TypeUI.setStatusText(selectedTower);
        TypeUI.Show();
    }
    protected void OnTowerRecipePointerEnter(PointerEventData pointerEventData)
    {
        RecipeUI.setStatusText(selectedTower);
        RecipeUI.Show();
    }

    protected void OnTowerRecipePointerExit(PointerEventData pointerEventData)
    {
        RecipeUI.Hide();
    }

}

