using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Summoner Spell Data", menuName = "Scriptable Object/Summoner Data", order = int.MaxValue - 1)]
[System.Serializable]
public class SummonerSpellData : ScriptableObject
{
    enum SummonerSpell { EXHAUST, IGNITE, FLASH, CLEANSE, HEAL, BARRIER, GHOST }

    [SerializeField]
    private Sprite[] summonerSpellIcon;

    public Sprite[] SummonerSpellIcon { get { return summonerSpellIcon; } }

    [SerializeField]
    private GameObject[] summonerSpellText;
    public GameObject[] SummonerSpellText { get { return summonerSpellText; } }
}
