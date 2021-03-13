using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

[CreateAssetMenu(fileName = "Champion Data", menuName = "Scriptable Object/Champion Data", order = int.MaxValue)]
[System.Serializable]
public class ChampionData : ScriptableObject
{
    [SerializeField]
    private string championName;
    [SerializeField]
    private string championTag;
    [SerializeField]
    private int championNumber;
    [SerializeField]
    private Sprite championIconBox;
    [SerializeField]
    private Sprite championIconCircle;
    [SerializeField]
    private Sprite championPortrait;
    [SerializeField]
    private Sprite[] championSkillSprite;

    public string ChampionName { get { return championName; } }
    public string ChampionTag { get { return championTag; } }
    public int ChampionNumber { get { return championNumber; } }
    public Sprite ChampionIconBox { get { return championIconBox; } }
    public Sprite ChampionIconCircle { get { return championIconCircle; } }
    public Sprite ChampionPortrait { get { return championPortrait; } }
    public Sprite[] ChampionSkill { get { return championSkillSprite; } }
}
