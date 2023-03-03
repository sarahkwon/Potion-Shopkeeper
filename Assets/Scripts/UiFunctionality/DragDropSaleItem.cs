using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static InventoryUI;

// this is for items that have been instantiated and placed in an item sale slot 
public class DragDropSaleItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private PlayerInventory playerInventory;

    private Canvas canvas;
    private RectTransform rectTransform;

    private CanvasGroup canvasGroup;

    private Vector3 origPos;

    private InventoryType inventoryType;
    private ItemUI itemUI;

    private ItemSaleUI itemSaleUI;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        rectTransform = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemUI = GetComponent<ItemUI>();

        itemSaleUI = ItemSaleUI.FindObjectOfType<ItemSaleUI>();
    }

    private void Start()
    {
        if (GetComponentInParent<InventoryUI>() != null) //this is for the case we instantiate a new icon when dragging into a potion sale slot
        {
            inventoryType = GetComponentInParent<InventoryUI>().inventoryType;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");

        if (eventData != null)
        {
            if (inventoryType == InventoryType.Potion)
            {
                
            }
            else
            { //Ingredient icon

            }

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        itemUI.iconAudioSource.clip = itemUI.beginDragSound;
        itemUI.iconAudioSource.Play();

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData) {
        
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        itemUI.iconAudioSource.clip = itemUI.endDragSound;
        itemUI.iconAudioSource.Play();

        //the eventData.pointerDrag is a dictionary key in ItemSaleUI 
        eventData.pointerDrag.GetComponent<ItemUI>().GetPotionSaleSlot().SlotFilled = false;
        Item item = itemSaleUI.GetItem(eventData.pointerDrag);
        playerInventory.RemovePotionSaleItem(item, eventData.pointerDrag);

        
    }
}
