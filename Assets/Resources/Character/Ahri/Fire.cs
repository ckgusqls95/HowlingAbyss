using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class Fire : MonoBehaviour
{
    const float speed = 5.0f;
    const float Range = 3.0f;
    GameObject target;
    private GameObject parent = null;

    float duration = 1.5f;

    private void Awake()
    {
        
    }

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
        duration -= Time.deltaTime;

        if(duration <= 0.0f)
        {
            Object.Destroy(gameObject);
        }

        if (target == null)
        {
            RaycastHit[] hits;

            hits = Physics.SphereCastAll(transform.position, Range, transform.up, 0.0f);
            {
                foreach (RaycastHit hit in hits)

                    if (hit.transform.CompareTag(parent.tag == "Red" ? "Blue" : "Red"))
                    {
                        if (hit.transform.TryGetComponent<Units>(out var script) &&
                                script.UnitTag != UnitsTag.Turret &&
                                script.UnitTag != UnitsTag.Nexus)
                        {
                            target = hit.transform.gameObject;
                            this.transform.parent = null;
                            break;
                        }
                    }

            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            FoxFire foxfire = parent.GetComponent<FoxFire>();
            Units unit = parent.GetComponent<Units>();

            float Damage = unit.Attack(AttackType.AP_SKILL, foxfire.skillFactor, foxfire.LevelperValues[foxfire.CurrentLevel].addDamage);
            float Suffer = 0.0f;
            if (other.TryGetComponent<Units>(out var script))
            {
                Suffer = script.hit(AttackType.AP_SKILL, Damage, unit.UnitStatus.magicResist);
            }
            
        }

        Object.Destroy(gameObject);
    }


    public void Init(GameObject Parent)
    {
        parent = Parent;
    }
}
