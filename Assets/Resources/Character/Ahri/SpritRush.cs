﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
public class SpritRush : Skill
{
    private Animator animator;
    private GameObject ROOT;
    const float RushCount = 3;
    float CurrentCount = 0;
    const float RecycleTime = 5.0f;
    float ElapsedRecycleTime = 0.0f;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        coolTime = 130.0f;
        animator = GetComponent<Animator>();
        Transform[] childrens = GetComponentsInChildren<Transform>();
        foreach (Transform child in childrens)
        {
            if (child.name.Contains("ROOT"))
            {
                ROOT = child.gameObject;
                break;
            }
        }
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if(CurrentCount == RushCount)
        {
            return false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("SkillLayer.R"))
        {
            return false;
        }

        return true;
    }

    public override void Play(GameObject target = null)
    {
        if (CurrentCount == 0)
        {
            currentCoolTime = coolTime;
        }

        ElapsedRecycleTime = RecycleTime;
        CurrentCount++;
        animator.SetTrigger("SpritRush");
        StartCoroutine(CalculationCooltime());
    }


    IEnumerator CalculationCooltime()
    {
        while (currentCoolTime > 0.001f)
        {
            currentCoolTime -= Time.deltaTime;
            ElapsedRecycleTime -= Time.deltaTime;

            if(ElapsedRecycleTime < 0.0f)
            {
                CurrentCount = RushCount;
            }

            if(animator.GetCurrentAnimatorStateInfo(0).IsName("SkillLayer.R") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
            {
                if (this.transform.parent.TryGetComponent<PlayerController>(out PlayerController script))
                {
                    script.isStopMove = false;
                }
            }

            yield return new WaitForEndOfFrame();
        }

        currentCoolTime = 0.0f;
        ElapsedRecycleTime = 0.0f;
        CurrentCount = 0;
    }

    private void CreateSpritRush()
    {
        if (this.transform.parent.TryGetComponent<PlayerController>(out PlayerController script))
        {
            script.isStopMove = true;
        }

        Instantiate(particleObj, ROOT.transform.position, Quaternion.identity,this.transform);
    }
}
