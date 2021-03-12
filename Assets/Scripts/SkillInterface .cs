using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkillSystem
{
    public enum skilltype
    {
        PASSIVE,
        ACTIVE
    }
    public enum PlayerState
    {
        IDLE,
        ATTACK,
        SKILL,
        ATTACKED,
        AFTERSKILL,
        HIT,
        CROWNCONTROL,
        USED,
        KILL,
        LEVELUP
    }
    public class Skill : MonoBehaviour
    {
        [SerializeField]
        protected Sprite skillImgae;

        [SerializeField]
        protected GameObject particleObj;
        public GameObject ParticleObj { get { return particleObj; } set { particleObj = value; } }

        [SerializeField]
        protected ParticleSystem particleSystemPrefab;
        public ParticleSystem ParticleSystemPrefab { get { return particleSystemPrefab; } set { particleSystemPrefab = value; } }

        [SerializeField]
        protected skilltype skillType;

        [SerializeField]
        protected float skillFactor = 0.0f;

        [SerializeField]
        protected Button skillButton;

        protected int skillLevel;
        protected float coolTime;
        protected float currentCoolTime;
        //private bool canUseSkill = true;

        public Sprite SkiilImage { get { return skillImgae; } }
        public skilltype SkillType { get { return skillType; } }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }
        protected virtual void Update()
        {
        }
        public virtual void Play(GameObject target = null) { }
        public virtual bool Try(PlayerState State = PlayerState.IDLE, GameObject target = null) { return true; }
        public virtual void Stop() { }
        public virtual void Destroy() { }

        // 쿨타임 hp OnClick 타겟팅(적 아군) 키보드입력 
        public skilltype TypeCheck()
        {
            return skillType;
        }

        protected IEnumerator ElapsedTimeCalculation()
        {
            while (currentCoolTime >= 0.01f)
            {
                currentCoolTime -= Time.deltaTime;

                if (currentCoolTime < -0.01f)
                {
                    currentCoolTime = 0.0f;
                }

                yield return new WaitForFixedUpdate();
            }
        }
        //IEnumerator CoolTime()
        //{
        //    while (skillFilter.fillAmount > 0)
        //    {
        //        skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

        //        yield return null;
        //    }

        //    canUseSkill = true;

        //    yield break;
        //}

        ////남은 쿨타임을 계산할 코루틴
        //IEnumerator CoolTimeCounter()
        //{
        //    while (currentCoolTime > 0)
        //    {
        //        yield return new WaitForSeconds(1.0f);

        //        currentCoolTime -= 1.0f;
        //        coolTimeCounter.text = "" + currentCoolTime;
        //    }
        //    coolTimeCounter.text = "";
        //    yield break;
        //}
    }

    //public class ignite : Skill , SkillInterface
    //{
    //    public void Play()
    //    {
    //        base.BasePlay();
    //    }
    //    public void Stop()
    //    {
    //        base.BaseStop();
    //    }
    //    public bool Try()
    //    {
    //        base.BaseTry();
    //        //if(Character.Target == Champion)
    //        return true;
    //    }
    //}

    //public class deletest
    //{
    //    public delegate float DamagePower();
    //    DamagePower regist;

    //    public void AddRegist(DamagePower added)
    //    {
    //        regist += added;
    //    }

    //    void damagegyesan()
    //    {
    //        float registpower = 0;
    //        registpower += regist();

    //    }
    //}
}
