using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    Camera mainCamera;
    Animator animator;
    public Vector3 targetpos;
    Transform Target;

    private NavMeshAgent agent;
 
    [HideInInspector]
    public float speed = 10.0f;

    private void Awake()
    {
        //path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        targetpos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.collider.CompareTag("Map"))
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
                }
            }            
        }
        if(move(targetpos))
        {
            turn(targetpos);
        }

    }

    private bool move(Vector3 target)
    {
        float dis = Vector3.Distance(transform.position, target);

        if(dis >= 0.01f)
        {            
            //if (agent.CalculatePath(target,agent.path))
            {
                agent.SetDestination(target);
                // ani
            }
            return true;
        }
        
        return false;
    }
    
    private void turn(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if(dir == Vector3.zero)
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

        if(newTarget.gameObject.GetComponent<Outline>())
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
