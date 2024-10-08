using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BtnSfx : DIMono
{
    [Inject]
    AudioManager audioManager;

    public enum BtnSound
    {
        BtnClickAndBuyUnit,
        NormalComb,
        SellTower,
        HiddenComb,
    }

    public BtnSound btnSound;


    string GetBtnSoundName()
    {
        return btnSound.ToString();
    }

    public void Click()
    {
        audioManager.Play(GetBtnSoundName());
    }

    protected override void Init()
    {
        var btn = this.GetComponent<Button>();

        btn.onClick.AddListener(() =>
        {
            Click();
        });
    }
}
