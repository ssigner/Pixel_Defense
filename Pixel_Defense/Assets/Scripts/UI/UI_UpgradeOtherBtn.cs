using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_UpgradeOtherBtn : DIMono
{
    [Inject]
    GameData gameData;

    [Inject]
    StagePlayData stagePlayData;

    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI upgradeLevel;
    public GameObject upgradeFailedUI;
    public GameObject maxLevelUI;

    public enum UpgradeType
    {
        Human,
        Spirit,
        notHuman,
        Priest
    }
    public UpgradeType upgradeType;

    List<UpgradeRow> GetUpgradeRowList()
    {
        switch (upgradeType)
        {
            case UpgradeType.Human:
                return gameData.upgradeHuman;
            case UpgradeType.Spirit:
                return gameData.upgradeSpirit;
            case UpgradeType.notHuman:
                return gameData.upgradeNotHuman;
            case UpgradeType.Priest:
                return gameData.upgradePriest;
        }

        return null;
    }
    int getUpgradeLevel()
    {
        switch (upgradeType)
        {
            case UpgradeType.Human:
                return stagePlayData.humanLevel;
            case UpgradeType.Spirit:
                return stagePlayData.spiritLevel;
            case UpgradeType.notHuman:
                return stagePlayData.notHumanLevel;
            case UpgradeType.Priest:
                return stagePlayData.priestLevel;
        }
        return 0;
    }

    void plusUpgradeLevel()
    {
        switch (upgradeType)
        {
            case UpgradeType.Human:
                stagePlayData.humanLevel++;
                return;
            case UpgradeType.Spirit:
                stagePlayData.spiritLevel++;
                return;
            case UpgradeType.notHuman:
                stagePlayData.notHumanLevel++;
                return;
            case UpgradeType.Priest:
                stagePlayData.priestLevel++;
                return;
        }
    }

    public void Click()
    {
        var nextUpgrade = GetUpgradeRowList().FindByLevel(getUpgradeLevel() + 1);
        if (nextUpgrade == null) return;
        if(stagePlayData.iron < nextUpgrade.cost)
        {
            popupUpgradeFailedUI();
            return;
        }
        stagePlayData.iron -= nextUpgrade.cost;
        plusUpgradeLevel();
        UpdateContents();

    }

    void UpdateContents()
    {
        var nextUpgrade = GetUpgradeRowList().FindByLevel(getUpgradeLevel() + 1);
        if (nextUpgrade == null)
        {
            popupMaxLevelUI();
            upgradeLevel.text = "MAX";
            return;
        }
        upgradeCost.text = nextUpgrade.cost.ToString();
        upgradeLevel.text = getUpgradeLevel().ToString();
    }

    void ShowFailedUI()
    {
        upgradeFailedUI.SetActive(true);
    }
    void HideFailedUI()
    {
        upgradeFailedUI.SetActive(false);
    }
    void popupUpgradeFailedUI()
    {
        StartCoroutine(IsInvalidUpgrade());
    }
    WaitForSeconds UIPopUpDuration = new WaitForSeconds(1f);
    IEnumerator IsInvalidUpgrade()
    {
        ShowFailedUI();
        // 1초 대기
        yield return UIPopUpDuration;

        // 원래의 머티리얼로 복원
        HideFailedUI();
    }

    void ShowMaxLevelUI()
    {
        maxLevelUI.SetActive(true);
    }
    void HideMaxLevelUI()
    {
        maxLevelUI.SetActive(false);
    }
    void popupMaxLevelUI()
    {
        StartCoroutine(IsMaxLevelUpgrade());
    }
    IEnumerator IsMaxLevelUpgrade()
    {
        ShowMaxLevelUI();
        // 1초 대기
        yield return UIPopUpDuration;

        // 원래의 머티리얼로 복원
        HideMaxLevelUI();
    }
}
