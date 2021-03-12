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
    // 0.95
    // Start is called before the first frame update

    private void Awake()
    {
        InvokeRepeating("AutomaticAcquisitionGold", 0.5f, 0.5f);
        text = GetComponentInChildren<TMP_Text>();
    }

    void AutomaticAcquisitionGold()
    {
        if (gold >= 99998.9f) return;
        gold += 0.95f;
    }

    private void FixedUpdate()
    {
        text.text = System.Math.Truncate(gold).ToString();
    }
    
}
