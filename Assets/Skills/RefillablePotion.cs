using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class RefillablePotion : Skill
{
    // Holds charges that refill upon visiting the shop. (2 charges);
    protected override void Start()
    {
        skillType = skilltype.ACTIVE;
    }

    public override void Destroy()
    {
        return;
    }

    public override void Play(GameObject target = null)
    {
        // regen 5 health every 0.5fs over 12 seconds, restoring a total of 150 health
        GameObject instantObj = Instantiate(particleObj, Vector3.zero, Quaternion.identity, target.transform);
    }

    public override void Stop()
    {
       
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if (State == PlayerState.USED)
        {
            return true;
        }

        return false;
    }
}
