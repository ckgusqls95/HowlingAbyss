using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
public class Tempest : Skill
{
    private bool tempest;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        tempest = false;
        coolTime = 10.0f;

        Resources.Load("Character/Leesin/Tempest");
        animator = GetComponent<Animator>();
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
        GameObject instance = Instantiate(particleObj,transform.position, Quaternion.identity, null);
        TempestParticle script;
        if (instance.TryGetComponent<TempestParticle>(out script))
        {
            script.init(this.transform.gameObject);
        }
    }

    void CreateCripple()
    {

    }
}
