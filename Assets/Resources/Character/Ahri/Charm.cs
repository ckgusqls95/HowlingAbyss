using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;
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


        // factor
        {
            CurrentLevel = 0;
            skillFactor = 0.4f;
            LevelperValues = new SkillLevel[5];
            LevelperValues[0] = new SkillLevel(1, 60, 70);
            LevelperValues[1] = new SkillLevel(2, 90, 70);
            LevelperValues[2] = new SkillLevel(3, 120, 70);
            LevelperValues[3] = new SkillLevel(4, 150, 70);
            LevelperValues[4] = new SkillLevel(5, 180, 70);
        }
        //
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
        GameObject particle = PhotonNetwork.Instantiate("Character/Ahri/e/Charm", weapon.transform.position, Quaternion.identity);
        CharmParticle script = particle.GetComponent<CharmParticle>();
        script.init(this.transform.gameObject);
    }
}
