using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Mob : DIMono
{
    [Inject]
    ObjectPoolManager poolManager;

    [Inject]
    MobPath mobPath;

    [Inject]
    MobManager mobManager;

    [Inject]
    StagePlayData stagePlayData;

    public GameObject visualPrefab;
    public Transform mobGageTf;

    public float speed;
    public float hp;
    public List<int> debuffs = new();

    Monster _monster;
    public Monster Monster { get => _monster; }

    
    public int ID { get; private set; }
    private float preHP;
    int mobPathIndex;

    [Inject("SpriteDefault")]
    private Material originalMaterial;

    [Inject("Hit")]
    Material hitMaterial;

    private Renderer renderer;
    private bool isChanging = false;
    public void SetData(Monster monster,int id)
    {
        CheckInject();
        this.ID = id;
        preHP = hp;
        
        this._monster = monster;
        string prefabKey = monster.prefabPath;
        Debug.Log($"monster path : {prefabKey}");
        //머티리얼 등록
        visualPrefab = poolManager.GetObjectFromPool(prefabKey);
        renderer = visualPrefab.GetComponent<Renderer>();
        renderer.material = originalMaterial;
        //originalMaterial = renderer.material;
    
        this.hp = monster.hp;
        this.transform.position= mobPath.pathPoints[0].transform.position;

        mobGageTf.transform.localPosition = new Vector3(0, monster.mobGageY, 0);
        mobGageTf.transform.localScale = new Vector3(monster.gageScale, 3, 0);

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
                stagePlayData.userHp -= Monster.atk;
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

        if(preHP != hp)
        {
            StartCoroutine(IsAttacked());
            preHP = hp;
        }

    }
    WaitForSeconds hitFxDuration= new WaitForSeconds(0.2f);

    public void RemoveDebuff(float debuffTime, float debuffDegree, int debuffCode)
    {
        StartCoroutine(DebuffTime(debuffTime, debuffDegree, debuffCode));
    }

    IEnumerator DebuffTime(float debuffTime, float debuffDegree, int debuffCode)
    {
        yield return new WaitForSeconds(debuffTime);
        this.speed += debuffDegree;
        debuffs.Remove(debuffCode);
    }

    public bool hasThisDebuff(int code)
    {
        return debuffs.Contains(code);
    }

    public void addDebuff(int code)
    {
        debuffs.Add(code);
    }

    public IEnumerator IsAttacked()
    {
        isChanging = true;

        // 새로운 머티리얼로 변경
        renderer.material = hitMaterial;

        // 0.2초 대기
        yield return hitFxDuration;

        // 원래의 머티리얼로 복원
        renderer.material = originalMaterial;

        isChanging = false;
    }

    public void ReturnToPool()
    {

        Debug.Log($"ReturnToPool Stage{stagePlayData.currentStage.code} " + this.name);

        poolManager.ReturnObjectToPool(visualPrefab);
        poolManager.ReturnObjectToPool(this.gameObject);
        mobManager.mobRemovedCalledByMob(this);
    }

    public bool IsAlive()
    {
        return (hp > 0);
    }
}
