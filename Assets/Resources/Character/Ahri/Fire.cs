using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    const float speed = 5.0f;
    const float Range = 3.0f;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);

        }
        else
        {
            transform.RotateAround(this.transform.parent.transform.position, Vector3.down, speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            RaycastHit[] hits;

            hits = Physics.SphereCastAll(transform.position, Range, transform.up, 0.0f);
            {
                foreach (RaycastHit hit in hits)

                if (hit.transform.CompareTag(transform.parent.tag == "Red" ? "Blue" : "Red"))
                {
                    target = hit.transform.gameObject;
                    this.transform.parent = null;
                    break;
                }

            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            Object.Destroy(this.gameObject);
        }
    }

}
