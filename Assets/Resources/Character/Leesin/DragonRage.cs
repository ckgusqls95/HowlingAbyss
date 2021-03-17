using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Unit;
using Photon.Pun;

public class DragonRage : Skill
{
    private Animator animator;
    private GameObject Target;
    private GameObject chest;

    public GameObject Particle2;
    protected override void Awake()
    {
        coolTime = 18.0f;
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

    public override void Play(GameObject target = null)
    {
        transform.GetComponent<Champion>().UnitStatus.cost -= LevelperValues[CurrentLevel].consumeCost;
        // target 
        currentCoolTime = coolTime;
        // particle target ->
        Target = target;
        animator.SetTrigger("DragonRage");
        StartCoroutine(CalculationCooltime());
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if (target == null) return false;

        
        if (currentCoolTime > 0.001f ||
           (Vector3.Distance(target.transform.position, this.transform.position) > 10.0f))
        {
            return false;
        }

        if(target.TryGetComponent<Units>(out Units script))
        {
            if(script.UnitTag != UnitsTag.Champion)
            {
                return false;
            }

        }
        else
        {
            return false;
        }

        if(transform.GetComponent<Champion>().UnitStatus.cost < LevelperValues[CurrentLevel].consumeCost)
        {
            return false;
        }

        Vector3 direction = (target.transform.position - this.transform.position);
        direction  = direction.normalized;
        float Rotation = Mathf.Acos(Vector3.Dot(this.transform.forward,direction));
        Rotation *= Mathf.Rad2Deg;
        
        const float ViewAngle = 60.0f;

        if(Rotation < ViewAngle)
        {
            return true;
        }

        return false;
    }

    public override void Destroy()
    {
        base.Destroy();
    }


    IEnumerator CalculationCooltime()
    {
        while (currentCoolTime > 0.001f)
        {
            currentCoolTime -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        currentCoolTime = 0.0f;
    }

    private void CreateDragonRage()
    {
        GameObject particle1 = PhotonNetwork.Instantiate("Character/Leesin/DragonRage1", chest.transform.position, this.transform.rotation);
        particle1.transform.SetParent(this.transform);

        GameObject particle = PhotonNetwork.Instantiate("Character/Leesin/DragonRage2", Target.transform.position,this.transform.rotation);
        particle.transform.SetParent(Target.transform);

        if(particle.TryGetComponent<DragonRageParticle>(out var script))
        {
            script.init(this.transform.gameObject);
        }

        //target damage
        {
            Units unit = transform.GetComponent<Units>();
            float Damage = unit.Attack(AttackType.AD_SKILL,skillFactor, LevelperValues[CurrentLevel].addDamage);
            float Suffer = Target.GetComponent<Units>().hit(AttackType.AD_SKILL, Damage, this.transform.GetComponent<Units>(),unit.UnitStatus.armorPenetration);
        }


    }
}
