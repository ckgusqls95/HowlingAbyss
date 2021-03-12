using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Unit;
public class Focus : Skill
{
    // Start is called before the first frame update
    protected override void Start()
    {
        skillType = skilltype.PASSIVE;
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
        // 5 bonus physical damage
    }

    public override void Stop()
    {
        throw new System.NotImplementedException();
    }

    public override bool Try(PlayerState State = PlayerState.IDLE,GameObject target = null)
    {
        if(State == PlayerState.ATTACK &&
            target.GetComponent<Units>().UnitTag == UnitsTag.Minion)
        {
            return true;
        }
        return false;
    }

}
