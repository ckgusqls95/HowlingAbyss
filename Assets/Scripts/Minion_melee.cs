using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;
using UnityEngine.UI;
using Unit;
public class Minion_melee : Units
{
    #region member
    private Animator animator;
    [SerializeField]
    private UnitData minionMeleeData;
    private NavMeshAgent agent;
    private GameObject goal;
    private RaycastHit[] hits;

    private int priority = int.MaxValue;
    const float searchcooltime = 3.0f;
    private float elapsedSearchTime = 0.0f;

    private HUD__HP HUD;

    [SerializeField]
    private Collider StaffCol;

    #endregion

    private void Awake()
    {
        
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();

        GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Attach(this.transform.gameObject);

        goal = GameObject.Find(this.transform.tag == "Red" ? "Nexus_blue" : "Nexus_red");

        agent.SetDestination(goal.transform.position);

        unitTag = UnitsTag.Minion;

        UnitStatus = Status.Initialize(minionMeleeData.initStatus);

        HUD = GetComponentInChildren<HUD__HP>();

        StaffCol.transform.GetComponent<Staff>().init(this.GetComponent<Minion_melee>());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if(UnitStatus.health < 0.0f && !isDeath)
        {
            const int DeathAnimaton = 4;
            animator.SetInteger("death", Random.Range(0, DeathAnimaton));
            //animator.SetTrigger("deathTrigger");
            isDeath = true;
        }

        if (isDeath) return;

        HUD.HpApply(UnitStatus.health, UnitStatus.Maxhealth);

        TargetTracking();

        if (Target && !Target.GetComponent<Units>().isDeath)
        {
            float dist = Vector3.Distance(this.transform.position, Target.transform.position);
            if (dist <= minionMeleeData.UnitSight.attackRange &&
                (animator.GetCurrentAnimatorStateInfo(0).IsName("Run")
                 || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
            {
                agent.isStopped = true;
                StartCoroutine("AttackAnim");
                animator.SetBool("attack", true);
                animator.SetBool("run", false);

                Vector3 dest = Target.transform.position - this.transform.position;
                dest.Normalize();
                this.transform.rotation = Quaternion.LookRotation(dest);
            }
            else if (dist > minionMeleeData.UnitSight.attackRange)
            {
                StopCoroutine("AttackAnim");
                animator.SetBool("attack", false);
                animator.SetBool("run", true);

                agent.SetDestination(Target.transform.position);
                Target = null;

                if (agent.isStopped)
                    agent.isStopped = false;
            }

        }
        else if (Target && Target.GetComponent<Units>().isDeath)
        {
            Target = null;
        }

    }

    IEnumerator AttackAnim()
    {
        int attackAnimNum = 0;

        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    attackAnimNum = 1;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                {
                    attackAnimNum = 2;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
                {
                    attackAnimNum = 3;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3b"))
                {
                    attackAnimNum = 0;
                }
                animator.SetInteger("attackNum", attackAnimNum);

                yield return new WaitForSeconds(1.0f);
            }

            yield return null;
        }

    }

    void TargetTracking()
    {
        if (Target == null)
        {
            hits = Physics.SphereCastAll(transform.position, minionMeleeData.UnitSight.aggroRange, Vector3.up, 0f);

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
                    else if (script.isDeath)
                    {
                        continue;
                    }

                    int tempPriority = Prioritization(hit.transform.gameObject);
                    if (priority == int.MaxValue)
                    {
                        Target = hit.transform.gameObject;
                        priority = tempPriority;
                    }
                    else if (tempPriority < priority)
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

    }
 

    public override void LevelUp()
    {
        base.LevelUp();
    }

    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
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
            if (enemyTarget)
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
                switch (enemyTag)
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

    private void SetActiveCollider()
    {
        if(StaffCol.enabled)
        {
            StaffCol.enabled = false;
        }
        else
        {
            StaffCol.enabled = true;
        }
    }
    private void DeathMinion()
    {
        GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Dettach(gameObject);
        Object.Destroy(this.gameObject);
    }
}


