using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Unit;

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
        Instantiate(particleObj, chest.transform.position, this.transform.rotation, this.transform);

        GameObject particle = Instantiate(Particle2, Target.transform.position,this.transform.rotation, Target.transform);
        particle.transform.parent = Target.transform;

        if(particle.TryGetComponent<DragonRageParticle>(out var script))
        {
            script.init(this.transform.gameObject);
        }
    }
}
