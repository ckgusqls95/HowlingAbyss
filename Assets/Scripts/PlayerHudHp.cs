using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
public class PlayerHudHp : MonoBehaviour
{
    [SerializeField]
    private GameObject HpMana;
    [SerializeField]
    private GameObject HpEnergy;

    private GameObject championHpCostObj;
    private Champion player;
    private void Awake()
    {
       player = this.gameObject.GetComponent<Champion>();
    }

    void Start()
    {
        if (player.championCostType == Champion.ChampionCostType.MANA)
        {
            championHpCostObj = Instantiate(HpMana, player.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        if (player.championCostType == Champion.ChampionCostType.ENERGY)
        {
            championHpCostObj = Instantiate(HpEnergy, player.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        championHpCostObj.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        championHpCostObj.transform.Find("Player Name").GetComponent<TMP_Text>().text = GetComponent<PhotonView>().Owner.NickName;
    }

    void Update()
    {
        championHpCostObj.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, 5, 0));
        championHpCostObj.transform.Find("HpBar").GetComponent<Image>().fillAmount = player.UnitStatus.health / player.UnitStatus.Maxhealth;
        championHpCostObj.transform.Find("CostBar").GetComponent<Image>().fillAmount = player.UnitStatus.cost / player.UnitStatus.maxCost;
        championHpCostObj.transform.Find("Level").GetComponent<TMP_Text>().text = player.UnitStatus.level.ToString();
    }
}
