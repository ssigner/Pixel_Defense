using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DIMono
{

    [Inject]
    ObjectPoolManager pool;

    Mob targetMob;
    int mobId;

    Vector3 targetMobPrePos;

    float speed;
    TowerUnit towerUnit;


    GameObject visualObj;

    public void SetVisualObj()
    {

        visualObj = pool.GetObjectFromPool(towerUnit.Tower.projectilePrefabPath);
        visualObj.transform.SetParent(this.transform);
        visualObj.transform.localPosition = Vector3.zero;

    }

    public void SetTowerUnit(TowerUnit towerUnit)
    {
        CheckInject();
        this.towerUnit = towerUnit;
        SetVisualObj();
        speed = 20;
    }
    

    public void SetTarget(Mob mob)
    {
        mobId = mob.ID;
        targetMob = mob;
        targetMobPrePos = targetMob.transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (towerUnit.Tower.towerClass == Tower.TowerClass.priest) return;
        Vector3 targetPos;

        if(mobId== targetMob.ID)
        {
            targetMobPrePos = targetMob.transform.position;
        }
        targetPos = targetMobPrePos;

        Vector3 distVec = targetMobPrePos - this.transform.position;
         var movDist= distVec.normalized*speed *Time.deltaTime;


        var r= Mathf.Atan2(movDist.y, movDist.x) *Mathf.Rad2Deg;
        visualObj.transform.rotation = Quaternion.Euler(0, 0, r);


        if(movDist.sqrMagnitude  > distVec.sqrMagnitude)
        {
            //µµÂø
            //¸÷¿¡°Ô µ¥¹ÌÁö Áà¾ßÇÔ.

            ReturnToPool();
            return;
        }

        this.transform.position += movDist;
    }

    void ReturnToPool()
    {

        pool.ReturnObjectToPool(this.visualObj);
        pool.ReturnObjectToPool(this.gameObject);

    }
}
