using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class FoxFire : Skill
{
    private Animator animator;
    private GameObject Root;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        Transform[] childrens = GetComponentsInChildren<Transform>();
        foreach (Transform child in childrens)
        {
            if (child.name.Contains("ROOT"))
            {
                Root = child.gameObject;
                break;
            }
        }

        coolTime = 9.0f;
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
        animator.SetTrigger("FoxFire");
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

    private void CreateFoxFire()
    {
        GameObject particle = Instantiate(ParticleObj, Root.transform.position, Quaternion.identity, this.transform);
        particle.GetComponent<ControlFoxFire>().init(this.transform.gameObject);
    }

}