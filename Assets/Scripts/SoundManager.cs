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
    
    public void PlaySE(string name, GameObject soundplayer = null)
    {
        for (int i = 0; i < Effectsounds.Count; i++)
        {
            if(Effectsounds[i].name.CompareTo(name) == 0)
            {
                AudioSource[] sources = null;
                AudioSource source = null;
                if (soundplayer)
                {
                    sources = soundplayer.GetComponents<AudioSource>();
                    if(sources == null)
                    {
                        source= soundplayer.AddComponent<AudioSource>();
                        source.clip = Effectsounds[i].clip;
                        source.Play();
                    }
                    else
                    {
                        for (int k = 0; k < sources.Length; k++)
                        {
                            if(!sources[k].isPlaying)
                            {
                                source = sources[k];
                                break;
                            }
                        }
                        if (source == null)
                        {
                            source = soundplayer.AddComponent<AudioSource>();
                        }

                        source.clip = Effectsounds[i].clip;
                        source.Play();

                    }
                }
                else
                {
                    sources = GetComponents<AudioSource>();
                    if(sources != null)
                    {
                        for (int k = 0; k < sources.Length; k++)
                        {
                            if(!sources[k].isPlaying)
                            {
                                sources[k].clip = Effectsounds[i].clip;
                                sources[k].Play();
                                source = sources[k];
                                break;
                            }
                        }
                    }

                    if(source == null)
                    {
                        source = gameObject.AddComponent<AudioSource>();
                        source.clip = Effectsounds[i].clip;
                        source.Play();
                    }
                }

                source.rolloffMode = AudioRolloffMode.Linear;
                source.spatialBlend = 1.0f;

            }            
        }
        

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
                AudioBGM.loop = true;
                return;
            }
        }
    }

    public void StopSE(string name,GameObject soundplayer = null)
    {
        AudioSource[] sources = null;

        if(soundplayer)
        {
            sources = soundplayer.GetComponents<AudioSource>();
        }
        else
        {
            sources = gameObject.GetComponents<AudioSource>();
        }

        if (sources == null) return;

        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].clip.name.CompareTo(name) == 0)
            {
                sources[i].Stop();
                break;
            }
        }

    }

    public void stopAllSE(GameObject soundplayer = null)
    {
        AudioSource[] sources = null;

        if (soundplayer)
        {
            sources = soundplayer.GetComponents<AudioSource>();
        }
        else
        {
            sources = gameObject.GetComponents<AudioSource>();
        }

        if (sources == null) return;

        foreach (AudioSource source in sources)
        {
            source.Stop();
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
        Object[] objs = Resources.LoadAll(Path);
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
