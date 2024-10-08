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
    public List<Stage> stages;
    public List<Skill> skills;
    public List<UpgradeRow> upgradePriest;
    public List<UpgradeRow> upgradeHuman;
    public List<UpgradeRow> upgradeSpirit;
    public List<UpgradeRow> upgradeNotHuman;
    public List<tutorialScript> tutorialScript;



    public Monster findMonster(int targetMobCode)
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i].code== targetMobCode)
            {
                return monsters[i];
            }
        }
        return null;
    }

    public Skill findSkill(int skillCode)
    {

        return skills.FirstOrDefault(l => l.code == skillCode);
    }


    public void Initialize()
    {

        foreach(var s in stages)
        {
            s.monster = findMonster(s.mobCode);
        }

        foreach(var t in towers)
        {
            t.skill = findSkill(t.code);
        }

    }
}


public static class GameDataExt
{

    public static UpgradeRow FindByLevel(this List<UpgradeRow> list, int level)
    {
        return list.Find(I => I.level == level);
    }
}