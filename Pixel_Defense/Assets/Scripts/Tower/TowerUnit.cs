using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static ObjectPoolManager;

public class TowerUnit : DIMono
{
    [Inject]
    MobManager mobManager;

    [Inject]
    ObjectPoolManager objectPoolManager;

    [Inject]
    GameData gameData;

    [Inject]
    TowerManager towerManager;

    [Inject]
    StagePlayData stagePlayData;

    [Inject]
    AudioManager audioManager;

    private Tower tower;

    public Tower Tower
    {
        get => tower;
    }

    public float nextAtkTime = 0;
    public float nextSkillTime = 0;
    Mob targetMob = null;
    public Skill skill = null;
    public Vector3Int currentPlace;
    public int ID { get; private set; }
    public List<Buff> buffs = new List<Buff>();

    public void SetData(Tower tower, int id, Vector3Int currentPlace)
    {
        this.CheckInject();
        this.tower = tower;
        nextAtkTime = Time.time;
        nextSkillTime = Time.time;
        this.ID = id;
        this.currentPlace = currentPlace;
        skill = gameData.skills.FirstOrDefault(I => I.code == tower.code);

        Debug.Log(" <color ='green'>TowerUnit SetData Skill = " + skill.code + " " + skill.coolTime+ "</color>");
        
    }

    //한번 지정한 몹을 계속 공격하다 공격범위를 벗어나면 제일가까운 몹 타겟
    private void Update()
    {
        AttackProcess();

        SkillProcess();

        DebuffProcess();

        CheckRemoveBuff();

        RenewBuffProcess();
    }
    #region Debuff Process
    private void DebuffProcess()
    {
        if (stagePlayData.currentStage.code % 10 == 0 || stagePlayData.currentStage.code == 35) return;
        if (skill.fx != Skill.Fx.debuff) return;
        if (skill.target == Skill.Target.oneTarget) return;
        //1. 범위 내의 몹 속도 감소(디버프를 이미 갖고있으면 제외)
        var targets = FindSkillDebuffTarget(skill.targetParam[0]);
        if (targets.Count <= 0) return;
        
        foreach (var mob in targets)
        {
            if (mob.hasThisDebuff(skill.code)) continue;
            var debuffDegree = mob.Monster.speed * (skill.fxParam[0] / 100);
            mob.speed -= debuffDegree;
            mob.addDebuff(skill.code);
        }
        //2. 해당 디버프 코드를 가진 몹이 범위 밖을 벗어나면 해제
        foreach(var mob in mobManager.mobs)
        {
            if(mob.hasThisDebuff(skill.code) && !isMobInRange(skill.targetParam[0], mob))
            {
                var debuffDegree = mob.Monster.speed * (skill.fxParam[0] / 100);
                mob.debuffs.Remove(skill.code);
                mob.speed += debuffDegree;
            }
        }
    }

    private bool isMobInRange(float range, Mob mob)
    {
        var towerPos = transform.position;
        var mobPos = mob.transform.position;
        var distV = mobPos - towerPos;
        var sqrDist = distV.sqrMagnitude;
        if (sqrDist <= range * range)
        {
            return true;
        }
        return false;
    }

    List<Mob> FindSkillDebuffTarget(float range)
    {
        List<Mob> targets = new List<Mob>();
        var towerPos = transform.position;
        foreach (var mob in mobManager.mobs)
        {
            var mobPos = mob.transform.position;
            var distV = mobPos - towerPos;
            var sqrDist = distV.sqrMagnitude;

            if (sqrDist <= range * range)
            {
                targets.Add(mob);
            }
        }
        return targets;
    }

    #endregion

    #region Buff Process
    private void AddBuff(Buff buff)
    {
        buffs.Add(buff);
    }

    private void RenewBuffProcess()
    {
        if (skill.fx != Skill.Fx.passiveBuff)
            return;
        var targetTowers = FindTowerTarget(skill.targetParam[0]);

        foreach (var targetTower in targetTowers)
        {
            if (targetTower == this) continue;
            var buffVal = skill.fxParam[0] + GetStatus(Status.ExtraPriestBuffVal);
            if (HasBuffsCastedByThis(targetTower.buffs) && getBuffCastedByThis(targetTower.buffs).val != buffVal)
            {
                var shouldRemoveBuff = getBuffCastedByThis(targetTower.buffs);
                targetTower.RemoveBuff(targetTower, shouldRemoveBuff);
                Buff buff = new Buff();
                buff.leftTime = -1;
                buff.caster = this;
                buff.val = buffVal;
                buff.targetStatus = skill.GetStatusFromBuffType();
                targetTower.AddBuff(buff);
            }
        }
    }

    private void RemoveBuff(TowerUnit targetTower, Buff buff)
    {
        targetTower.buffs.Remove(buff);
    }

