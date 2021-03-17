using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
public class Staff : MonoBehaviour
{
    private Units parentMinion = null; 

    public void init(Units Minion)
    {
        parentMinion = Minion;
        Collider col = GetComponent<Collider>();
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(parentMinion.Target)
        {
            if(other.transform.gameObject == parentMinion.Target)
            {
                float Damage = parentMinion.Attack(AttackType.MELEE);
                float Suffer;
                if(other.TryGetComponent<Units>(out var script))
                {
                   Suffer = script.hit(AttackType.MELEE, Damage, null,parentMinion.UnitStatus.armorPenetration);
                }
            }
        }
    }
}
