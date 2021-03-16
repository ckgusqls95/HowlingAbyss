using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;
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
        transform.GetComponent<Champion>().UnitStatus.cost -= LevelperValues[CurrentLevel].consumeCost;

        if (target)
        {
            animator.SetTrigger("SafeGuard");             
            Target = target;
        }
        else
        {
                
        }

        StartCoroutine(CalculationCooltime());
        instanceParticle = PhotonNetwork.Instantiate("Character/Leesin/SafeGuard", chest.transform.position, Quaternion.identity);
        instanceParticle.transform.SetParent(this.transform);
        currentCoolTime = coolTime;
        Leesin player = gameObject.GetComponent<Leesin>();
        player.playsound("SFX_Safeguard");
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if(currentCoolTime > 0.001f || transform.GetComponent<Champion>().UnitStatus.cost < LevelperValues[CurrentLevel].consumeCost)
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
