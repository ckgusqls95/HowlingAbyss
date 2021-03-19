using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Text;
public class SkillPannelSystem : MonoBehaviour
{
    [SerializeField]
    private Button[] skillButton;
    private SkillButton[] skillButtonScript;
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
    [SerializeField]
    private Image experienceBar;
    [SerializeField]
    private Text level;

    private Champion player;

    void Awake()
    {
        player = GameManager.Instance.player;
        skillButtonScript = new SkillButton[5];
        for (int index = 0; index < skillButton.Length; ++index)
        {
            skillButtonScript[index] = skillButton[index].gameObject.GetComponent<SkillButton>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder hp = new StringBuilder();
        hp.Append((int)player.UnitStatus.health);
        hp.Append(" / ");
        hp.Append((int)player.UnitStatus.Maxhealth);
        hpBarText.text = hp.ToString();
        hpBarImage.fillAmount = player.UnitStatus.health / player.UnitStatus.Maxhealth;

        StringBuilder cost = new StringBuilder();
        cost.Append((int)player.UnitStatus.cost);
        cost.Append(" / ");
        cost.Append((int)player.UnitStatus.maxCost);
        costBarText.text = cost.ToString();
        costBarImage.fillAmount = player.UnitStatus.cost / player.UnitStatus.maxCost;

        float requiredExperience = 180 + (player.UnitStatus.level * 100);
        experienceBar.fillAmount = player.UnitStatus.experience / requiredExperience;
        level.text = player.UnitStatus.level.ToString();

        for(int index = 1; index < 5; index++)
        {
            skillButtonScript[index].skillFilter.fillAmount = player.ChampionSkill[index - 1].currentCoolTime / player.ChampionSkill[index - 1].coolTime;
            if(System.Math.Truncate(player.ChampionSkill[index - 1].currentCoolTime) > 0)
            {
                skillButtonScript[index].coolTimeCounter.text = System.Math.Truncate(player.ChampionSkill[index - 1].currentCoolTime).ToString();
            }
            else
            {
                skillButtonScript[index].coolTimeCounter.text = "";
            }
        }
    }

    public void MatchingChampionSkillPannel(ChampionData _championData)
    {
        for (int index = 0; index < skillButton.Length; index++)
        {
            skillButton[index].image.sprite = _championData.ChampionSkill[index];
        }
        
        summonerSpell[0].image.sprite = GameManager.Instance.summonerSpell.SummonerSpellIcon[GameManager.Instance.SpellIndex[0]];
        summonerSpell[1].image.sprite = GameManager.Instance.summonerSpell.SummonerSpellIcon[GameManager.Instance.SpellIndex[1]];
        championPortrait.sprite = _championData.ChampionIconCircle;
    }
}
