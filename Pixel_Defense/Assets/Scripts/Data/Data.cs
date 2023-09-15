

using UnityEngine;

[System.Serializable]
public class Tower
{
    public int code;
    public string name;
    public int price;


    public enum TowerType
    {
        Melee,
        Raged
    }
    public TowerType towerType;

    public RangedValue atkRange;
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
    public int hp;
    public int def;
    public float speed;
    public string prefabPath;
    public float scale;
}