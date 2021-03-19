using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using Photon.Pun;
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
    
    [SerializeField]
    private GameObject DestoryTurrent;

    [SerializeField]
    private GameObject ExplosionEffect;


    #endregion

    private void Awake()
    {
        linerender = GetComponent<LineRenderer>();

        GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Attach(this.transform.gameObject,icon);

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
        if(UnitStatus.health <= 0.0f)
        {
            isDeath = true;
            GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Dettach(gameObject);
            Instantiate(DestoryTurrent, transform.position, transform.rotation);
            Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
            Object.Destroy(gameObject);
        }

        if(Target && !Target.GetComponent<Units>().isDeath)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) <= turretData.UnitSight.attackRange)
            {
                DrawLine();
               
                if(!isRunningCoroutin)
                {
                    StartCoroutine("CannonFiring",3.0f);
                }
            }
            else
            {
                linerender.SetPosition(1, joint.transform.position);
                linerender.enabled = false;

                if(isRunningCoroutin)
                {
                    StopCoroutine("CannonFiring");
                    isRunningCoroutin = false;
                }
            }
             
        }
        else if (Target && Target.GetComponent<Units>().isDeath)
        {
            Target = null;
        }


        TargetTracking();

    }

    void DrawLine()
    {
        if (!linerender.enabled)
        {
            linerender.enabled = true;
        }

        if (linerender.GetPosition(1) != Target.transform.position)
            linerender.SetPosition(1, Target.transform.position);
    }

    void TargetTracking()
    {
        if(Target == null)
        {
            hits = Physics.SphereCastAll(transform.position, turretData.UnitSight.aggroRange, Vector3.up, 0f);

            foreach(RaycastHit hit in hits)
            {
                if (gameObject == hit.transform.gameObject) continue;
                if (hit.transform.CompareTag("particle")) continue;
                if (hit.transform.CompareTag(this.transform.CompareTag("Red") ? "Blue" : "Red"))
                {
                    Target = hit.transform.gameObject;
                    break;
                }

            }
            if(Target)
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
        if(Target)
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
       
    }
}
