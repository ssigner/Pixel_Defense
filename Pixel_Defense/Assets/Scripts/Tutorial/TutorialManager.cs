using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TutorialManager : DIMono
{
    [Inject]
    GameData gameData;

    public int scriptIdx;
    public int currentScriptIdx;
    public int step;
    public bool goToNextScript;

    public TextMeshProUGUI tutorialtext;
    private List<tutorialScript> tutorialScripts;

    public GameObject nextBtn;
    public GameObject FirstView;
    public GameObject transImg;

    public GameObject tutorialFrame;
    public List<GameObject> menuBtns;
    const float delay = 1.0f;
    public bool trigger;
    protected override void Init()
    {
        base.Init();
        if (SceneManager.GetActiveScene().name != "Tutorial") gameObject.SetActive(false);
        scriptIdx = 0;
        currentScriptIdx = 0;
        step = 0;
        trigger = false;
        FirstView.SetActive(true);
        tutorialScripts = gameData.tutorialScript;
    }
    private void SetText()
    {
        string script = tutorialScripts[scriptIdx].script;
        tutorialtext.text = StringParser.SetText(script);
    }
    private void HideButton()
    {
        nextBtn.SetActive(false);
    }
    private void HideFrame()
    {
        tutorialFrame.SetActive(false);
    }
    private void ShowFrame()
    {
        tutorialFrame.SetActive(true);
    }
    private void ShowButton()
    {
        nextBtn.SetActive(true);
    }
    private void HideTransImg()
    {
        transImg.SetActive(false);
    }
    private void ShowTransImg()
    {
        transImg.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (scriptIdx.Equals(currentScriptIdx)) return; 
        SetText();
        ShowFrame();
        if (tutorialScripts[scriptIdx].isButtonHide)
        {
            HideButton();
            Invoke("HideFrame", delay);
        }
        else ShowButton();

        ShowTransImg();
        if(tutorialScripts[scriptIdx].hideTransImg)
        {
            Invoke("HideTransImg", delay);
        }
        else ShowTransImg();
        step = tutorialScripts[scriptIdx].step;
        currentScriptIdx++;
    }
}
