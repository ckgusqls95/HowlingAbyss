using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;
using Unit;

public class Leesin : Champion
{
    private Animator animator;
    const int Skillcount = 5;
    //const string ResourcesPath = "Character/Leesin";

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        championSkill = new Skill[Skillcount];
        championSkill[0] = GetComponent<SonicWave>();
        ChampionSkill[1] = GetComponent<SafeGuard>();
        ChampionSkill[2] = GetComponent<Tempest>();
        ChampionSkill[3] = GetComponent<DragonRage>();
        
    }

    public override void UseSkillQ()
    {
        const int index = 0;
        
        if(championSkill[index].Try(PlayerState.SKILL,Target))
        {
            championSkill[index].Play();
        }

        return;
    }

    public override void UseSkillW()
    {
        const int index = 1;
        
        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

    }

    public override void UseSkillE()
    {
        const int index = 2;
        
        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

    }

    public override void UseSkillR()
    {
        const int index = 3;
        
        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play(Target);
        }

    }
}
