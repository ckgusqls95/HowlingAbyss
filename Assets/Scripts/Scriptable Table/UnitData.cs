using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InitStatus
{
    public float health;
    public float healthRegen;
    public float cost;
    public float costRegen;
    public float attackSpeed;
    public float movementSpeed;

    public float attackDamage;
    public float armor;
    public float magicResist;

    public int killGold;
    public static InitStatus operator +(InitStatus s1, InitStatus s2)
    {
        InitStatus status = new InitStatus();
        status.armor = s1.armor + s2.armor;
        status.attackDamage = s1.attackDamage + s2.attackDamage;
        status.attackSpeed = s1.attackSpeed * s2.attackSpeed;
        status.cost = s1.cost + s2.cost;
        status.health = s1.health + s2.health;
        status.magicResist = s1.magicResist + s2.magicResist;
        status.movementSpeed = s1.movementSpeed + s2.movementSpeed;
        return status;
    }
}

[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Unit Data", order = int.MaxValue - 2)]
[System.Serializable]
public class UnitData : ScriptableObject
{
    [SerializeField]
    public InitStatus growthStatus;
    [SerializeField]
    public InitStatus initStatus;

    public InitStatus GrowthStatus { get { return growthStatus; } }

    [System.Serializable]
    [SerializeField]
    public struct Sight
    {
        public float sightRange;
        public float attackRange;
        public float aggroRange;
    }

    [SerializeField]
    private Sight unitSight;

    public Sight UnitSight { get { return unitSight; }  } 

}