using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class RelentlessPursuit : Skill
{
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        coolTime = 12.0f;
        animator = GetComponent<Animator>();

        Resources.Load("Character/Lucian/SkillPrefabs/ArdentBlaze");

    }

    private void FixedUpdate()
    {

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
        // target not Used
        if (Try(PlayerState.IDLE, target))
        {
            animator.SetTrigger("RelentlessPursuit");
            currentCoolTime = coolTime;
            StartCoroutine(CalculationCooltime());
        }
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if (currentCoolTime <= 0.1f)
        {
            return true;
        }
        return false;
    }

    IEnumerator CalculationCooltime()
    {
        while (currentCoolTime > 0.1f)
        {
            currentCoolTime -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        if (currentCoolTime < 0)
        {
            currentCoolTime = 0.0f;
        }
    }

    void CreateArdentBlaze()
    {
        GameObject instance = Instantiate(particleObj, this.transform.position, Quaternion.identity, null);

        ArdentBlazeParticle script;
        if (instance.TryGetComponent<ArdentBlazeParticle>(out script))
        {
            //script.init(this.transform.gameObject);
        }
    }
}
