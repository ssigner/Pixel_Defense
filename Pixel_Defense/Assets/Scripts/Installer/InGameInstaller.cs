using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameInstaller : MonoBehaviour
{
    public ObjectPoolManager poolManager;
    public MobPath mobPath;

    private void Awake()
    {
        var container= new DIContainer(); ;
        DIContainer.Current = container;


        container.Regist(new StagePlayData());

        container.Regist(poolManager);
        container.Regist(mobPath);



    }

}
