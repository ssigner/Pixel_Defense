using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTowerDictionary : DIMono
{
    [Inject]
    SceneChanger sceneChanger;
    public void Click()
    {
        sceneChanger.LoadScene("TowerDictionary", SceneChanger.LoadingScene.DiamondPattern);
    }
}
