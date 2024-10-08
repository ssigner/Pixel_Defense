using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

[System.Serializable]
public class Tower
{
    public int code;
    public string name;
    public int price;
    public int hyperPrice;
    public TowerClass towerClass;
    public float atk;
    public float atkSpeed;
    public float atkRange;
    public string prefabPath;
    public string spritePath;
    public string typeSpritePath;
    public string projectilePrefabPath;
    public IntList hidden;
    public string recipe;
    public Skill skill;

    public enum Grade
    {
        Normal,
        Rare,
        Unique,
        Hidden,
        HyperHidden,
    }

    public Grade grade;

    public enum TowerClass 
    {
        human,
        priest,
        spirit,
        not_human,
    }

    public float GetState(Status stat)
    {
        switch (stat)
        {
            case Status.Atk:
                return atk;
            case Status.AtkSpeed:
                return atkSpeed;
        }
        return 0;
    }
}
public enum Status
{
    Atk,
    AtkSpeed,
    ExtraPriestBuffVal
}
[System.Serializable]
public class Buff
{
    public TowerUnit caster;
    public Status targetStatus;
    public float val;

    public float leftTime;
}

public interface IFillFromStr
{
    void fillFromStr(string str);

}

[System.Serializable]
public class RangedValue : IFillFromStr
{
    public float min, max;

    public void fillFromStr(string str)
    {
        var strArr=str.Trim().Split(":");
        
        min = float.Parse(strArr[0]);
        max = float.Parse(strArr[1]);
    }
}

[System.Serializable]
public class Monster
{
    public int code;
    public string name;
    public float hp;
    public int def;
    public float speed;
    public string prefabPath;
    public float scale;
    public int atk;
    public float mobGageY;
    public float gageScale;
}

[System.Serializable]
public class Stage
{
    public int code;
    public int mobCode;
    public int mobNum;

    public Monster monster;
}

[System.Serializable]
public class Skill
{
    public int code;
    public string imagePath;
    public int coolTime;
    public string script;
    public enum Fx
    {
        selfBuff,
        attack,
        passiveBuff,
        passiveGold,
        passiveIron,
        passiveDebuff,
        debuff,
        percentAttack,
        passiveEmelard,
        none,
    }

    public Fx fx;
    public enum Target 
    {
        self,
        mobRange,
        towerRange,
        oneTarget,
        none,
    }

    public Target target;

    public bool isPercentage;
    public FloatList fxParam;

    public enum BuffType
    {
        none,
        atk,
        atkSpeed,
    }

    public BuffType buffType;

    public Status GetStatusFromBuffType()
    {
        switch (buffType)
        {
            case BuffType.none:
            case BuffType.atk:
                return Status.Atk;
            case BuffType.atkSpeed:

                return Status.AtkSpeed;
        }

        return Status.Atk;
    }

    public FloatList targetParam;
    public int percentParam;

}

[System.Serializable]
public class IntList : IFillFromStr, IEnumerable<int>
{
    public List<int> list;

    public int this[int index]
    {
        get
        {
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }
            throw new IndexOutOfRangeException("Index is out of range.");
        }
        set
        {
            if (index >= 0 && index < list.Count)
            {
                list[index] = value;
            }
            else
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
        }
    }

    public void fillFromStr(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            list = new List<int>();
            return;
        }

        var strArr = str.Trim().Split(",");
        list = new List<int>();

        foreach (var s in strArr)
        {
            list.Add(int.Parse(s));
        }
    }

    public IEnumerator<int> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
[System.Serializable]
public class FloatList : IFillFromStr, IEnumerable<float>
{
    public List<float> list;

    public float this[int index]
    {
        get
        {
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }
            throw new IndexOutOfRangeException("Index is out of range.");
        }
        set
        {
            if (index >= 0 && index < list.Count)
            {
                list[index] = value;
            }
            else
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
        }
    }

    public void fillFromStr(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            list = new List<float>();
            return;
        }

        var strArr = str.Trim().Split(",");
        list = new List<float>();

        foreach (var s in strArr)
        {
            list.Add(float.Parse(s));
        }
    }

    public IEnumerator<float> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
[System.Serializable]
public class UpgradeRow
{
    public int level;
    public float val;
    public int cost;
}
[System.Serializable]
public class tutorialScript
{
    public int step;
    public string script;
    public bool isButtonHide;
    public bool hideTransImg;

}