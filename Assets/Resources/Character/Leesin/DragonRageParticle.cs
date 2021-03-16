using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
public class DragonRageParticle : MonoBehaviour
{
    GameObject createObj;
    Vector3 dir;
    Vector3 startpos;
    PlayerController pc;
    const float range = 10.0f;
    const float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(Vector3.Distance(startpos,this.transform.parent.position) <= range)
        {
            this.transform.parent.position += dir * Time.deltaTime * speed;
        }
        else
        {
            if(pc)
            {
                pc.Play();
            }

            Object.Destroy(this.transform.gameObject);
        }
    }


    public void init(GameObject obj)
    {
        createObj = obj;
        dir = obj.transform.forward;
        startpos = this.transform.position;

        if(transform.parent.TryGetComponent<PlayerController>(out pc))
        {
            pc.stop();
        }
             
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(createObj.tag == "Red" ? "Blue" : "Red"))
        {
            if (other.TryGetComponent<Units>(out Units script))
            {
                DragonRage rage = createObj.GetComponent<DragonRage>();
                Units unit = createObj.GetComponent<Units>();

                float Damage = unit.Attack(AttackType.AD_SKILL, rage.skillFactor, rage.LevelperValues[rage.CurrentLevel].addDamage);
                float Suffer = 0.0f;

                {
                    Suffer = script.hit(AttackType.AD_SKILL, Damage, unit.UnitStatus.armorPenetration);
                }

            }
        }

    }
}
