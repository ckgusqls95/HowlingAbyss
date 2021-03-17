using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;
using Unit;

public class Leesin : Champion
{
    [SerializeField]
    private UnitData LeesinData;

    private Animator animator;
    const int Skillcount = 5;
    string SFXResource = "LeesinSFX/SFX/";
    string voResource = "LeesinSFX/vo/";
    
    List<string> idlesounds = new List<string>();

    [SerializeField]
    private Collider[] hands;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        championSkill = new Skill[Skillcount];
        championSkill[0] = GetComponent<SonicWave>();
        ChampionSkill[1] = GetComponent<SafeGuard>();
        ChampionSkill[2] = GetComponent<Tempest>();
        ChampionSkill[3] = GetComponent<DragonRage>();

        SoundManager.instance.LoadClip(SFXResource);
        string[] vo = SoundManager.instance.LoadClip(voResource);
        
        for (int i = 0; i < vo.Length; i++)
        {
            if(vo[i].Contains("idle"))
            {
                idlesounds.Add(vo[i]);
            }
        }

        UnitStatus = Status.Initialize(LeesinData.initStatus);
        UnitSight = LeesinData.UnitSight;

        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].GetComponent<Staff>().init(this);
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
    }

    public override void UseSkillQ()
    {
        const int index = 0;
        
        if(championSkill[index].Try(PlayerState.SKILL,Target))
        {
            championSkill[index].Play();
        }

        return;
    }

    public override void UseSkillW()
    {
        const int index = 1;
        
        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

    }

    public override void UseSkillE()
    {
        const int index = 2;
        
        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

    }

    public override void UseSkillR()
    {
        const int index = 3;
        
        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play(Target);
        }

    }

    public void playsound(string soundname)
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources != null)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i].Stop();
            }
        }

        SoundManager.instance.PlaySE(soundname, gameObject);

        SetCollision();
    }

    public void SetCollision()
    {
        for (int i = 0; i < hands.Length; i++)
        {
            if (hands[i].enabled)
                hands[i].enabled = false;
            else
                hands[i].enabled = true;
        }
    }

    public void IdleSound()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources != null)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].isPlaying)
                {
                    return;
                }
            }
        }

        int index = Random.Range(0, idlesounds.Count);
        SoundManager.instance.PlaySE(idlesounds[index], gameObject);
    }

    protected override void Die()
    {
        base.Die();
        if(!isDeath)
        {
            animator.SetTrigger("Death");
            isDeath = true;
            this.gameObject.GetComponent<PlayerController>().isStopMove = true;
            //Object.Destroy(gameObject);
        }
    }

}
