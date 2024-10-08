using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Money : DIMono
{
    [Inject]
    StagePlayData stagePlayData;

    int preGold;
    int preEmerald;
    int preIron;
    public TextMeshProUGUI displayGold;
    public TextMeshProUGUI displayEmerald;
    public TextMeshProUGUI displayIron;

    public GameObject changeGold;
    public GameObject changeEmerald;
    public GameObject changeIron;

    TextMeshProUGUI changeGoldText;
    TextMeshProUGUI changeEmeraldText;
    TextMeshProUGUI changeIronText;
    protected override void Init()
    {
        CheckInject();
        preGold = stagePlayData.gold;
        preEmerald = stagePlayData.emelard;
        preIron = stagePlayData.iron;
        displayGold.text = stagePlayData.gold.ToString();
        displayEmerald.text = stagePlayData.emelard.ToString();
        displayIron.text = stagePlayData.iron.ToString();
        changeGoldText = changeGold.GetComponentInChildren<TextMeshProUGUI>();
        changeEmeraldText = changeEmerald.GetComponentInChildren<TextMeshProUGUI>();
        changeIronText = changeIron.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(preGold != stagePlayData.gold)
        {
            var currentGold = stagePlayData.gold;
            setChangeMoneyText(changeGoldText, preGold, currentGold);
            popupChangeMoneyUI(changeGold);
            displayGold.text = currentGold.ToString();
            preGold = currentGold;
        }
        if(preEmerald != stagePlayData.emelard)
        {
            var currentEmerald = stagePlayData.emelard;
            displayEmerald.text = currentEmerald.ToString();
            setChangeMoneyText(changeEmeraldText, preEmerald, currentEmerald);
            popupChangeMoneyUI(changeEmerald);
            preEmerald = currentEmerald;
        }
        if(preIron != stagePlayData.iron)
        {
            var currentIron = stagePlayData.iron;
            displayIron.text = currentIron.ToString();
            setChangeMoneyText(changeIronText, preIron, currentIron);
            popupChangeMoneyUI(changeIron);
            preIron = currentIron;
        }
    }

    void setChangeMoneyText(TextMeshProUGUI text, int preValue, int currentValue)
    {
        if (preValue < currentValue)
        {
            text.text = "+ " + (currentValue - preValue).ToString();
        }
        else
        {
            text.text = "- " + (preValue - currentValue).ToString();
        }
    }

    void popupChangeMoneyUI(GameObject changeMoneyUI)
    {
        StartCoroutine(IsInvalidExchange(changeMoneyUI));
    }
    WaitForSeconds UIPopUpDuration = new WaitForSeconds(0.5f);
    IEnumerator IsInvalidExchange(GameObject changeMoneyUI)
    {
        changeMoneyUI.SetActive(true);
        // 1초 대기
        yield return UIPopUpDuration;

        // 원래의 머티리얼로 복원
        changeMoneyUI.SetActive(false);
    }

}
