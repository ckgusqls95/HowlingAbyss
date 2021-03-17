using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
public class TempestParticle : MonoBehaviour
{
    #region member
    ParticleSystem[] psArray;
    private Tempest parent;
    float elapsedTime;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
        psArray = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1.0f)
        {
            Object.Destroy(this.gameObject);
        }
    }
    public void init(GameObject parentObject)
    {
        parent = parentObject.GetComponent<Tempest>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parent)
        {
            if (other.TryGetComponent<Units>(out Units script))
            {
                if ((script.UnitTag == UnitsTag.Minion ||
                    script.UnitTag == UnitsTag.Champion) &&
                    !other.CompareTag(parent.tag))
                {
                    Units unit = parent.transform.GetComponent<Units>();

                    float Damage = unit.Attack(AttackType.AP_SKILL, parent.skillFactor, parent.LevelperValues[parent.CurrentLevel].addDamage);
                    float Suffer = 0.0f;

                    {
                        Suffer = script.hit(AttackType.AP_SKILL, Damage, parent.GetComponent<Units>() , unit.UnitStatus.magicPenetration);
                    }
                    
                }
            }
        }
    }
}