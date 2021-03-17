using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using UnityEngine.AI;
using Unit;
public class SonicWave : Skill
{
    private bool sonicWave;
    private Animator animator;
    public GameObject HitObject;
    private GameObject hand;
    const float flyspeed = 20.0f;
    private PlayerController pc;
    private NavMeshAgent agent;

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


        // factor
        {
            CurrentLevel = 0;
            skillFactor = 1.0f;
            LevelperValues = new SkillLevel[5];
            LevelperValues[0] = new SkillLevel(1, 55, 50);
            LevelperValues[1] = new SkillLevel(2, 80, 50);
            LevelperValues[2] = new SkillLevel(3, 105, 50);
            LevelperValues[3] = new SkillLevel(4, 130, 50);
            LevelperValues[4] = new SkillLevel(5, 155, 50);
        }
        //

    }

    private void FixedUpdate()
    {
        
    }

    public override void Destroy()
    {
        
    }

    public override void Play(GameObject target = null)
    {
        transform.GetComponent<Champion>().UnitStatus.cost -= LevelperValues[CurrentLevel].consumeCost;

        if (!sonicWave && currentCoolTime <= 0.1f)
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
        //pc.stop();
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
        while(currentCoolTime > 0.1f)
        {
            currentCoolTime -= Time.deltaTime;
               
            yield return new WaitForEndOfFrame();
        }        
        currentCoolTime = 0.0f;

        sonicWave = false;

        if(HitObject)
        {
            HitObject = null;
        }
    }

    IEnumerator FlyingAnimation()
    {
        pc.stop();
        Vector3 target = HitObject.transform.position;
        target.y = this.transform.position.y;
        float step = flyspeed * Time.deltaTime;
        while (Vector3.Distance(target, this.transform.position) > 1f && HitObject)
        {            
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);
            this.transform.LookAt(target);
            yield return null;
        }
        pc.targetpos = this.transform.position;
        pc.Play();
        animator.SetBool("ResonatingStrike", false);

        if (HitObject)
        {
            if (HitObject.TryGetComponent<Units>(out var script))
            {
                Units unit = transform.GetComponent<Units>();

                float Damage = unit.Attack(AttackType.AD_SKILL, skillFactor, LevelperValues[CurrentLevel].addDamage);

                float Suffer = 0.0f;
                {
                    Suffer = script.hit(AttackType.AD_SKILL, Damage, unit,unit.UnitStatus.armorPenetration);
                }
                
            }

            HitObject = null;
        }
    }
    void CreateSonicWave()
    {
        GameObject instance = Instantiate(particleObj,hand.transform.position,Quaternion.identity, null);

        SonicwaveParticle script = instance.GetComponent<SonicwaveParticle>();
        {
            script.init(gameObject);
        }

        //pc.Play();
    }
}
