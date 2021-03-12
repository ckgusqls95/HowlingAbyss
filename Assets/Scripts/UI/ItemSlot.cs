using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    //[HideInInspector]
    public Scriptable_Item Item;
    public int count;
    [SerializeField]
    public Transform ItemImage;
    // Start is called before the first frame update
    void Start()
    {
        Item = null;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(Scriptable_Item item)
    {
        if (Item)
        {
            Item = null;
            count = 0;
            ItemImage.GetComponent<Image>().sprite = null;
        }
        if(item)
        {
            ItemImage.GetComponent<Image>().enabled = true;
            Item = item;
            ItemImage.GetComponent<Image>().sprite = Item.Image;

            if (Item.itemTag == Scriptable_Item.tag_item.Expendables)
            {
                count++;
            }
        }
        else
        {
            ItemImage.GetComponent<Image>().enabled = false;
            Item = null;
            count = 0;
        }
        
    }
    


}
