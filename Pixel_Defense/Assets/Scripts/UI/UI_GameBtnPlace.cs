using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameBtnPlace : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UnitPlace;
    public GameObject[] otherUnitPlaces = new GameObject[1];
    public GameObject FailedUI;
    public void CheckPlaceOpen()
    {
        foreach (GameObject otherUnitPlace in otherUnitPlaces)
        {
            if (otherUnitPlace.gameObject.activeInHierarchy) otherUnitPlace.SetActive(false);
        }
        if (!UnitPlace.activeSelf)
        {
            UnitPlace.SetActive(true);
            if(FailedUI.activeSelf) FailedUI.SetActive(false);
        }
        else
        {
            UnitPlace.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UnitPlace.activeSelf)
        {
            UnitPlace.SetActive(false); // esc 키를 누르면 UI를 비활성화
        }
    }
}
