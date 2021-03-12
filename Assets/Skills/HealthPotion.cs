using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using SkillSystem;

public class HealthPotion : Skill
{
    protected override void Start()
    {
        skillType = skilltype.ACTIVE;
    }

    // Update is called once per frame
    protected override void Update()
    {

    }
    public override void Destroy()
    {
        throw new System.NotImplementedException();
    }

    public override void Play(GameObject target = null)
    {
        // regen 5 health every 0.5fs over 15 seconds, restoring a total of 150 health
        GameObject instantObj = Instantiate(particleObj, Vector3.zero, Quaternion.identity, target.transform);
    }

    public override void Stop()
    {
        throw new System.NotImplementedException();
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if(State == PlayerState.USED)
        {
            return true;
        }

        return false;
    }

}
