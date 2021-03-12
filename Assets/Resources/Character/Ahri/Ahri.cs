using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;
using Unit;

public class Ahri : Champion
{
    private Animator animator;
    const int Skillcount = 4;
    //const string ResourcesPath = "Character/Ahri";
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        championSkill = new Skill[Skillcount];
        championSkill[0] = GetComponent<OrbofDeception>();
        championSkill[1] = GetComponent<FoxFire>();
        championSkill[2] = GetComponent<Charm>();
        championSkill[3] = GetComponent<SpritRush>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseSkillE()
    {
        const int index = 2;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

        return;
    }

    public override void UseSkillQ()
    {
        const int index = 0;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

        return;
    }

    public override void UseSkillR()
    {
        const int index = 3;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
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

        return;
    }

}
