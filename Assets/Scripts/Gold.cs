using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Gold : MonoBehaviour
{
    const float MaxGold = 99999;
    private float gold = 0.0f; 

    private TMP_Text text;
    [HideInInspector]
    public PlayerController player;
    // 0.95
    // Start is called before the first frame update

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        StartCoroutine(AutoMining());
    }

    private void FixedUpdate()
    {
        text.text = System.Math.Truncate(gold).ToString();
    }
    
    IEnumerator AutoMining()
    {
        while(true)
        {
            EarnGold(0.95f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void EarnGold(float money)
    {
        if(gold <= MaxGold)
        {
            gold += money;
        }
    }

    public void UseGold(float money)
    {
        gold -= money;
        if(gold < 0.0f)
        {
            gold = 0.0f;
        }
    }
    public float getGold() { return gold; }
}
