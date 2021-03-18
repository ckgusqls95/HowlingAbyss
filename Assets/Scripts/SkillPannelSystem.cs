using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Text;
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
    private Champion player;

    void Awake()
    {
        player = GameManager.Instance.player;
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
        hp.Append((int)player.UnitStatus.health);
        hpBarText.text = hp.ToString();
        hpBarImage.fillAmount = (int)(player.UnitStatus.health / player.UnitStatus.Maxhealth);

        StringBuilder cost = new StringBuilder();
        cost.Append((int)player.UnitStatus.cost);
        cost.Append(" / ");
        cost.Append((int)player.UnitStatus.maxCost);
        costBarText.text = cost.ToString();
        costBarImage.fillAmount = (int)(player.UnitStatus.cost / player.UnitStatus.maxCost);
    }

    public void MatchingChampionSkillPannel(ChampionData _championData)
    {
        for (int index = 0; index < SkillButton.Length; index++)
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
