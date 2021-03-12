using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class OrbofDeception : Skill
{
    private Animator animator;
    private GameObject weapon;

    protected override void Awake()
    {
        base.Awake();
        coolTime = 7.0f;
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
        if(currentCoolTime > 0.0f)
        {
            return false;
        }

        return true;
    }


    public override void Play(GameObject target = null)
    {
        if(this.transform.parent.TryGetComponent<PlayerController>(out PlayerController script))
        {
            script.isStopMove = true;
        }

        animator.SetTrigger("Orb");
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
        weapon.SetActive(true);
    }

    void CreateOrb()
    {
        if (this.transform.parent.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.isStopMove = false;
        }

        ActiveWeapon();
        GameObject particle = Instantiate(ParticleObj,weapon.transform.position, Quaternion.identity,null);
        OrbParticle script = particle.AddComponent<OrbParticle>();
        script.init(this.gameObject);
    }

    public void ActiveWeapon()
    {
        if(weapon.activeInHierarchy)
        {
            weapon.SetActive(false);
        }
        else
        {
            weapon.SetActive(true);
        }
    }
}
