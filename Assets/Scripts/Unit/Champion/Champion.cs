using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using SkillSystem;
using UnityEngine.AI;

public abstract class Champion : Units
{
    #region ChampionCommonData
    
    protected Skill[] championSkill;
    public Skill[] ChampionSkill { get { return championSkill; } }
    
    #endregion

    protected virtual void Awake()
    {
        
        unitTag = UnitsTag.Champion;
        
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        #region InitStatus
        
        UnitStatus.abilityPower = 0.0f;
        UnitStatus.abilityHaste = 0.0f;
        UnitStatus.armorPenetration = 0.0f;
        UnitStatus.magicPenetration = 0.0f;
        UnitStatus.experience = 0.0f;
        UnitStatus.killExperience = 0.0f;
        UnitStatus.killGold = 300.0f;
        UnitStatus.level = 1;
        UnitStatus.spellVamp = 0.0f;
        UnitStatus.lifeSteal = 0.0f;

        unitTag = UnitsTag.Champion;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void MeeleeAttack()
    {
        // 공격 범위 그리기
        //champion.UnitSight.attackRange;

        // 좌클릭시 타게팅 공격
        //if (Input.GetMouseButtonDown(0))
        //{
        //}

        // 우클릭시 공격 모드 해제        
        //else if (Input.GetMouseButtonDown(1))
        //{
        //}

        
    }

    public abstract void UseSkillQ();

    public abstract void UseSkillW();

    public abstract void UseSkillE();

    public abstract void UseSkillR();

    public void Targeting(GameObject obj)
    {
        Target = obj;
    }
}
