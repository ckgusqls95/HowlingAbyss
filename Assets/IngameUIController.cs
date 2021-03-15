using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIController : MonoBehaviour
{
   
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            PopupShop();
        }
    }

    public void PopupShop()
    {
        GameObject shop = this.transform.Find("Shop").gameObject;

        if(shop)
        {
            bool button = shop.activeInHierarchy == true ? false : true;
            shop.SetActive(button);
        }
    }
}
