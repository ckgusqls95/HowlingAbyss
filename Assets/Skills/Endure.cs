using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Unit;

public class Endure : Skill
{
    protected override void Awake()
    {
        skillType = skilltype.PASSIVE;
    }


    // Start is called before the first frame update
    protected override void  Start()
    {
        
    }

    public override void Destroy()
    {
        
    }

    public override void Play(GameObject target = null)
    {
        GameObject instantObj = Instantiate(particleObj, Vector3.zero, Quaternion.identity, target.transform);
        //gain health regeneration equal to 0 − 40
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
            if (State == PlayerState.HIT &&
                unit.UnitTag == UnitsTag.Champion)
            {
                return true;
            }

        }
        return false;
    }
}
