using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using UnityEngine.AI;
public class SonicWave : Skill
{
    private bool sonicWave;
    private Animator animator;
    public GameObject HitObject;
    private GameObject hand;
    const float flyspeed = 15.0f;
    public PlayerController pc;
    public NavMeshAgent agent;
    protected override void Awake()
    {
        base.Awake();
        sonicWave = false;
        coolTime = 10.0f;

        Resources.Load("Character/Leesin/SonicWave");
        animator = GetComponent<Animator>();
        pc = this.transform.GetComponent<PlayerController>();
        agent = transform.GetComponent<NavMeshAgent>();
        Transform[] childrens = GetComponentsInChildren<Transform>();
        foreach(Transform child in childrens)
        {
            if(child.name.Contains("l_hand"))
            {
                hand = child.gameObject;
                break;
            }
        }

    }

    private void FixedUpdate()
    {
        
    }

    public override void Destroy()
    {
        
    }

    public override void Play(GameObject target = null)
    {
        if(!sonicWave && currentCoolTime <= 0.1f)
        {
            animator.SetTrigger("Sonicwave");
            currentCoolTime = coolTime;
            StartCoroutine(CalculationCooltime());
            sonicWave = true;
        }
        else if(sonicWave && currentCoolTime > 0.1f && HitObject)
        {
            animator.SetTrigger("ResonatingStrike");
            animator.SetBool("ResonatingStrike",true);
            sonicWave = false;

            // coroutin
            StartCoroutine(FlyingAnimation());
        }
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        return true;
    }

    IEnumerator CalculationCooltime()
    {
        while(currentCoolTime > 0.1f)
        {
            currentCoolTime -= Time.deltaTime;
               
            yield return new WaitForEndOfFrame();
        }
        
        if(currentCoolTime < 0)
        {
            currentCoolTime = 0.0f;
        }

        sonicWave = false;
    }

    IEnumerator FlyingAnimation()
    {
        pc.isStopMove = true;
        agent.isStopped = true;
        Vector3 target = HitObject.transform.position;
        target.y = this.transform.position.y;
        float step = flyspeed * Time.deltaTime;
        while (Vector3.Distance(target, this.transform.parent.position) > 2f)
        {            
            this.transform.parent.position = Vector3.MoveTowards(this.transform.parent.position, target, step);
            this.transform.parent.transform.LookAt(target);
            yield return null;
        }

        pc.targetpos = this.transform.parent.transform.position;
        pc.isStopMove = false;
        agent.isStopped = false;

        animator.SetBool("ResonatingStrike", false);
        HitObject = null;
    }
    void CreateSonicWave()
    {
        GameObject instance = Instantiate(particleObj,hand.transform.position,Quaternion.identity, null);

        SonicwaveParticle script;
        if (instance.TryGetComponent<SonicwaveParticle>(out script))
        {
            script.init(this.transform.gameObject);
        }
    }
}
