using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
public class Charm : Skill
{
    private Animator animator;
    private GameObject weapon;

    protected override void Awake()
    {
        base.Awake();
        coolTime = 12.0f;
        animator = GetComponent<Animator>();

        Transform[] childrens = GetComponentsInChildren<Transform>();
        foreach (Transform child in childrens)
        {
            if (child.name.Contains("weapon"))
            {
                weapon = child.gameObject;
                break;
            }
        }
    }

    public override bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null)
    {
        if (currentCoolTime > 0.0f)
        {
            return false;
        }

        return true;
    }

    public override void Play(GameObject target = null)
    {
        animator.SetTrigger("Charm");
        currentCoolTime = coolTime;
        StartCoroutine(CalculationCooltime());
    }

    IEnumerator CalculationCooltime()
    {
        while (currentCoolTime > 0.1f)
        {
            currentCoolTime -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        currentCoolTime = 0.0f;
    }

    void CreateCharm()
    {
        GameObject particle = Instantiate(ParticleObj, weapon.transform.position, Quaternion.identity, null);
        CharmParticle script = particle.GetComponent<CharmParticle>();
        script.init(this.transform.gameObject);
    }
}
