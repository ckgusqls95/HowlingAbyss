using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class LifeLine : Skill
{
    protected override void Awake()
    {
        skillType = skilltype.PASSIVE;
        coolTime = 90.0f;
    }

    public override void Destroy()
    {
       
    }

    public override void Play(GameObject target)
    {
        GameObject instantObj = Instantiate(particleObj, Vector3.zero, Quaternion.identity, target.transform);
        currentCoolTime = coolTime;

        StartCoroutine(ElapsedTimeCalculation());
    }

    public override void Stop()
    {
       
    }

    public override bool Try(PlayerState State,GameObject target = null)
    {
        if(State  == PlayerState.HIT)
        {
            // player health under 30percent && cooltime 90second
            return true;
        }

        return false;
    }



}
