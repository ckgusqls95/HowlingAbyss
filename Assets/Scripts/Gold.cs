using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Gold : MonoBehaviour
{
    [Range(0,99999)]
    public float gold = 0.0f;

    private TMP_Text text;
    [HideInInspector]
    public PlayerController player;
    // 0.95
    // Start is called before the first frame update

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void FixedUpdate()
    {
        gold = player.gold;
        text.text = System.Math.Truncate(gold).ToString();
    }
    
}
