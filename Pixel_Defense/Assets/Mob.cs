using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : DIMono
{
    [Inject]
    ObjectPoolManager poolManager;

    [Inject]
    MobPath mobPath;

    public GameObject visualPrefab;

    public float speed;


    int mobPathIndex;
    public void SetData(Monster monster)
    {
        CheckInject();

        string prefabKey = monster.prefabPath;
        visualPrefab = poolManager.GetObjectFromPool(prefabKey);

        this.transform.position= mobPath.pathPoints[0].transform.position; ;

        visualPrefab.transform.SetParent(this.transform);
        visualPrefab.transform.localPosition = Vector3.zero;
        visualPrefab.transform.localScale = monster.scale * Vector3.one;
        speed = monster.speed;
        mobPathIndex = 0;
    }


    private void Update()
    {

        Vector3 startPosition = mobPath.pathPoints[mobPathIndex].transform.position;
        Vector3 endPosition = mobPath.pathPoints[mobPathIndex + 1].transform.position;
        Vector3 dir = (endPosition - startPosition).normalized;

        float distance = Time.deltaTime * speed;
        Vector3 currentPosition = this.transform.position;
        float remainDistance = (endPosition - currentPosition).magnitude;

        if (distance >= remainDistance)
        {
            distance -= remainDistance;
            if (mobPathIndex + 2 == mobPath.pathPoints.Count)
            {
                ReturnToPool();
                return;
            }
            mobPathIndex++;
            this.transform.position = endPosition;
            startPosition = mobPath.pathPoints[mobPathIndex].transform.position;
            endPosition = mobPath.pathPoints[mobPathIndex + 1].transform.position;
            dir = (endPosition - startPosition).normalized;

        }

        var localScale = visualPrefab.transform.localScale;       

        localScale.x = Mathf.Sign(dir.x) * Mathf.Abs(localScale.x);

        visualPrefab.transform.localScale= localScale;

        this.transform.position += dir * distance;

    }


    public void ReturnToPool()
    {
        poolManager.ReturnObjectToPool(visualPrefab);
        poolManager.ReturnObjectToPool(this.gameObject);
    }

}
