using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    private void ShowFailedUI(GameObject UI)
    {
        UI.SetActive(true);
    }
    private void HideFailedUI(GameObject UI)
    {
        UI.SetActive(false);
    }
    public void Popup(GameObject UI, WaitForSeconds UIPopUpDuration)
    {
        StartCoroutine(PopupCoroutine(UI, UIPopUpDuration));
    }
    IEnumerator PopupCoroutine(GameObject UI, WaitForSeconds UIPopUpDuration)
    {
        ShowFailedUI(UI);
        // 1초 대기
        yield return UIPopUpDuration;

        // 원래의 머티리얼로 복원
        HideFailedUI(UI);
    }

}

