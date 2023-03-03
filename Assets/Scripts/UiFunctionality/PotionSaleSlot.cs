using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotionSaleSlot : MonoBehaviour, IDropHandler
{
    public int id;
    private bool slotFilled = false;
    private PlayerInventory playerInventory;
    [SerializeField] private InventoryUI inventoryUI;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public bool SlotFilled
    {
        get { return slotFilled; }
        set { slotFilled = value; }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop PotionSaleSlot");
        GameObject eventGameObj = eventData.pointerDrag;
        InventorySlot currInvSlot = eventGameObj.GetComponent<ItemUI>().GetInventorySlot();
        if (slotFilled)
        {
            Debug.Log("Slot full");
        }
        else
        {
            //get the inventory item that the icon is associated with
            Item item = inventoryUI.GetInventoryItem(eventGameObj).item;
            playerInventory.RemovePotionItem(item, 1); //subtracts 1 from the qty

            GameObject newIcon = playerInventory.AddPotionSaleItem(item);

            //snap to grid
            if (newIcon != null)
            { 
                newIcon.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;


                newIcon.GetComponent<ItemUI>().SetPotionSaleSlot(this);
                newIcon.GetComponent<ItemUI>().iconAudioSource = inventoryUI.iconAudioSource;
                slotFilled = true;

                newIcon.GetComponent<DragDropItem>().enabled = false;
                newIcon.GetComponent<DragDropSaleItem>().enabled = true;
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
