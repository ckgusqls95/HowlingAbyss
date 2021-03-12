using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class LightsSlinger : Skill
{
    [SerializeField]
    private ParticleSystem particle;

    public override void Play(GameObject target = null)
    {
        particle.Play();
    }
    public override void Stop()
    {
        particle.Stop();
    }
    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        return true;
    }
}
