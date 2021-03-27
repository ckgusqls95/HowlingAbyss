using System.Collections;
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
    
    private RaycastHit[] hits;
    private Champion champion;
    private InventorySystem inventory;
    private PhotonView PV;
    [HideInInspector]
    public float speed = 10.0f;
    [HideInInspector]
    public Gold wallet;
    [HideInInspector]
    public GameObject Rallypoint;
    Camera mainCamera;
    public Vector3 targetpos;
    public bool isStopMove;
    private bool isAttack;
    Transform Target;

    Dictionary<KeyCode, Action> keyDictionary;

    private Animator animator;
    #endregion

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        naviAgent = GetComponent<NavMeshAgent>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        GameObject.FindWithTag("Canvas").GetComponentInChildren<Gold>().player = this;
        targetpos = this.transform.position;
        champion = GetComponentInChildren<Champion>();
        animator = GetComponentInChildren<Animator>();
        isStopMove = false;
        isAttack = false;
        if(PV.IsMine)
        {
            GameObject RallyParticle = Resources.Load("click/select 1") as GameObject;
            Rallypoint = Instantiate(RallyParticle);
            Rallypoint.SetActive(false);
        }
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
        inventory = GameObject.Find("Canvas").GetComponentInChildren<InventorySystem>();
        wallet = inventory.transform.FindChild("Gold").GetComponent<Gold>();
        wallet.EarnGold(500);
    }

    #region KeyBoardInput
    private void KeyDown_A()
    {
        champion.MeleeAttack();
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
        if (!isStopMove && PV.IsMine)
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
                    champion.Target = null;
                    if(PV.IsMine)
                    {
                        Vector3 newRallyPosition = targetpos;
                        newRallyPosition.y = -1.2f;
                        Rallypoint.transform.position = newRallyPosition;
                        Rallypoint.SetActive(true);
                    }
                }
                else
                {
                    if (hit.collider.transform == champion.transform) return;
                    if (hit.collider.CompareTag("particle")) return;
                    
                    if (PV.IsMine)
                    {
                        Rallypoint.SetActive(false);
                    }
                    TargetOutline(hit.transform);

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

                    champion.Targeting(hit.transform.gameObject);
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

        #region attack
        if(champion.Target)
        {
            float distance = Vector3.Distance(gameObject.transform.position, champion.Target.transform.position);
            if (distance <= champion.UnitSight.attackRange && !champion.Target.transform.CompareTag(gameObject.tag))
            {
                animator.SetBool("run", false);
                animator.SetBool("attack", isAttack = true);
                naviAgent.isStopped = true;
                StartCoroutine(attack(champion.Target.transform.position));

                return false;
            }
        }
        #endregion

        Vector3 pos = transform.position;
        pos.y = 0.0f;
        target.y = 0.0f;

        float dis = Vector3.Distance(pos, target);

        if (dis >= 0.5f)
        {
            naviAgent.isStopped = false;
            naviAgent.SetDestination(target);
            animator.SetBool("run", true);
            animator.SetBool("attack", isAttack = false);
            return true;
        }

        animator.SetBool("run", false);
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
            360.0f);
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

    IEnumerator attack(Vector3 pos)
    {
        while((animator.GetCurrentAnimatorStateInfo(0).normalizedTime) <= 0.8f && isAttack == true)
        {
            turn(pos);
            yield return new WaitForFixedUpdate();
        }

        if(isAttack)
        {
            animator.SetBool("attack",isAttack = false);
        }

    }

    public void stop()
    {
        isStopMove = true;
        naviAgent.isStopped = isStopMove;
    }

    public void Play()
    {
        isStopMove = false;
        naviAgent.isStopped = isStopMove;
    }

}
