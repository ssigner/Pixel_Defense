using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using System;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System.Reflection;
using System.Linq;


[Serializable]
public class GameData :ScriptableObject
{
    public List<Tower> towers;
    public List<Monster> monsters;


}
