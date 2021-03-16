using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Unit
{
    #region Properties
    [System.Serializable]
    public struct Status
    {
        public int level;
        public float health;
        public float healthRegen;
        public float Maxhealth;

        public float cost;
        public float costRegen;
        public float maxCost;

        public float attackSpeed;
        public float movementSpeed;

        public float attackDamage;
        public float abilityPower;

        public float armor;
        public float magicResist;

        public float armorPenetration; // 방관
        public float magicPenetration; // 마관

        public float lifeSteal; // 생명력 흡수
        public float spellVamp; // 주문 흡혈

        public float abilityHaste; // 스킬가속
        public float tenacity;     // 강인함

        public float experience;

        public float killGold;
        public float killExperience;

        //public static Status operator +(Status s1, Status s2)
        //{
        //    Status status = new Status();
        //    status.abilityPower = s1.abilityPower + s2.abilityPower;
        //    status.armor = s1.armor + s2.armor;
        //    status.attackDamage = s1.attackDamage + s2.attackDamage;
        //    status.attackSpeed = s1.attackSpeed * s2.attackSpeed;
        //    status.cost = s1.cost + s2.cost;
        //    status.health = s1.health + s2.health;
        //    status.magicResist = s1.magicResist + s2.magicResist;
        //    status.movementSpeed = s1.movementSpeed + s2.movementSpeed;
        //    return status;
        //}
        public static Status Initialize(InitStatus init)
        {
            Status temp = new Status();
            temp.health = init.health;
            temp.Maxhealth = init.health;
            temp.healthRegen = init.healthRegen;

            temp.cost = init.cost;
            temp.maxCost = init.cost;
            temp.costRegen = init.costRegen;

            temp.attackDamage = init.attackDamage;
            temp.attackSpeed = init.attackSpeed;

            temp.armor = init.armor;
            temp.magicResist = init.magicResist;

            temp.movementSpeed = init.movementSpeed;

            temp.killGold = init.killGold;
            return temp;
        }
    }
    #endregion

    #region Override
    //public virtual void Move() { }
    //public virtual void LevelUp()
    //{
    //    ResultInfo.status += growthStatus;
    //}
    #endregion

    #region Tags
    public enum UnitsTag
    {
        Minion,
        Champion,
        Turret,
        Nexus
    }

    public enum AttackType
    {
        MEELEE,
        AD_SKILL,
        AP_SKILL,
        TRUE_DAMAGE
    }

    public enum UnitEventType
    {
        Death,
        Hit
    }
    #endregion

    public class Units : MonoBehaviourPun, IPunObservable
    {

        #region Members
        public UnitsTag unitTag;
        protected PhotonView PV;
        public Status UnitStatus;
        public UnitData.Sight UnitSight;
        public UnitsTag UnitTag { get { return unitTag; } set { unitTag = value; } }

        //[HideInInspector]
        public GameObject Target = null;
        public bool isDeath = false;
        #endregion

        //public virtual bool Attack(Units unit) { return false; }
        //public virtual void Move() { }
        public virtual void LevelUp()
        {

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            //if (stream.IsWriting)
            //{
            //    stream.SendNext(unitStatus);
            //}
            //else
            //{
            //    this.unitStatus = (Status)stream.ReceiveNext();
            //}

        }

        public float hit(AttackType type, float damage, float Penetration = 0.0f)
        {
            const float percent = 100.0f;
            float SufferDamege = 0.0f;
            float Resist = 0.0f;

            switch (type)
            {
                case AttackType.MEELEE:
                case AttackType.AD_SKILL:
                    Resist = UnitStatus.armor;
                    break;
                case AttackType.AP_SKILL:
                    Resist = UnitStatus.magicResist;
                    break;
                case AttackType.TRUE_DAMAGE:
                    Resist = 0.0f;
                    break;
                default:
                    Debug.LogError("Error");
                    break;
            }

            SufferDamege = (percent / (percent + Resist - Penetration)) * damage;

            UnitStatus.health -= SufferDamege;

            return SufferDamege;
        }


        /// <summary>
        /// damageFactor = Base Damage + (damageTypeStatus* damage Ratio)
        /// The attack type is owned by the Units Class.
        /// </summary>
        public float Attack(AttackType type,float factor = 1.0f,float addDamege = 0.0f)
        {
            float Damege = 0.0f;
            Damege += addDamege;

           switch(type)
            {
                case AttackType.MEELEE:
                case AttackType.AD_SKILL:
                    {
                        Damege += (UnitStatus.attackDamage * factor); 
                    }
                    break;
                case AttackType.AP_SKILL:
                    {
                       Damege += (UnitStatus.abilityPower * factor);
                    }
                    break;
                default:
                    Debug.LogError("Default Damege Calculation Error");
                    break;
            }

            return Damege;
        }
           
        public virtual void receiveEvent(UnitsTag otherTag,UnitEventType eventType,Status otherStatus,GameObject _target = null)
        {
            switch (eventType)
            {
                case UnitEventType.Death:
                    if (otherTag == UnitsTag.Minion && 
                        unitTag == UnitsTag.Champion)
                    {
                        float exp = otherStatus.killExperience;
                        float gold = otherStatus.killGold;
                    }
                    break;
                case UnitEventType.Hit:
                    if(otherTag == UnitsTag.Champion &&
                        unitTag == UnitsTag.Minion &&
                        _target)
                    {
                        // 미니언 우선순위 변경
                    }
                    break;
            }

        }

        public virtual void GiveEvent(UnitEventType eventType)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, UnitSight.sightRange, Vector3.up, 0f);

            foreach (RaycastHit hit in hits)
            {
                if (this == hit.transform.gameObject) continue;
                if (hit.transform.CompareTag("particle")) continue;

                if(hit.transform.CompareTag(transform.tag))
                {
                    Units script;

                    if (hit.transform.TryGetComponent<Units>(out script))
                    {
                        switch (eventType)
                        {
                            case UnitEventType.Death:
                                script.receiveEvent(unitTag, eventType, UnitStatus);
                                break;
                            case UnitEventType.Hit:
                                script.receiveEvent(unitTag, eventType, UnitStatus, Target);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

        }

    }

}
