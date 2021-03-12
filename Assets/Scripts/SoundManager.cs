using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class sound
{
    public string name;
    public AudioClip clip;

}

public class SoundManager : MonoBehaviour
{
    #region member
    public List<AudioSource> AudioEffect;
    private Dictionary<string, int> playSoundName = new Dictionary<string, int>();
    //private Dictionary<string, AudioSource> AudioEffect;
    public AudioSource AudioBGM;

    public List<sound> Effectsounds;
    public List<sound> BGMsound;
    #endregion

    #region Singleton
    public static SoundManager instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Object.Destroy(gameObject);
            return;
        }

        AudioBGM = this.gameObject.AddComponent<AudioSource>();
        const string BasicSoundPath = "BasicSound";
        LoadClip(BasicSoundPath);

        const string BGM = "BGM";
        string[] bgmList = LoadBGMClip(BGM);
        PlayBGM(bgmList[0]);
    }
    #endregion
    
    public void PlaySE(string name)
    {
        for (int i = 0; i < Effectsounds.Count; i++)
        {
            if(Effectsounds[i].name.CompareTo(name) == 0)
            {
                for (int j = 0; j < AudioEffect.Count; j++)
                {
                   if(!AudioEffect[j].isPlaying)
                    {
                        if (playSoundName.ContainsKey(name))
                        {
                            playSoundName[name] = j;
                        }
                        else
                        {
                            playSoundName.Add(name,j);                        
                        }

                        AudioEffect[j].clip = Effectsounds[i].clip;
                        AudioEffect[j].Play();                        
                        return;
                    }
                }

                playSoundName.Add(name, playSoundName.Count);
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = Effectsounds[i].clip;
                source.Play();
                AudioEffect.Add(source);
                return;
            }            
        }

        Debug.Log("Empty Sound Effect");

    }

    public void PlayBGM(string name)
    {
        for (int i = 0; i < BGMsound.Count; i++)
        {
            if(BGMsound[i].name.CompareTo(name) == 0)
            {
                if(AudioBGM && AudioBGM.isPlaying)
                {
                    AudioBGM.Stop();
                }

                AudioBGM.clip = BGMsound[i].clip;
                AudioBGM.Play();
                return;
            }
            
            AudioBGM.clip = BGMsound[i].clip;
            AudioBGM.Play();

            return;
        }
    }

    public void StopSE(string name)
    {
        AudioEffect[playSoundName[name]].Stop();
    }

    public void stopAllSE()
    {
        for (int i = 0; i < AudioEffect.Count; i++)
        {
            AudioEffect[i].Stop();
        }
    }

    public bool AddSE(AudioClip clip,string name = null)
    {
        for (int i = 0; i < Effectsounds.Count; i++)
        {
            if(Effectsounds[i].name.CompareTo(name) == 0)
            {
                return false;
            }
        }

        if (clip)
        {
            sound newEffect = new sound();
            newEffect.clip = clip;

            if (name == null)
            {
                newEffect.name = clip.name;
            }
            Effectsounds.Add(newEffect);
            return true;
        }

        return false;

    }

    public bool AddBGM(AudioClip clip,string name = null)
    {
        for (int i = 0; i < BGMsound.Count; i++)
        {
            if(BGMsound[i].name == name)
            {
                return false;
            }
        }

        if(clip)
        {
            sound BGM = new sound();
            BGM.clip = clip;
            if(name == null)
            {
                BGM.name = clip.name;
            }
            BGMsound.Add(BGM);
            return true;
        }

        return false;
    }

    public string[] LoadClip(string Path)
    {
        AudioClip[] clips;
        Object[] objs = Resources.LoadAll(Path, typeof(AudioClip));
        clips = new AudioClip[objs.Length];
        string[] names = new string[clips.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            clips[i] = objs[i] as AudioClip;
            AddSE(clips[i]);
            names[i] = clips[i].name;
        }

        return names;
    }

    public string[] LoadBGMClip(string Path)
    {
        AudioClip[] clips;
        Object[] objs = Resources.LoadAll(Path, typeof(AudioClip));
        clips = new AudioClip[objs.Length];
        string[] names = new string[clips.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            clips[i] = objs[i] as AudioClip;
            AddBGM(clips[i]);
            names[i] = clips[i].name;
        }

        return names;



    }
    

}
