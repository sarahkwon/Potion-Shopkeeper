using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IComparable, IDropHandler
{
    public int id;
    private bool slotFilled = false; 

    public bool SlotFilled { 
        get {  return slotFilled; }
        set { slotFilled = value; }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject eventGameObj = eventData.pointerDrag;
        ItemUI itemUI = eventGameObj.GetComponent<ItemUI>();
        DragDropItem dragDrop = eventGameObj.GetComponent<DragDropItem>();

        //only invoke this function if the item is not being dragged from a potion sale slot AND it's a potion lool
        if (dragDrop != null && dragDrop.enabled && dragDrop.GetInventoryType() == InventoryUI.InventoryType.Potion)
        {
            if (slotFilled)
            {

                eventGameObj.GetComponent<RectTransform>().anchoredPosition = itemUI.GetInventorySlot().GetComponent<RectTransform>().anchoredPosition;
            }
            else
            {
                //moving an icon from inventoryslot to inventoryslot
                if (itemUI != null)
                {
                    if (itemUI.GetInventorySlot() != null) {
                        itemUI.GetInventorySlot().SlotFilled = false;
                    }
                    
                }

                //we moved an icon from a potionSaleSlot to an inventorySlot, this should never happen
                /*
                if (currPotSaleSlot != null) {
                    currPotSaleSlot.SlotFilled = false;
                    eventGameObj.GetComponent<ItemUI>().SetPotionSaleSlot(null);
                }
                */



                eventGameObj.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventGameObj.GetComponent<ItemUI>().SetInventorySlot(this);
                slotFilled = true;

                eventGameObj.GetComponent<DragDropItem>().enabled = true;
                eventGameObj.GetComponent<DragDropSaleItem>().enabled = false;
            }
        }




    }

    public int CompareTo(object obj)
    {
        var b = obj as InventorySlot;

        if (this.id < b.id)
        {
            return -1;
        }

        if (this.id > b.id)
        {
            return 1;
        }

        return 0;
    }

}
