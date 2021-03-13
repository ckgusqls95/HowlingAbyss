using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ChampionPick : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum ChampionName { Ahri, LeeSin, Lucian  }
    public enum SummonerSpell { EXHAUST, IGNITE, FLASH, CLEANSE, HEAL, BARRIER }

    [SerializeField]
    private ChampionData[] championData;

    [SerializeField]  
    private SummonerSpellData summonerSpell;

    [SerializeField]
    private Image myChampionCircleImage;

    private Button[] ChampionChoiceButton;

    [SerializeField]
    private Button[] ChampionUseSpellButton;

    private Button[] ChampionSpellButton;

    private Button LastSellectedButton;

    [SerializeField]
    private GameObject SummonerSpellView;

    [SerializeField]
    private TextMeshProUGUI pickTime;

    [SerializeField]
    private TextMeshProUGUI searchChanmpionText;

    private GameObject spellText;

    private Image LastSpellChoiceImage;

    private Button ExitPickRoomButton;

    private float LimitTime = 80.0f;
    private int allReadyCount = 0;

    private void Awake()
    {
        ChampionChoiceButton = transform.Find("ChampionPick").GetComponentsInChildren<Button>();
        ChampionSpellButton = transform.Find("SummonerSpell").Find("SummonerSpell View").GetComponentsInChildren<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MatchingChampionButton();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("StartLimitTime");
    }

    public void MatchingChampionCircleImage(ChampionData championCircleData)
    {
        myChampionCircleImage.sprite = championCircleData.ChampionIconCircle;
        myChampionCircleImage.color = new Color(myChampionCircleImage.color.r, myChampionCircleImage.color.g, myChampionCircleImage.color.b, 255);
    }

    public void ChangeSummonerSpell(int index)
    {
        LastSellectedButton.image.sprite = summonerSpell.SummonerSpellIcon[index];
        transform.Find("SummonerSpell").Find("Panel").gameObject.SetActive(false);
        SummonerSpellView.SetActive(false);
        Destroy(spellText);
        LastSellectedButton.transform.parent.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        LastSpellChoiceImage.color = new Color(255, 255, 255, 0);

        if (LastSellectedButton.name == "Spell Left Button")
        {
            GameManager.Instance.SpellIndex[0] = index;
        }
        else
        {
            GameManager.Instance.SpellIndex[1] = index;
        }
    }

    public void OnSummonerSpellButton(Button button)
    {
        SummonerSpellView.SetActive(true);
        transform.Find("SummonerSpell").Find("Panel").gameObject.SetActive(true);
        if (LastSellectedButton)
        {
            LastSellectedButton.transform.parent.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        LastSellectedButton = button;
        LastSellectedButton.transform.parent.GetComponentInParent<Image>().color = new Color(255, 255, 255, 255);
    }

    public void SpellPointerEnter(int index)
    {
        spellText = (GameObject)Instantiate(summonerSpell.SummonerSpellText[index], transform.Find("SummonerSpell"));
        spellText.transform.position = new Vector3(spellText.transform.position.x, spellText.transform.position.y - 50.0f, spellText.transform.position.z);
        spellText.GetComponent<Text>().raycastTarget = false;
        LastSpellChoiceImage = ChampionSpellButton[index].transform.parent.GetComponentInParent<Image>();
        LastSpellChoiceImage.color = new Color(255, 255, 255, 255);
    }

    public void SpellPointerExit()
    {
        Destroy(spellText);
        LastSpellChoiceImage.color = new Color(255, 255, 255, 0);
    }

    private void MatchingChampionButton()
    {
        for (int index = 0; index < ChampionChoiceButton.Length; ++index)
        {
            ChampionChoiceButton[index].image.sprite = championData[index].ChampionIconBox;
        }
    }

    public void SearchChampion(TMP_InputField searchText)
    {
        for (int index = 0; index < championData.Length; ++index)
        {
            string searchTextUpper = searchText.text.ToUpper();

            if (championData[index].ChampionName.Contains(searchTextUpper))
            {
                ChampionChoiceButton[index].gameObject.SetActive(true);
            }
            else
            {
                ChampionChoiceButton[index].gameObject.SetActive(false);
            }
        }
    }

    public void SelectChampion(string _championName)
    {
        GameManager.Instance.ChampionName = _championName;
    }

    //public void SelectSummonerSpell(int index)
    //{
    //    GameManager.Instance.player.SpellIndex = index;
    //}

    public void OnLeaveButton()
    {
        FindObjectOfType<Canvas>().GetComponent<LobbySceneUI>().LeaveRoom();
    }

    public void ReadyAllPlayer()
    {
        allReadyCount++;
        if (allReadyCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            NetworkManager.Instance.StartGame();
        }
    }

    public void LimitTimeExcess()
    {
        if( LimitTime < 0.1f)
        {
            if (GameManager.Instance.ChampionName == null)
            {
                FindObjectOfType<Canvas>().GetComponent<LobbySceneUI>().LeaveRoom();
            }
            else
            {
                NetworkManager.Instance.StartGame();
            }
        }
    }

    IEnumerator StartLimitTime()
    {
        LimitTime -= Time.deltaTime;
        pickTime.text = System.Math.Truncate(LimitTime).ToString();

        if (LimitTime < 0.1f)
        {
            LimitTimeExcess();
            yield break;
        }
        yield return null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(allReadyCount);
        }
        else
        {
            allReadyCount = (int)stream.ReceiveNext();
        }
    }
}
