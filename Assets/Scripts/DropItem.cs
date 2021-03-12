using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DropItem : MonoBehaviour , IDropHandler
{
    public DragContainer Container;


    public void OnDrop(PointerEventData eventData)
    {
        if (Container.DragItem != null)
        {
            ItemSlot slot = transform.gameObject.GetComponent<ItemSlot>();

            Scriptable_Item item = slot.Item;
            Container.image.sprite = item.Image;
            Container.count = slot.count;

            slot.Item = Container.DragItem;
            slot.count = Container.count;
            slot.ItemImage.GetComponent<Image>().sprite = Container.image.sprite;
            slot.ItemImage.GetComponent<Image>().enabled = true;

            Container.DragItem = item;
        }
        else
        {
            Container.DragItem = null;
        }
    }
}
