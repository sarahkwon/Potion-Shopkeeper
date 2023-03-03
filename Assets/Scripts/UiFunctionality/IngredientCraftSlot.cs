using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientCraftSlot : MonoBehaviour, IDropHandler
{
    public int id;
    private bool slotFilled = false;
    private PlayerInventory playerInventory;
    private PotionCraftingUI potionCraftingUI;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        potionCraftingUI = FindObjectOfType<PotionCraftingUI>();
    }

    public bool SlotFilled
    {
        get { return slotFilled; }
        set { slotFilled = value; }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject eventGameObj = eventData.pointerDrag;
        Debug.Log("OnDrop IngredientSlot");
        eventData.pointerDrag.gameObject.GetComponent<ItemUI>().SetCraftSlot(this);
        potionCraftingUI.UpdateTotalPriceSection();

        eventGameObj.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
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
