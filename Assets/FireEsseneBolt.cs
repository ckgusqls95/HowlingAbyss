using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
public class FireEsseneBolt : MonoBehaviour
{
    const float speed = 5.0f;
    const float Range = 3.0f;
    GameObject parent;
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
            Object.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            SpritRush foxfire = parent.GetComponent<SpritRush>();
            Units unit = parent.GetComponent<Units>();

            float Damage = unit.Attack(AttackType.AP_SKILL, foxfire.skillFactor, foxfire.LevelperValues[foxfire.CurrentLevel].addDamage);
            float Suffer = 0.0f;
            if (other.TryGetComponent<Units>(out var script))
            {
                Suffer = script.hit(AttackType.AP_SKILL, Damage, parent.GetComponent<Ahri>(), unit.UnitStatus.magicResist);
            }
        }
        Object.Destroy(gameObject);
    }


    public void Init(GameObject _Parent, GameObject _Target)
    {
        parent = _Parent;
        target = _Target;
    }
}
