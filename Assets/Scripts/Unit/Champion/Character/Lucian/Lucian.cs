using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;
using Unit;

public class Lucian : Champion
{
    //[SerializeField]
    //private UnitData lucianData;
    //[SerializeField]
    //private ChampionData lucianSkillData;

    protected Animator animator;
    const int Skillcount = 5;



    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        championSkill = new Skill[Skillcount];
        championSkill[0] = GetComponent<LightsSlinger>();
        ChampionSkill[1] = GetComponent<PiercingLight>();
        ChampionSkill[2] = GetComponent<ArdentBlaze>();
        ChampionSkill[3] = GetComponent<RelentlessPursuit>();
        championSkill[4] = GetComponent<TheCulling>();
    }

    public override void UseSkillQ()
    {
        const int index = 1;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

        return;
    }
    public override void UseSkillW()
    {
        const int index = 2;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

        return;
    }

    public override void UseSkillE()
    {
        const int index = 3;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

        return;
    }

    public override void UseSkillR()
    {
        const int index = 4;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

        return;
    }
}
