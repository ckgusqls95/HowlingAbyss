using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    const string HermitPath = "hermit/vo";
    const string VikingPath = "viking/vo";
    private string[] vo;

    string PrevPlaySound;

    private void Awake()
    {
      vo = SoundManager.instance.LoadClip(
            gameObject.name.Contains("Viking") ?  VikingPath : HermitPath);
    }
    
    public void ClickMerchant()
    {
        if(!string.IsNullOrEmpty(PrevPlaySound))
        {
            SoundManager.instance.StopSE(PrevPlaySound);
        }
        
        int index = Random.Range(0, vo.Length);
        SoundManager.instance.PlaySE(vo[index]);
        PrevPlaySound = vo[index];
    }
}
