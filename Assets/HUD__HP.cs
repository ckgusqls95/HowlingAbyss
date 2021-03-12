using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD__HP : MonoBehaviour
{
    Image hpImage;
    private void Awake()
    {
        hpImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HpApply(float CurrentHP,float MaxHp)
    {
        hpImage.fillAmount = CurrentHP / MaxHp;
    }
}
