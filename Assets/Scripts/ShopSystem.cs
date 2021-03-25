using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class ShopSystem : MonoBehaviour , IDropHandler
{
    #region member
    private Scriptable_Item[] ItemList;
    public GameObject ItemButton;
    public Transform GridLayout;
    private Transform Info;
    public Transform[] CraftItem;
    private Transform[] CraftItemPrice;
    private int SelectItem;
    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        SelectItem = int.MaxValue;
        ItemList = Resources.LoadAll<Scriptable_Item>("Item");

        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            if (child.name == "Info")
            {
                Info = child;
                Info.gameObject.SetActive(false);
                break;
            }
        }

        Transform CraftArea = Info.transform.GetChild(3);
        CraftItem = new Transform[CraftArea.transform.childCount];
        for (int i = 0; i < CraftArea.transform.childCount; i++)
        {
            CraftItem[i] = CraftArea.GetChild(i);
        }


        CreateItemButton();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateItemButton()
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            int Low = i;

            for (int j = i + 1; j < ItemList.Length; j++)
            {
                if (ItemList[j].Id < ItemList[Low].Id)
                {
                    Low = j;
                }
            }

            if (Low != i)
                ItemSwap(i, Low);
        }

        for (int i = 0; i < ItemList.Length; i++)
        {
            Transform child = Instantiate(ItemButton, GridLayout).transform;
            child.GetChild(0).GetComponent<Image>().sprite = ItemList[i].Image;
            child.GetComponent<SelectItem>().index = i;
        }

    }

    void ItemSwap(int i, int j)
    {
        Scriptable_Item Temp = ItemList[i];
        ItemList.SetValue(ItemList[j], i);
        ItemList.SetValue(Temp, j);
    }

    public void SetInfomation(int index)
    {
        Info.gameObject.SetActive(true);
        TMP_Text ItemName = Info.transform.GetChild(0).GetComponent<TMP_Text>();
        Image ItemImage = Info.transform.GetChild(1).GetComponent<Image>();
        Text Context = Info.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        ItemImage.sprite = ItemList[index].Image;
        ItemName.text = ItemList[index].Name;
        Context.text = ItemList[index].Info;

        // 1 2 3 4
        Image MainItem = CraftItem[0].GetComponent<Image>();
        MainItem.sprite = ItemList[index].Image;
        MainItem.transform.Find("Price").GetComponent<TMP_Text>().text = ItemList[index].Gold.ToString();

        for (int i = 1; i < CraftItem.Length; i++)
        {
            CraftItem[i].gameObject.SetActive(false);
        }

        switch (ItemList[index].ItemForCraft)
        {
            case 1:
                CraftItem[2].gameObject.SetActive(true);
                CraftItem[2].GetComponent<Image>().sprite = ItemList[ItemList[index].IdCraft1].Image;
                CraftItem[2].transform.Find("Price").GetComponent<TMP_Text>().text =
                    ItemList[ItemList[index].IdCraft1].Gold.ToString();
                break;
            case 2: // 2 4
                CraftItem[1].gameObject.SetActive(true);
                CraftItem[1].GetComponent<Image>().sprite = ItemList[ItemList[index].IdCraft1].Image;

                CraftItem[1].transform.Find("Price").GetComponent<TMP_Text>().text =
                ItemList[ItemList[index].IdCraft1].Gold.ToString();


                CraftItem[3].gameObject.SetActive(true);
                CraftItem[3].GetComponent<Image>().sprite = ItemList[ItemList[index].IdCraft2].Image;

                CraftItem[3].transform.Find("Price").GetComponent<TMP_Text>().text =
                 ItemList[ItemList[index].IdCraft2].Gold.ToString();
                break;
            case 3: // 2 3 4
                CraftItem[1].gameObject.SetActive(true);
                CraftItem[1].GetComponent<Image>().sprite = ItemList[ItemList[index].IdCraft1].Image;
                CraftItem[1].transform.Find("Price").GetComponent<TMP_Text>().text =
                ItemList[ItemList[index].IdCraft1].Gold.ToString();


                CraftItem[2].gameObject.SetActive(true);
                CraftItem[2].GetComponent<Image>().sprite = ItemList[ItemList[index].IdCraft2].Image;
                CraftItem[2].transform.Find("Price").GetComponent<TMP_Text>().text =
                ItemList[ItemList[index].IdCraft2].Gold.ToString();


                CraftItem[3].gameObject.SetActive(true);
                CraftItem[3].GetComponent<Image>().sprite = ItemList[ItemList[index].IdCraft3].Image;
                CraftItem[3].transform.Find("Price").GetComponent<TMP_Text>().text =
                ItemList[ItemList[index].IdCraft3].Gold.ToString();
                break;
            default:
                break;
        }



        SelectItem = index;
    }


    public void BuyItem()
    {
        InventorySystem Inventory = GameObject.Find("Inventory").GetComponent<InventorySystem>();
        if(!Inventory)
        {
            Debug.LogError("Inventory Not Found");
            return;
        }
        if(SelectItem == int.MaxValue)
        {
            Debug.LogError("don't Selection Item");
            return;
        }

        int[] Inven_ItemID = Inventory.InventoryItemSearch();
        float requireGold = ItemList[SelectItem].Gold;
        float Gold = Inventory.GetGold();

        bool IsEmptySlot = false;
        for (int i = 0; i < Inven_ItemID.Length; i++)
        {
            if (Inven_ItemID[i] == int.MaxValue)
            {
                IsEmptySlot = true;
                break;
            }
        }
        if (ItemList[SelectItem].itemTag == Scriptable_Item.tag_item.equipment)
        {
            if (ItemList[SelectItem].ItemForCraft == 0)
            {
                if (!IsEmptySlot)
                {
                    //error
                }

                if (requireGold < Gold)
                {
                    Inventory.Equipitem(ItemList[SelectItem], requireGold);
                }
                else
                {
                    // error 
                }
            }
            else
            {
                requireGold = CalculationPrice(Inven_ItemID, SelectItem);
                IsEmptySlot = false;

                for (int i = 0; i < Inven_ItemID.Length; i++)
                {
                    if (Inven_ItemID[i] == int.MaxValue ||
                        Inven_ItemID[i] == int.MinValue)
                    {
                        IsEmptySlot = true;
                        break;
                    }
                }

                if(IsEmptySlot && requireGold < Gold)
                {
                    Inventory.Equipitem(ItemList[SelectItem],requireGold);
                }
                else
                {
                    //error
                }
            }
        }
        else // 
        {
            if (requireGold > Gold)
            {
                //error
                Debug.Log("RequireGold");
                return;
            }

            int ItemSlotNum = int.MaxValue;
            for (int i = 0; i < Inven_ItemID.Length; i++)
            {
                if (ItemList[SelectItem].Id == Inven_ItemID[i])
                {
                    ItemSlotNum = i;
                    break;
                }
            }

            if (ItemSlotNum != int.MaxValue &&
                Inventory.ExpendableItemCount(ItemSlotNum) < ItemList[SelectItem].Overlap)
            {
                Inventory.Expendableitem(ItemSlotNum, requireGold);
            }
            else if (ItemSlotNum == int.MaxValue && IsEmptySlot)
            {
                Inventory.Equipitem(ItemList[SelectItem], requireGold);
            }
            else
            {
                //error max
                Debug.Log("Error");
            }

        }


    }

    private float CalculationPrice(int[] Inventory, int index)
    {
        float price = ItemList[index].Gold;

        for (int i = 0; i < ItemList[index].ItemForCraft; i++)
        {
            int compareID = 0;
            switch (i)
            {
                case 0:
                    compareID = ItemList[index].IdCraft1;
                    break;
                case 1:
                    compareID = ItemList[index].IdCraft2;
                    break;
                case 2:
                    compareID = ItemList[index].IdCraft3;
                    break;
                default:
                    break;
            }

            for (int j = 0; j < Inventory.Length; j++)
            {
                if (Inventory[j] == compareID)
                {
                    price -= ItemList[compareID].Gold;
                    Inventory[j] = int.MinValue;
                    
                }
            }
        }

        return price;
    }


    public void ExitButton()
    {
        this.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject Inventory = GameObject.Find("Inventory");

        if (Inventory)
        {
            InventorySystem InvenSystem = Inventory.GetComponent<InventorySystem>();

            Transform Container = Inventory.transform.Find("DragContainer");
            DragContainer dragContainer = Container.GetComponent<DragContainer>();

            if (dragContainer.DragItem == null) return;

            float price;
            if(dragContainer.DragItem.itemTag == Scriptable_Item.tag_item.Expendables)
            {
                price = dragContainer.DragItem.Gold * dragContainer.count * 0.4f;
            }
            else
            {
                price = dragContainer.DragItem.Gold * 0.7f;
            }

            dragContainer.DragItem = null;
            dragContainer.count = 0;
            dragContainer.image.sprite = null;

            InvenSystem.SaleItem(price);
        }
    }
}
