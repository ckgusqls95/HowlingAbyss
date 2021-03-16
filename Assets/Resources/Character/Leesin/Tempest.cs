﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;
using System.IO;
public class Tempest : Skill
{
    private bool tempest;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        tempest = false;
        coolTime = 10.0f;
        animator = GetComponent<Animator>();

        // factor
        {
            CurrentLevel = 0;
            skillFactor = 1.0f;
            LevelperValues = new SkillLevel[5];
            LevelperValues[0] = new SkillLevel(1, 100, 50);
            LevelperValues[1] = new SkillLevel(2, 140, 50);
            LevelperValues[2] = new SkillLevel(3, 180, 50);
            LevelperValues[3] = new SkillLevel(4, 220, 50);
            LevelperValues[4] = new SkillLevel(5, 260, 50);
        }
        //

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

        if (!tempest && currentCoolTime <= 0.0f)
        {
            animator.SetTrigger("Tempest");

            currentCoolTime = coolTime;
            StartCoroutine(CalculationCooltime());
            tempest = true;
        }
        else if (tempest && currentCoolTime > 0.001f)
        {
            animator.SetTrigger("Cripple");
            tempest = false;
        }
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if(transform.GetComponent<Champion>().UnitStatus.cost < LevelperValues[CurrentLevel].consumeCost)
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

            yield return new WaitForEndOfFrame();
        }
        
        currentCoolTime = 0.0f;
    
        tempest = false;
    }

    void CreateTempest()
    {
        GameObject instance = PhotonNetwork.Instantiate("Character/Leesin/Tempest", transform.position, Quaternion.identity);
        TempestParticle script;
        if (instance.TryGetComponent<TempestParticle>(out script))
        {
            script.init(this.transform.gameObject);
        }
    }

    void CreateCripple()
    {
        GameObject instance = Instantiate(particleObj, transform.position, Quaternion.identity, null);
        TempestParticle script;
        if (instance.TryGetComponent<TempestParticle>(out script))
        {
            script.init(this.transform.gameObject);
        }
    }
}
