using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Transform[] slots;
    // Start is called before the first frame update
    void Start()
    {
        Transform Inven = transform.Find("Slots");

        slots = new Transform[Inven.transform.childCount];
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = Inven.GetChild(i);
        }

    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {

        }
    }

    public int[] InventoryItemSearch()
    {
        int[] SlotItemID = new int[slots.Length];
        int cnt = 0;
        foreach (Transform slot in slots)
        {
            ItemSlot ItemSlot = slot.GetComponent<ItemSlot>();
            if (ItemSlot.Item)
                SlotItemID[cnt] = ItemSlot.Item.Id;
            else
            {
                SlotItemID[cnt] = int.MaxValue;
            }

            cnt++;
        }

        return SlotItemID;
    }

    public void SaleItem(float salePrice) { transform.Find("Gold").GetComponent<Gold>().EarnGold(salePrice); }
    public float ExpendableItemCount(int index)
    {
        return slots[index].GetComponent<ItemSlot>().count;
    }
    public void Equipitem(Scriptable_Item item,float price)
    {
        for(int i = 0; i < item.ItemForCraft; i++)
        {
            int compareID = 0;
            switch (i)
            {
                case 0:
                    compareID = item.IdCraft1;
                    break;
                case 1:
                    compareID = item.IdCraft2;
                    break;
                case 2:
                    compareID = item.IdCraft3;
                    break;
                default:
                    break;
            }

            for (int j = 0; j < slots.Length; j++)
            {
                ItemSlot script = slots[j].GetComponent<ItemSlot>();
                if (script.Item)
                {
                    if(script.Item.Id == compareID)
                    {
                        script.SetItem(null);
                        break;
                    }
                }
            }
            
        }
        
        foreach(Transform child in slots)
        {
            if(child.GetComponent<ItemSlot>().Item == null)
            {
                child.GetComponent<ItemSlot>().SetItem(item);
                break;
            }
        }
        transform.Find("Gold").GetComponent<Gold>().UseGold(price);
    }

    public void Expendableitem(int index, float price)
    {
        slots[index].GetComponent<ItemSlot>().count++;
        transform.Find("Gold").GetComponent<Gold>().UseGold(price);
    }

    public float GetGold() { return transform.Find("Gold").GetComponent<Gold>().getGold(); }
   

}
