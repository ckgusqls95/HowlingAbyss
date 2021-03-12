using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
public class SelectItem : MonoBehaviour
{
    [HideInInspector]
    public int index;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SetupInfo);
    }

    void SetupInfo()
    {
        GetComponentInParent<ShopSystem>().SetInfomation(index);
        
    }
}
