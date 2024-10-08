using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NoExitBtn : MonoBehaviour
{
    public GameObject ExitUI;
    public void Click()
    {
        ExitUI.SetActive(false);
    }
}
