using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGage : MonoBehaviour
{

    public SpriteRenderer spRenderer;

    Mob mob = null;

    const float startU = 0.168f;
    const float endU = 0.263f;
    public float hpRatio;
    public List<Mob> mobs = new List<Mob>();
    Material gageMat;
    private void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
        //0.168
        //0.263
        gageMat = spRenderer.material;

        mob = transform.parent.parent.GetComponent<Mob>();

    }
    float calcHpRatio(float mobHp)
    {
        var result = startU;
        result += (endU - startU) * (mobHp / mob.Monster.hp);
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        hpRatio = calcHpRatio(mob.hp);

        gageMat.SetFloat("_endU", hpRatio);
    }
}
