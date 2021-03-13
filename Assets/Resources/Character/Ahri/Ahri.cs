using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Photon.Pun;
using Unit;

public class Ahri : Champion
{
    [SerializeField]
    private UnitData AhriData;

    private Animator animator;
    const int Skillcount = 4;
    string SFXResource = "AhriSFX/SFX/";
    string voResource = "AhriSFX/vo/";
    List<string> idlesounds = new List<string>();

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        championSkill = new Skill[Skillcount];
        championSkill[0] = GetComponent<OrbofDeception>();
        championSkill[1] = GetComponent<FoxFire>();
        championSkill[2] = GetComponent<Charm>();
        championSkill[3] = GetComponent<SpritRush>();

        SoundManager.instance.LoadClip(SFXResource);
        string[] vo = SoundManager.instance.LoadClip(voResource);

        for (int i = 0; i < vo.Length; i++)
        {
            if (vo[i].Contains("idle"))
            {
                idlesounds.Add(vo[i]);
            }
        }

        UnitStatus = Status.Initialize(AhriData.initStatus);
        UnitSight = AhriData.UnitSight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseSkillE()
    {
        const int index = 2;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

        return;
    }

    public override void UseSkillQ()
    {
        const int index = 0;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
        {
            championSkill[index].Play();
        }

        return;
    }

    public override void UseSkillR()
    {
        const int index = 3;

        if (championSkill[index].Try(PlayerState.SKILL, Target))
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

        return;
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
    }

    public void IdleSound()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources != null)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if(sources[i].isPlaying)
                {
                    return;
                }
            }
        }

        int index = Random.Range(0, idlesounds.Count);
        SoundManager.instance.PlaySE(idlesounds[index], gameObject);
    }
}
