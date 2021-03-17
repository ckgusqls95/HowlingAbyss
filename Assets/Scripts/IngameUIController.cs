using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIController : MonoBehaviour
{
    public GameObject GameOverUI;
    private void Start()
    {
        Screen.SetResolution(1920, 1080, false);
    }
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

    public void PopupVictory()
    {
        GameOverUI.SetActive(true);
        GameOverUI.transform.Find("Victory").gameObject.SetActive(true);
        GameObject.Find("Announcer").GetComponent<Announcer>().Request(Announcer.RequestMenu.Victory);
    }

    public void PopupDefeat()
    {
        GameOverUI.SetActive(true);
        GameOverUI.transform.Find("Defeat").gameObject.SetActive(true);
        GameObject.Find("Announcer").GetComponent<Announcer>().Request(Announcer.RequestMenu.Defeat);
    }
}
