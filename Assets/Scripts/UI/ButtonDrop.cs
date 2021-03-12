using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonDrop : MonoBehaviour, 
    IDropHandler,
    IBeginDragHandler,
    IEndDragHandler, 
    IDragHandler
{
   
    public static Vector2 defaultpos;

    public DragContainer Container;

    bool isDragging;
    // Start is called before the first frame update
    void Start()
    {
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemSlot slot = transform.GetComponent<ItemSlot>();

        Container.gameObject.SetActive(true);
        if(slot.Item != null)
        {
            Container.DragItem = null;
            Container.DragItem = slot.Item;
            Container.image.sprite = Container.DragItem.Image;

            if (slot.Item.itemTag == Scriptable_Item.tag_item.Expendables)
            {
                Container.count = slot.count;
            }
            else
            {
                Container.count = 0;
            }
        }
        
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            Container.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            ItemSlot slot = transform.gameObject.GetComponent<ItemSlot>();
            if (Container.DragItem != null)
            {
                slot.SetItem(Container.DragItem);
                slot.count = Container.count;
                slot.ItemImage.GetComponent<Image>().enabled = true;
            }
            else
            {
                slot.Item = null;
                slot.count = 0;
                slot.ItemImage.GetComponent<Image>().sprite = null;
                slot.ItemImage.GetComponent<Image>().enabled = false;
            }
        }

        isDragging = false;
        Container.DragItem = null;
        if(Container.image)
            Container.image.sprite = null;
        Container.count = 0;
        Container.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (Container.DragItem != null)
        {
            ItemSlot slot = transform.gameObject.GetComponent<ItemSlot>();
            Scriptable_Item item = slot.Item;
            int count = slot.count;

            slot.Item = null;
            slot.Item = Container.DragItem;
            slot.count = Container.count;
            transform.Find("Image").GetComponent<Image>().sprite = Container.image.sprite;
            transform.Find("Image").GetComponent<Image>().enabled = true;

            if(item)
            {
                Container.image.sprite = item.Image;
                Container.count = count;
                Container.DragItem = null;
                Container.DragItem = item;
            }
            else
            {
                Container.image.sprite = null;
                Container.count = 0;
                Container.DragItem = null;
            }
        }
        else
        {
            Container.DragItem = null;
        }
    }

}
