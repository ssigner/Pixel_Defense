using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TutorialOnOff : MonoBehaviour
{
    public GameObject tutorialUI;
    public GameObject[] steps;
    public void Click()
    {
        if (tutorialUI.activeSelf)
        {
            tutorialUI.SetActive(false);
        }
        else
        {
            tutorialUI.SetActive(true);
            steps[0].SetActive(true);
            for (int i = 1; i < steps.Length; i++)
            {
                steps[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && tutorialUI.activeSelf)
        {
            tutorialUI.SetActive(false); // esc Ű�� ������ UI�� ��Ȱ��ȭ
        }
    }
}
