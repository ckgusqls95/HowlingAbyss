using Photon.Pun;
using System.Collections;
using Unit;
using UnityEngine;
public class Turret : Units
{
    #region
    [SerializeField]
    private UnitData turretData;
    public GameObject particle;
    private GameObject joint;
    private LineRenderer linerender;
    private RaycastHit[] hits;
    private bool isRunningCoroutin = false;
    public GameObject icon;
    private int priority = int.MaxValue;
    private Units TargetScript;
    [SerializeField]
    private GameObject DestoryTurrent;

    [SerializeField]
    private GameObject ExplosionEffect;


    #endregion

    private void Awake()
    {
        linerender = GetComponent<LineRenderer>();

        GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Attach(this.transform.gameObject, icon);

        Transform[] childs = GetComponentsInChildren<Transform>();

        foreach (Transform child in childs)
        {
            if (child.name.Contains("joint"))
            {
                joint = child.gameObject;
                break;
            }
        }

        linerender.SetPosition(0, joint.transform.position);
        linerender.SetPosition(1, transform.position);

        linerender.startWidth = linerender.endWidth = 0.1f;
        linerender.enabled = false;

        UnitStatus = Status.Initialize(turretData.initStatus);
        UnitTag = UnitsTag.Turret;

    }

    private void Update()
    {
        if (UnitStatus.health <= 0.0f)
        {
            Die();
        }

        if (Target && !TargetScript.isDeath)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) <= turretData.UnitSight.attackRange)
            {
                DrawLine();

                if (!isRunningCoroutin)
                {
                    StartCoroutine("CannonFiring", 3.0f);
                }
            }
            else
            {
                linerender.SetPosition(1, joint.transform.position);
                linerender.enabled = false;

                if (isRunningCoroutin)
                {
                    StopCoroutine("CannonFiring");
                    isRunningCoroutin = false;
                }
                Target = null;
                TargetScript = null;
                priority = int.MaxValue;
            }

        }
        else
        {
            linerender.SetPosition(1, joint.transform.position);
            linerender.enabled = false;


            if (isRunningCoroutin)
            {
                StopCoroutine("CannonFiring");
                isRunningCoroutin = false; 
            }

            Target = null;
            TargetScript = null;
            priority = int.MaxValue;
        }

    }
    private void FixedUpdate()
    {
        TargetTracking();
    }

    void DrawLine()
    {
        if (!linerender.enabled)
        {
            linerender.enabled = true;
        }

        //if (linerender.GetPosition(1) != Target.transform.position)
        linerender.SetPosition(1, Target.transform.position);
    }

    void TargetTracking()
    {
        
        {
            hits = Physics.SphereCastAll(transform.position, turretData.UnitSight.aggroRange, Vector3.up, 0f);

            foreach (RaycastHit hit in hits)
            {
                if (gameObject == hit.transform.gameObject) continue;
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
                    else if (hit.transform.gameObject == null)
                    {
                        continue;
                    }

                    int tempPriority = Prioritization(hit.transform.gameObject);
                    if (priority == int.MaxValue || tempPriority < priority)
                    {
                        Target = hit.transform.gameObject;
                        priority = tempPriority;
                        TargetScript = script;
                    }

                }

            }
            if (Target)
                Debug.Log(Target.name);

            if (Target == null)
            {
                linerender.enabled = false;
            }

        }

    }
    IEnumerator CannonFiring(float cooltime)
    {
        isRunningCoroutin = true;

        while (cooltime > 1.0f && Target)
        {
            cooltime -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        if (Target)
        {
            CreateParticle();
        }

        isRunningCoroutin = false;
    }

    void CreateParticle()
    {
        if (Target)
        {
            GameObject obj;
            if (this.transform.CompareTag("Red"))
            {
                obj = PhotonNetwork.Instantiate("turret/RedTurret", joint.transform.position, Quaternion.identity);
            }
            else
            {
                obj = PhotonNetwork.Instantiate("turret/BlueTurretBeam", joint.transform.position, Quaternion.identity);
            }
            obj.GetComponent<Siege_CannonBall>().init(gameObject, Target);
        }
    }

    protected override void Die()
    {
        isDeath = true;
        GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Dettach(gameObject);
        Instantiate(DestoryTurrent, transform.position, transform.rotation);
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Object.Destroy(gameObject);

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
                if (targetscript)
                {
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
}
