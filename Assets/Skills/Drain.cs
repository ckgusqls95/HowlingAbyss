using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Unit;

public class Drain : Skill
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
        // restore mana +6
    }

    public override void Stop()
    {
        return;
    }


    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        Units unit;
        if (target.TryGetComponent<Units>(out unit))
        {

            if (State == PlayerState.KILL &&
                unit.UnitTag == UnitsTag.Minion)
            {
                return true;
            }

        }
        return false;
    }

}
