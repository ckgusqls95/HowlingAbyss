using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unit;
using UnityEngine.AI;

public class Minion_range : Units
{
    #region member
    [SerializeField]
    private UnitData minionRangeData;
    private Animator animator;
    private NavMeshAgent agent;
    private GameObject goal;
    private RaycastHit[] hits;
    public GameObject fireball;
    private GameObject staff;
    
    public int priority = int.MaxValue;
    const float searchcooltime = 3.0f;
    private float elapsedSearchTime = 0.0f;
    private HUD__HP HUD;
    #endregion

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();

        GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Attach(this.transform.gameObject);

        Transform[] childs = GetComponentsInChildren<Transform>();

        foreach (Transform child in childs)
        {
            if (child.name.Contains("Staff"))
            {
                staff = child.gameObject;
                break;
            }
        }

        goal = GameObject.Find(this.transform.tag == "Red" ? "Nexus_blue" : "Nexus_red");

        agent.SetDestination(goal.transform.position);

        unitTag = UnitsTag.Minion;
        UnitStatus = Status.Initialize(minionRangeData.initStatus);
        HUD = GetComponentInChildren<HUD__HP>();
    }

    private void FixedUpdate()
    {
        if (UnitStatus.health < 0.0f && !isDeath)
        {
            const int DeathAnimaton = 2;
            animator.SetInteger("death", Random.Range(0, DeathAnimaton));
            isDeath = true;
        }

        HUD.HpApply(UnitStatus.health, UnitStatus.Maxhealth);
        if (isDeath) return;

        TargetTracking();

        if (Target && !Target.GetComponent<Units>().isDeath)
        {
            float dist = Vector3.Distance(this.transform.position, Target.transform.position);
            if (dist <= minionRangeData.UnitSight.attackRange &&
                (animator.GetCurrentAnimatorStateInfo(0).IsName("Run")
                 || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
            {
                agent.isStopped = true;
                //StartCoroutine("AttackAnim");
                animator.SetBool("attack", true);
                animator.SetBool("run", false);

                Vector3 dest = Target.transform.position - this.transform.position;
                dest.Normalize();
                this.transform.rotation = Quaternion.LookRotation(dest);
            }
            else if (dist <= minionRangeData.UnitSight.attackRange)
            {
                Vector3 dest = Target.transform.position - this.transform.position;
                dest.Normalize();
                this.transform.rotation = Quaternion.LookRotation(dest);
            }
            else if (dist > minionRangeData.UnitSight.attackRange)
            {
                Target = null;
                //StopCoroutine("AttackAnim");
                animator.SetBool("attack", false);
                animator.SetBool("run", true);

                if (agent.isStopped)
                    agent.isStopped = false;
            }

        }
        else if(Target &&Target.GetComponent<Units>().isDeath)
        {
            Target = null;
        }

        if(elapsedSearchTime >= 0.0f)
        {
            elapsedSearchTime -= Time.deltaTime;
            elapsedSearchTime =  elapsedSearchTime < 0.0f ? 0.0f : elapsedSearchTime;
        }
    }

    protected override void Die()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, UnitSight.sightRange, Vector3.up, 0f);

        foreach (RaycastHit hit in hits)
        {
            if (this == hit.transform.gameObject) continue;
            if (hit.transform.CompareTag("particle")) continue;

            if (hit.transform.CompareTag(this.transform.CompareTag("Red") ? "Blue" : "Red"))
            {
                Champion script;

                if (hit.transform.TryGetComponent<Champion>(out script))
                {
                    script.UnitStatus.experience += this.UnitStatus.killExperience;
                }
            }
        }
    }

    void TargetTracking()
    {
        if (Target == null || elapsedSearchTime <= 0.0f )
        {
            hits = Physics.SphereCastAll(transform.position, minionRangeData.UnitSight.aggroRange, Vector3.up, 0f);
            foreach (RaycastHit hit in hits)
            {
                if (this == hit.transform.gameObject) continue;
                if (hit.transform.CompareTag("particle")) continue;
                if (hit.transform.CompareTag(this.transform.CompareTag("Red") ? "Blue" : "Red"))
                {
                    Units script;

                    if (!hit.transform.TryGetComponent<Units>(out script))
                    {
                        continue;
                    }
                    else if(script.isDeath)
                    {
                        continue;
                    }

                    int tempPriority = Prioritization(hit.transform.gameObject);
                    if (priority == int.MaxValue)
                    {
                        Target = hit.transform.gameObject;
                        priority = tempPriority;
                    }
                    else if(tempPriority < priority)
                    {
                        //Target = null;
                        Target = hit.transform.gameObject;
                        priority = tempPriority;                            
                    }

                }
            }

            if (Target == null)
            {
                animator.SetBool("run", true);
                animator.SetBool("attack", false);
                agent.isStopped = false;
                agent.SetDestination(goal.transform.position);
                priority = int.MaxValue;
            }
            else
            {
                elapsedSearchTime = searchcooltime;
            }
        }

        //fireball.Targeting(Target);
    }

    void CreateParticle()
    {
        if(Target)
        {
            GameObject obj = PhotonNetwork.Instantiate("ha_range/FireboltParticle", staff.transform.position, Quaternion.identity);
            obj.GetComponent<range_fireball>().init(gameObject, Target);
        }
    }

    private int Prioritization(GameObject enemy)
    {
        Units enemyscript = enemy.GetComponentInChildren<Units>();

        int newPriority = int.MaxValue;

        if (enemyscript)
        {
            UnitsTag enemyTag;
            UnitsTag enemyTargetTag;

            enemyTag = enemyscript.UnitTag;
            GameObject enemyTarget = enemyscript.Target;
            if(enemyTarget)
            {
                Units targetscript = enemyTarget.GetComponentInChildren<Units>();
                enemyTargetTag = targetscript.UnitTag;
                if (enemyTag == UnitsTag.Minion &&
                    enemyTargetTag == UnitsTag.Champion)
                {
                    newPriority = 2;
                }
                else if (enemyTag == UnitsTag.Minion &&
                    enemyTargetTag == UnitsTag.Minion)
                {
                    newPriority = 3;
                }
                else if (enemyTag == UnitsTag.Champion &&
                   enemyTargetTag == UnitsTag.Minion)
                {
                    newPriority = 5;
                }
            }
            else
            {
                switch(enemyTag)
                {
                    case UnitsTag.Minion:
                        newPriority = 6;
                        break;
                    case UnitsTag.Champion:
                        newPriority = 7;
                        break;
                    case UnitsTag.Turret:
                    case UnitsTag.Nexus:
                        newPriority = 4;
                        break;
                }
            }
        }

        return newPriority;
    }
    private void DeathMinion()
    {
        GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Dettach(gameObject);
        Object.Destroy(this.gameObject);
    }
}
