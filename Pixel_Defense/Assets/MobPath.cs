using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MobPath : MonoBehaviour
{
    public List<PathPoint> pathPoints = new List<PathPoint>();

    
    void OnEnable()
    {
        var tf = this.transform;

        pathPoints.Clear();
        
        foreach (Transform pathTf in tf)
        {
            var pp = pathTf.GetComponent<PathPoint>();

            pathPoints.Add(pp);
        }        
    }

}
