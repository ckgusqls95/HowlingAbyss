using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SkillPannelSystem : MonoBehaviour
{
    [SerializeField]
    private Button[] SkillButton;
    [SerializeField]
    private Button[] summonerSpell;
    [SerializeField]
    private Image hpBarImage;
    [SerializeField]
    private Text hpBarText;
    [SerializeField]
    private Image costBarImage;
    [SerializeField]
    private Text costBarText;
    [SerializeField]
    private Image championPortrait;

    public Champion playerChampion;

    private float currentHP;
    private float maxHP;
    private float currentCost;
    private float maxCost;

    void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = playerChampion.UnitStatus.health;
        maxHP = playerChampion.UnitStatus.Maxhealth;
        currentCost = playerChampion.UnitStatus.cost;
        maxCost = playerChampion.UnitStatus.maxCost;
        hpBarText.text = currentHP.ToString() + " / " + maxHP.ToString();
        costBarText.text = currentCost.ToString() + " / " + maxCost.ToString();
        hpBarImage.fillAmount = currentHP / maxHP;
        costBarImage.fillAmount = currentCost / maxCost;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP != playerChampion.UnitStatus.health || maxHP != playerChampion.UnitStatus.Maxhealth)
        {
            currentHP = playerChampion.UnitStatus.health;
            maxHP = playerChampion.UnitStatus.Maxhealth;
            hpBarText.text = currentHP.ToString() + " / " + maxHP.ToString();
            hpBarImage.fillAmount = currentHP / maxHP;
        }

        if (currentCost != playerChampion.UnitStatus.cost || maxCost != playerChampion.UnitStatus.maxCost)
        {
            currentCost = playerChampion.UnitStatus.cost;
            maxCost = playerChampion.UnitStatus.maxCost;
            costBarText.text = currentCost.ToString() + " / " + maxCost.ToString();
            costBarImage.fillAmount = currentCost / maxCost;
        }
    }

    public void MatchingChampionSkillPannel(ChampionData _championData)
    {
        for(int index = 0; index < SkillButton.Length; index++)
        {
            SkillButton[index].image.sprite = _championData.ChampionSkill[index];
        }
        for (int index = 0; index < summonerSpell.Length; index++)
        {
            summonerSpell[index].image.sprite = GameManager.Instance.summonerSpell.SummonerSpellIcon[index];
        }
        championPortrait.sprite = _championData.ChampionIconCircle;
    }
}
