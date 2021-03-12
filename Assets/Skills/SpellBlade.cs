using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class SpellBlade : Skill
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
        GameObject instantObj = Instantiate(particleObj, Vector3.zero, Quaternion.identity, target.transform);
        currentCoolTime = coolTime;

        StartCoroutine(ElapsedTimeCalculation());
    }

    public override void Stop()
    {
    }

    public override bool Try(PlayerState State = PlayerState.IDLE,GameObject target = null)
    {
        if(State == PlayerState.AFTERSKILL)
        {
            return true;
        }

        return false;
    }
}
