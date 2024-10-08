using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DiamondPattern : MonoBehaviour
{
    public Material material;

    [Range(0,1)]
    public float progress;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_Progress", progress);
    }
}
