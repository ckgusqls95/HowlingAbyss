using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;

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

        // factor
        {
            CurrentLevel = 0;
            skillFactor = 0.3f;
            LevelperValues = new SkillLevel[5];
            LevelperValues[0] = new SkillLevel(1, 40, 40);
            LevelperValues[1] = new SkillLevel(2, 65, 40);
            LevelperValues[2] = new SkillLevel(3, 90, 40);
            LevelperValues[3] = new SkillLevel(4, 115, 40);
            LevelperValues[4] = new SkillLevel(5, 140, 40);
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
        GameObject particle = PhotonNetwork.Instantiate("Character/Ahri/w/FoxFire", Root.transform.position, Quaternion.identity);
        particle.transform.SetParent(this.transform);
        particle.GetComponent<ControlFoxFire>().init(this.transform.gameObject);
    }

}