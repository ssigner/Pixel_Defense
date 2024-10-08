using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TutorialNextPrevBtn : MonoBehaviour
{
    public GameObject nextStepView;

    public void Click()
    {
        nextStepView.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
}
