using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private string championName;
    public string ChampionName { get { return championName; } set { championName = value; } }
    private int[] spellIndex;
    public int[] SpellIndex { get { return spellIndex; } set { spellIndex = value; } }
    private int masteryIndex;
    public int MasteryIndex { get { return masteryIndex; } set { masteryIndex = value; } }

    public SummonerSpellData summonerSpell;
    public Champion player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spellIndex = new int[2];
        spellIndex[0] = 0;
        spellIndex[1] = 3;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
