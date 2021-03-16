using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class OrbofDeception : Skill
{
    private Animator animator;
    private GameObject weapon;
    private PlayerController pc;
    protected override void Awake()
    {
        base.Awake();
        coolTime = 7.0f;
        animator = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();

        Transform[] childrens = GetComponentsInChildren<Transform>();
        foreach (Transform child in childrens)
        {
            if (child.name.Contains("weapon"))
            {
                weapon = child.gameObject;
                break;
            }
        }

        // factor
        {
            CurrentLevel = 0;
            skillFactor = 0.35f;
            LevelperValues = new SkillLevel[5];
            LevelperValues[0] = new SkillLevel(1, 40, 65);
            LevelperValues[1] = new SkillLevel(2, 65, 70);
            LevelperValues[2] = new SkillLevel(3, 90, 75);
            LevelperValues[3] = new SkillLevel(4, 115, 80);
            LevelperValues[4] = new SkillLevel(5, 140, 85);
        }
        //

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
        if(this.transform.TryGetComponent<PlayerController>(out PlayerController script))
        {
            script.isStopMove = true;
        }

        animator.SetTrigger("Orb");
        currentCoolTime = coolTime;
        StartCoroutine(CalculationCooltime());
        //StartCoroutine(turn());
    }
    IEnumerator turn()
    {
        Vector3 dir = Input.mousePosition - transform.position;
        dir.y = 0.0f;
        Quaternion Rotation = Quaternion.LookRotation(dir);
        while (dir != Vector3.zero)
        {
            this.transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Rotation,
                360.0f * Time.deltaTime);
            yield return null;
        }
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
        if (this.transform.TryGetComponent<PlayerController>(out PlayerController player))
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
