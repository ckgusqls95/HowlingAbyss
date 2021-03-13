using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Announcer : MonoBehaviour
{
    public enum RequestMenu
    {
        AbyssEntrance,
        MinionSummon,
        BlueTeamVictory,
        RedTeamVictory,
        enemyKill,
        Excute,
        Death
    }
    
    private Dictionary<RequestMenu, string> soundMenu = new Dictionary<RequestMenu, string>();
    const string Path = "Announcer/SE/";

    private void Awake()
    {
        string[] AnnouncerSE;
        AnnouncerSE = SoundManager.instance.LoadClip(Path);


        for (int i = 0; i < AnnouncerSE.Length; i++)
        {
            if (AnnouncerSE[i].Contains("entrance"))
            {
                soundMenu.Add(RequestMenu.AbyssEntrance, AnnouncerSE[i]);
                continue;
            }
            else if(AnnouncerSE[i].Contains("create"))
            {
                soundMenu.Add(RequestMenu.MinionSummon, AnnouncerSE[i]);
                continue;
            }
            else if(AnnouncerSE[i].CompareTo("blue team victory") == 0)
            {
                soundMenu.Add(RequestMenu.BlueTeamVictory, AnnouncerSE[i]);
                continue;
            }
            else if (AnnouncerSE[i].CompareTo("Red team victory") == 0)
            {
                soundMenu.Add(RequestMenu.RedTeamVictory, AnnouncerSE[i]);
                continue;
            }
            else if(AnnouncerSE[i].CompareTo("enemy kill") == 0)
            {
                soundMenu.Add(RequestMenu.enemyKill, AnnouncerSE[i]);
            }
        }

    }

    private void Start()
    {
        Request(RequestMenu.AbyssEntrance);
    }

    public void Request(RequestMenu Request)
    {
        SoundManager.instance.PlaySE(soundMenu[Request]);
    }
    
}
