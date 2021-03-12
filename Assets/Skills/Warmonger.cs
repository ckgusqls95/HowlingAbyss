using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class Warmonger : Skill
{

    protected override void Awake()
    {
        skillType = skilltype.PASSIVE;
        
       // if (Try())
        {
            Play(this.transform.gameObject);
        }


    }

    public override void Play(GameObject target)
    {
        //player Omni vamp 2.5% 
        
    }

    public override void Stop()
    {
        return;
    }

    public override void Destroy()
    {
        // 
    }

    public override bool Try(PlayerState State = PlayerState.IDLE,GameObject target = null)
    {
        return true;
    }
}
