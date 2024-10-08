using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIMono : MonoBehaviour
{

    protected virtual void Init()
    {

    }

    public void CheckInject()
    {
        InjectObj.CheckInject(this);
    }

    InjectObj InjectObj = new InjectObj();

    // Start is called before the first frame update
    void Start()
    {
        CheckInject();
        Init();
    }}
