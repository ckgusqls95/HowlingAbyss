using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbParticle : MonoBehaviour
{
    GameObject parent;
    Vector3 direction;
    const float speed = 3.0f;
    const float MaxDistacne = 1.0f;
    Vector3 startpos;
    bool isRetuned;

    private void Awake()
    {
        isRetuned = false;
    }

    private void FixedUpdate()
    {
        if(!isRetuned)
        {
            this.transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(startpos,this.transform.position) >= MaxDistacne)
            {
                isRetuned = true;
            }
        }
        else if(isRetuned)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, parent.transform.position, speed * Time.deltaTime);

            if(Vector3.Distance(this.transform.position,parent.transform.position) < MaxDistacne * 0.1f)
            {
                parent.GetComponent<OrbofDeception>().ActiveWeapon();
                Object.Destroy(this.gameObject);
            }
        }
    }

    public void init(GameObject obj)
    {
        parent = obj;
        direction = obj.transform.forward;
        direction = direction.normalized;
        startpos = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(parent.transform.tag == "Red" ? "Blue" : "Red"))
        {
            Debug.Log("Enemy orb hit!!");
        }
    }

}
