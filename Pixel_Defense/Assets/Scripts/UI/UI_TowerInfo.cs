using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class UI_TowerInfo : UI_TowerInfoBase
{
    public Image skillCoolTime;
    public TextMeshProUGUI coolTimeSec;
    public bool isHiddenBtnClicked = false;
    public UI_TowerSkillTooltip inGameSkillUI;
    public GameObject HiddenTowerPlace;

    protected override void Init()
    {
    }

    public override void SetTower(Tower tower)
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

        if (skill.coolTime < 0 || skill == null)
        {
            coolTimeSec.gameObject.SetActive(false);
            skillCoolTime.gameObject.SetActive(false);
        }
        else
        {
            coolTimeSec.gameObject.SetActive(true);
            skillCoolTime.gameObject.SetActive(true);
        }

    }

    private void Update()
    {
        var leftTime = towerUnit.nextSkillTime - Time.time;
        coolTimeSec.text = leftTime.ToString("f1");

        var t = leftTime / skill.coolTime;
        skillCoolTime.fillAmount = t;
    }

    internal void Show(TowerUnit towerUnit)
    {
        this.towerUnit = towerUnit;
        SetTower(towerUnit.Tower);
        this.gameObject.SetActive(true);
    }

    internal bool IsVisible()
    {
        return this.gameObject.activeSelf;
    }

    internal void Hide()
    {
        HideHiddenPlace();
        this.towerUnit = null;
        this.gameObject.SetActive(false);
    }
    internal void HideHiddenPlace()
    {
        HiddenTowerPlace.SetActive(false);
    }
    protected override void OnTowerSkillPointerExit(PointerEventData pointerEventData)
    {
        inGameSkillUI.Hide();
    }

    protected override void OnTowerSkillPointerEnter(PointerEventData pointerEventData)
    {
        inGameSkillUI.setStatusText(selectedTower);
        inGameSkillUI.Show();
    }

    protected override void OnTowerStatPointerEnter(PointerEventData pointerEventData)
    {
        StatUI.setStatusText(towerUnit);
        StatUI.Show();
    }

}