    private void CheckRemoveBuff()
    {
        //지속시간이 다되면
        //버프를 주는 유닛이 사라지면

        var idx=buffs.Count-1;
        while(idx>=0)
        {
            var buff = buffs[idx];

            bool needRemove = false;

            if(buff.leftTime > 0)
            {
                buff.leftTime -= Time.deltaTime;

                if (buff.leftTime <= 0)
                {
                    needRemove = true;
                }
            }

            if(buff.caster == null)
            {
                needRemove = true;
            }

            idx--;

            if (needRemove)
            {
                buffs.Remove(buff);
            }
        }
    }
    //패시브 버프를 주는 유닛이 소환
    //이미 소환되어있는데 범위 내에 새 유닛 소환
    bool HasBuffsCastedByThis(List<Buff> targetBuffs)
    {
        foreach(Buff buff in targetBuffs)
        {
            if (buff.caster == this) return true;
        }
        return false;
    }

    public Buff getBuffCastedByThis(List<Buff> targetBuffs)
    {
        foreach (Buff buff in targetBuffs)
        {
            if (buff.caster == this) return buff;
        }
        return null;
    }

    public void PassiveBuffProcess()
    {
        //만약 자신이 패시브 버프 스킬이 아니라면 리턴
        if (skill.fx != Skill.Fx.passiveBuff)
            return;

        //자신의 버프를 주변에 나누어줌.
        var targetTowers = FindTowerTarget(skill.targetParam[0]);
        
        foreach(var targetTower in targetTowers)
        {
            if (targetTower == this) continue;
            if (HasBuffsCastedByThis(targetTower.buffs)) continue;
            Buff buff = new Buff();
            buff.leftTime = -1;
            buff.caster = this;
            buff.val = skill.fxParam[0] + GetStatus(Status.ExtraPriestBuffVal);
            buff.targetStatus = skill.GetStatusFromBuffType();
            targetTower.AddBuff(buff);
        }

    }
    IEnumerable<TowerUnit> FindTowerTarget(float range)
    {
        var towerPosition = this.transform.position;

        foreach (var tower in towerManager.towerUnits)
        {
            var targetTowerPos = tower.currentPlace;
            var distV = targetTowerPos - towerPosition;

            var sqrDist = distV.sqrMagnitude;
            if (sqrDist > range * range)
            {
                continue;
            }
            yield return tower;
        }
    }
    #endregion

    #region Skill Process
    private void SkillProcess()
    {
        //클래스가 사제일 시 return
        if(skill==null || this.Tower.towerClass == Tower.TowerClass.priest)
        {
            return;
        }

        //쿨타임 체크
        if (nextSkillTime >= Time.time)
        {
            return;
        }

        nextSkillTime += skill.coolTime;

        List<Mob> targetMobList = new();

        //몹 타겟 분류별로 정하기
        switch (skill.target)
        {
            case Skill.Target.mobRange:
                targetMobList = FindSkillAttackTarget();
                break;
            case Skill.Target.oneTarget:
                if(targetMob != null) targetMobList.Add(targetMob);
                break;
        }

        switch (skill.fx)
        {
            case Skill.Fx.selfBuff:
                {
                    Buff buff = new Buff();
                    buff.leftTime = skill.fxParam[0];
                    buff.targetStatus = skill.GetStatusFromBuffType();
                    buff.caster = this;
                    buff.val = skill.fxParam[1];
                    AddBuff(buff);
                }
                break;
            case Skill.Fx.attack:
                {
                    if (targetMobList.Count <= 0) return;
                    foreach (Mob mob in targetMobList)
                    {
                        mob.hp -= skill.fxParam[0];
                        SetProjectile(mob);
                        CheckMobReturn(mob);
                    }
                }
                break;
            default:
                break;
        }
    }
    List<Mob> FindSkillAttackTarget()
    {
        List<Mob> targets = new List<Mob>();
        var firstTarget = FindTarget();
        if (firstTarget == null) return targets;
        var targetRange = skill.targetParam[0];
        var targetPosition = firstTarget.transform.position;
        for (int i = 0; i < mobManager.mobs.Count; i++)
        {
            var mobPosition = mobManager.mobs[i].transform.position;
            var distV = mobPosition - targetPosition;
            var sqrDist = distV.sqrMagnitude;

            if (sqrDist <= targetRange * targetRange)
            {
                targets.Add(mobManager.mobs[i]);
            }
        }
        return targets;
    }
    #endregion

    #region Attack Process
    private void CheckMobReturn(Mob mob)
    {
        if (mob.hp <= 0) mob.ReturnToPool();
    }

    
    private void AttackProcess()
    {
        if (nextAtkTime >= Time.time)
        {
            return;
        }

        if (targetMob == null)
        {
            targetMob = FindTarget();
        }
        else if (IsTargetOutOfAtkRange() ||
                targetMob.IsAlive() == false)
        {                
            targetMob = null;
        }

        if (targetMob != null)
        {
            Attack(targetMob);
        }

        nextAtkTime += 1 / GetStatus(Status.AtkSpeed);
    }

