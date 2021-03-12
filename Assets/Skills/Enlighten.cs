using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Unit;

public class Enlighten : Skill
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

    }

    public override void Play(GameObject target = null)
    {
        // player maximam mana * 0.2f stores over 3 seconds
    }

    public override void Stop()
    {
       
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if(State == PlayerState.LEVELUP)
        {
            return true;
        }

        return false;
    }

}
