using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;

public class SonicwaveParticle : MonoBehaviour
{
    #region member
    ParticleSystem[] psArray;
    private Vector3 dir;
    private float speed = 10.0f;
    private SonicWave parent;
    private Vector3 startpos;
    private Collider col;
    #endregion

    private void Awake()
    {
        psArray = GetComponentsInChildren<ParticleSystem>();
        col = GetComponent<Collider>();
        foreach(ParticleSystem ps in psArray)
        {
            ps.Play();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        if(Vector3.Distance(startpos,this.transform.position) >= 10.0f)
        {
            Object.Destroy(gameObject);
        }
        else
        {
            col.transform.position += dir * Time.deltaTime * speed;            
        }

    }

    public void init(GameObject parentObject)
    {
        parent = parentObject.GetComponent<SonicWave>();
        startpos = transform.position;

        dir = parentObject.transform.forward;
        dir.y = 0.0f;
        dir = dir.normalized; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(parent.tag)) return;
        if (other.tag == "Untagged") return;
        if (parent)
        {
            if (other.TryGetComponent<Units>(out Units script))
            {
                if ((script.UnitTag == UnitsTag.Minion ||
                    script.UnitTag == UnitsTag.Champion) &&
                    !other.CompareTag(parent.tag))
                {
                    parent.HitObject = other.transform.gameObject;
                    Units unit = parent.transform.GetComponent<Units>();

                    float Damage = unit.Attack(AttackType.AD_SKILL, parent.skillFactor, parent.LevelperValues[parent.CurrentLevel].addDamage);
                    float Suffer = 0.0f;

                    {
                        Suffer = script.hit(AttackType.AD_SKILL, Damage, unit.UnitStatus.armorPenetration);
                    }
                    
                }
            }
        }

        {
            Object.Destroy(gameObject);
        }
    }

}
