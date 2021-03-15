using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class basicAttack : MonoBehaviour
{
    private Units parentMinion = null;
    private GameObject target = null;
    const float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            Vector3 current = transform.position;
            Vector3 targetposition = target.transform.position;
            targetposition.y = current.y;
            transform.position = Vector3.MoveTowards(current, targetposition, speed * Time.deltaTime);
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
    }

    public void init(Units Minion)
    {
        parentMinion = Minion;
        target = parentMinion.Target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentMinion.Target)
        {
            if (other.transform.gameObject ==target)
            {
                Debug.Log("Hit");

                float Damage = parentMinion.Attack(AttackType.MEELEE);
                float Suffer;
                if (other.TryGetComponent<Units>(out var script))
                {
                    Suffer = script.hit(AttackType.MEELEE, Damage, parentMinion.UnitStatus.armorPenetration);
                }
            }
        }

        Object.Destroy(this.gameObject);
    }
}
