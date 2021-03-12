using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChampionPick : MonoBehaviour
{
    public enum SummonerSpell { EXHAUST, IGNITE, FLASH, CLEANSE, HEAL, BARRIER }
    public enum Champion { Ahri, LeeSin, Lucian }

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

    private float LimitTime = 80.0f;

    private Button ExitPickRoomButton;

    private void Awake()
    {
        ChampionChoiceButton = transform.Find("ChampionPick").GetComponentsInChildren<Button>();
        ChampionSpellButton = transform.Find("SummonerSpell").Find("SummonerSpell View").GetComponentsInChildren<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MatchingChampionButton();
        ButtonListener();
    }

    // Update is called once per frame
    void Update()
    {
        LimitTime -= Time.deltaTime;
        pickTime.text = System.Math.Truncate(LimitTime).ToString();
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
            GameManager.Instance.player.SpellIndex[0] = index;
        }
        else
        {
            GameManager.Instance.player.SpellIndex[1] = index;
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

    private void ButtonListener()
    {
        ChampionChoiceButton[0].onClick.AddListener(delegate { MatchingChampionCircleImage(championData[(int)Champion.Ahri]); });
        ChampionChoiceButton[1].onClick.AddListener(delegate { MatchingChampionCircleImage(championData[(int)Champion.LeeSin]); });
        ChampionChoiceButton[2].onClick.AddListener(delegate { MatchingChampionCircleImage(championData[(int)Champion.Lucian]); });
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

    public void OnClickExitPickRoom()
    {
        FindObjectOfType<Canvas>().GetComponent<LobbySceneUI>().ExitPickRoom();
    }

    public void SelectChampion(string _championName)
    {
        GameManager.Instance.player.ChampionName = _championName;
    }
    //public void SelectSummonerSpell(int index)
    //{
    //    GameManager.Instance.player.SpellIndex = index;
    //}
}
