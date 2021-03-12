using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class SafeGuard : Skill
{
    //private bool safeguard;
    private Animator animator;
    private GameObject player;
    private GameObject Target;
    const float speed =30.0f;
    private GameObject instanceParticle;
    private GameObject chest;
    protected override void Awake()
    {
        base.Awake();
        coolTime = 12.0f;
        //safeguard = false;
        //instanceParticle = Resources.Load("Character/Leesin/SafeGuard");
        animator = GetComponent<Animator>();

        Transform[] childrens = GetComponentsInChildren<Transform>();
        foreach (Transform child in childrens)
        {
            if (child.name.Contains("chest"))
            {
                chest = child.gameObject;
                break;
            }
        }

    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void Stop()
    {
        base.Stop();
    }

    public override void Play(GameObject target = null)
    {
        if (target)
        {
            animator.SetTrigger("SafeGuard");             
            Target = target;
        }
        else
        {
                
        }

        StartCoroutine(CalculationCooltime());
        instanceParticle = Instantiate(particleObj, chest.transform.position, Quaternion.identity, this.transform);
        currentCoolTime = coolTime;
        
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if(currentCoolTime > 0.001f)
        {
            return false;
        }

        return true;
    }


    IEnumerator CalculationCooltime()
    {
        while (currentCoolTime > 0.001f)
        {
            currentCoolTime -= Time.deltaTime;
            
            if(Target)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, 
                    Target.transform.position, Time.deltaTime * speed);
            }

            yield return null;
        }

        if (currentCoolTime < 0)
        {
            currentCoolTime = 0.0f;
        }

       if(Target)
        {
            Target = null;
        }

        Object.Destroy(instanceParticle);
    }
}
