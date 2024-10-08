using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class TutorialBase : DIMono
{
    [SerializeField]
    protected GameObject NextStep;
    protected abstract void Enter();
    protected abstract void Execute();
    protected abstract void Exit();
}
