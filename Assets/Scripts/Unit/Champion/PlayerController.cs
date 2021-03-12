﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;
using Unit;
using System;

public class PlayerController : MonoBehaviour
{
    #region member
    public NavMeshAgent naviAgent;
    public GameObject target = null;
    private RaycastHit[] hits;
    private string championName;
    public string ChampionName { get { return championName; } set { championName = value; } }
    private int[] spellIndex;
    public int[] SpellIndex { get { return spellIndex; } set { spellIndex = value; } }
    private int masteryIndex;
    public int MasteryIndex { get { return masteryIndex; } set { masteryIndex = value; } }
    private Champion champion;
    private InventorySystem inventory;
    private PhotonView PV;
    public int gold = 0;

    [HideInInspector]
    public float speed = 10.0f;

    Camera mainCamera;
    public Vector3 targetpos;
    public bool isStopMove;
    Transform Target;

    Dictionary<KeyCode, Action> keyDictionary;

    private Animator animator;
    #endregion

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        naviAgent = GetComponent<NavMeshAgent>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        targetpos = this.transform.position;
        champion = GetComponentInChildren<Champion>();
        animator = GetComponentInChildren<Animator>();
        isStopMove = false;

        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { KeyCode.A, KeyDown_A },
            { KeyCode.Q, KeyDown_Q },
            { KeyCode.W, KeyDown_W },
            { KeyCode.E, KeyDown_E },
            { KeyCode.R, KeyDown_R }
        };

    }

    // Start is called before the first frame update
    void Start()
    {
        //champion.ChampionSkill[2].p;
    }

    #region KeyBoardInput
    private void KeyDown_A()
    {
        champion.MeeleeAttack();
    }
    private void KeyDown_Q()
    {
        champion.UseSkillQ();
    }
    private void KeyDown_W()
    {
        champion.UseSkillW();
    }
    private void KeyDown_E()
    {
        champion.UseSkillE();
    }
    private void KeyDown_R()
    {
        champion.UseSkillR();
    }
    #endregion
    // Update is called once per frame

    void Update()
    {
        // if(PV.IsMine)
        {
            Targeting();
            if (Input.anyKeyDown)
            {
                foreach (var dic in keyDictionary)
                {
                    if (Input.GetKeyDown(dic.Key))
                    {
                        dic.Value();
                    }
                }
            }
        }


    }

    private void Targeting()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Map"))
                {
                    targetpos = hit.point;
                    
                }
                else
                {
                    if (hit.collider.transform == this.transform) return;
                    if (hit.collider.CompareTag("particle")) return;
                    TargetOutline(hit.transform);
                    // minion champion etc
                    //
                    if (hit.collider.CompareTag("Shop") && hit.transform.parent.CompareTag(this.transform.tag))
                    {
                        if (GameObject.FindWithTag("Canvas").TryGetComponent<IngameUIController>(out IngameUIController UI))
                        {
                            UI.PopupShop();
                        }
                        if(hit.transform.TryGetComponent<Merchant>(out Merchant merchant))
                        {
                            merchant.ClickMerchant();
                        }
                    }

                    Champion script = GetComponentInChildren<Champion>();

                    if (script)
                    {
                        script.Targeting(hit.transform.gameObject);
                    }


                }
            }
        }

        if (MovePlayer(targetpos))
        {
            turn(targetpos);
        }
    }

    private bool MovePlayer(Vector3 target)
    {
        if (isStopMove) return false; // animator or force move transform 

        //if(!naviAgent.CalculatePath(target,naviAgent.path))
        //{
        //    animator.SetBool("run", false);
        //    return false;
        //}

        Vector3 pos = transform.position;
        pos.y = 0.0f;
        target.y = 0.0f;

        float dis = Vector3.Distance(pos, target);

        if (dis >= 0.5f)
        {
            //if (agent.CalculatePath(target,agent.path))
            {
                naviAgent.SetDestination(target);
                animator.SetBool("run", true);
                // ani
            }
            return true;
        }
        else
        {
            animator.SetBool("run", false);
        }

        return false;
    }
    private void turn(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if (dir == Vector3.zero)
        {
            return;
        }

        Quaternion Rotation = Quaternion.LookRotation(dir);

        this.transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Rotation,
            360.0f * Time.deltaTime);
    }
    private void TargetOutline(Transform newTarget)
    {
        if (Target == newTarget) return;

        if (newTarget.gameObject.GetComponent<Outline>())
        {
            newTarget.gameObject.GetComponent<Outline>().OutlineColor = new Color(1, 0, 0);
            newTarget.gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
            newTarget.gameObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            Outline outline = newTarget.gameObject.AddComponent<Outline>();
            outline.OutlineColor = new Color(1, 0, 0);
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.enabled = true;
        }

        if (Target)
        {
            Target.gameObject.GetComponent<Outline>().enabled = false;
        }

        Target = newTarget;
    }

}