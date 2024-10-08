using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UIOnOffBtn : MonoBehaviour
{
    public GameObject targetUI;

    public void Click()
    {
        if (targetUI.activeSelf) targetUI.SetActive(false);
        else targetUI.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && targetUI.activeSelf)
        {
            targetUI.SetActive(false); // esc 키를 누르면 UI를 비활성화
        }
    }
}