    bool IsTargetOutOfAtkRange()
    {
        var targetMobPosition = targetMob.gameObject.transform.position;
        var towerPosition = this.transform.position;
        var distV = targetMobPosition - towerPosition;
        var sqrDist = distV.sqrMagnitude;
        return (sqrDist > tower.atkRange * tower.atkRange);
     }

    Mob FindTarget()
    {
        float minDistance = 99999f;
        var towerPosition = this.transform.position;
        Mob result = null;
        for(int i = 0; i < mobManager.mobs.Count; i++)
        {
            var mobPosition = mobManager.mobs[i].transform.position;
            var distV = mobPosition - towerPosition;

            var sqrDist = distV.sqrMagnitude;
            if (sqrDist > tower.atkRange * tower.atkRange )
            {
                continue;
            }

            if(minDistance > sqrDist)
            {
                minDistance = sqrDist;
                result = mobManager.mobs[i];
            }
        }
        return result;
    }



    void Attack(Mob mob)
    {
        if (this.Tower.towerClass == Tower.TowerClass.priest) return;
        SetProjectile(mob);

        //attack mob
        mob.hp -= GetStatus(Status.Atk);
        bool isBossStage = stagePlayData.currentStage.code % 10 != 0 && stagePlayData.currentStage.code != 35;

        var sfxName = audioManager.GetAttackSFXName(this.Tower.towerClass);
        audioManager.Play(sfxName);

        var skillPercent = (int)Random.Range(0f, 100f);

        if(skillPercent <= skill.percentParam && skill.fx == Skill.Fx.percentAttack && isBossStage)
        {
            mob.hp -= (mob.Monster.hp * (skill.fxParam[0] / 100));
        }

        if ((skill.fx == Skill.Fx.debuff && skill.target == Skill.Target.oneTarget) && isBossStage)
        {
            if (targetMob.hasThisDebuff(skill.code))
            {
                return;
            }
            var debuffDegree = targetMob.speed * (skill.fxParam[1] / 100);
            targetMob.speed -= debuffDegree;
            targetMob.addDebuff(skill.code);
            mob.RemoveDebuff(skill.fxParam[0], debuffDegree, skill.code);
        }

        if (mob.hp <= 0)
        {

            
            if (skillPercent > skill.percentParam)
            {
                mob.ReturnToPool();
                return;
            }
            if (skill.fx == Skill.Fx.passiveIron)
            {
                stagePlayData.iron += (int)skill.fxParam[0];
            }
            if (skill.fx == Skill.Fx.passiveGold)
            {
                stagePlayData.gold += (int)skill.fxParam[0];
            }
            if (skill.fx == Skill.Fx.passiveEmelard)
            {
                stagePlayData.emelard += (int)skill.fxParam[0];
            }
            mob.ReturnToPool();
        }
    }

    #endregion

    #region Tower Projectile
    private void SetProjectile(Mob mob)
    {
        var proj = objectPoolManager.GetObjectFromPool("ProjectileProto").GetComponent<Projectile>();
        proj.SetTarget(mob);
        proj.SetTowerUnit(this);
        proj.transform.position = this.transform.position;
    }
    #endregion

    #region Tower Total Status
    public float GetStatus(Status status)
    {
        var finalStatus = Tower.GetState(status);
        foreach(var buff in buffs)
        {
            if (buff.targetStatus == status)
            {
                if (buff.targetStatus == Status.AtkSpeed) finalStatus += Tower.GetState(Status.AtkSpeed) * (buff.val / 100);
                else finalStatus += buff.val;
            }
            
        }

        if(this.Tower.towerClass == Tower.TowerClass.human && status == Status.Atk)
        {
            var row = gameData.upgradeHuman.FindByLevel(stagePlayData.humanLevel);
            if (row != null)
            {
                finalStatus += row.val;
            }
        }
        if (this.Tower.towerClass == Tower.TowerClass.spirit && status == Status.Atk)
        {
            var row = gameData.upgradeSpirit.FindByLevel(stagePlayData.spiritLevel);
            if (row != null)
            {
                finalStatus += row.val;
            }
        }
        if (this.Tower.towerClass == Tower.TowerClass.not_human && status == Status.Atk)
        {
            var row = gameData.upgradeNotHuman.FindByLevel(stagePlayData.notHumanLevel);
            if (row != null)
            {
                finalStatus += row.val;
            }
        }
        if (this.Tower.towerClass == Tower.TowerClass.priest && status == Status.ExtraPriestBuffVal)
        {
            var row = gameData.upgradePriest.FindByLevel(stagePlayData.priestLevel);
            if (row != null)
            {
                finalStatus += row.val;
            }
        }

        return finalStatus;
    }
    #endregion
}
