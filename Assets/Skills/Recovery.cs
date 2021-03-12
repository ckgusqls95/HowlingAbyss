using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Unit;

public class Recovery : Skill
{
    protected override void Awake()
    {
        skillType = skilltype.PASSIVE;
    }

    public override void Destroy()
    {
        
    }

    public override void Play(GameObject target = null)
    {
       // restores 6 health every 5 seconds
    }

    public override void Stop()
    {
        
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        return true;
    }
}
