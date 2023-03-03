using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static InventoryUI;

public class DragDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private PotionCraftingUI potionCraftingUI;

    private CanvasGroup canvasGroup;

    private Vector3 origPos;

    private InventoryType inventoryType;
    private ItemUI itemUI;

    //these are for ingredients only
    private Transform iconDragParent;
    private Transform replacementParent;
    private GameObject replacement;
    InventoryUI inventoryUI;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemUI = GetComponent<ItemUI>();
    }

    private void Start()
    {
        if (GetComponentInParent<InventoryUI>() != null) //this is for the case we instantiate a new icon when dragging into a potion sale slot
        {
            inventoryUI = GetComponentInParent<InventoryUI>();
            replacementParent = inventoryUI.transform;
            inventoryType = inventoryUI.inventoryType;

        }

        if (inventoryType == InventoryType.Ingredient) {
            iconDragParent = GameObject.Find("ImproviseMenu").transform;
            potionCraftingUI = GetComponentInParent<PotionCraftingUI>();
        }
        


    }

    private void Update()
    {

    }

    //differences between ingredient and potion
    // for ingredients, onpointerdown, instantiate a new ingredient icon 
    // on pointer up, destroy that ingredient icon IF it is not placed on an ingredient slot

    // for potions, it will not instantiate a new icon, we can drag them around 
    // if dragged on top of another inventory slot, move it to that slot 
    // if dragged on top of the item sale box, instantiate a new icon and inventoryItem with 1 amt 
    //place it on the  box
    //and 

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("OnPointerDown");

        if (eventData != null) {
            if (inventoryType == InventoryType.Potion)
            {
                origPos = rectTransform.anchoredPosition;
            }
            else { //Ingredient icon

                
                

                


                
            }
            
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (inventoryType == InventoryType.Ingredient) {
            //the ingredient is in the inventory right now, clicking on it will instantiate a new instance 
            if (itemUI.GetCraftSlot() == null)
            {
                //instantiate a new ingredient icon that is a copy of pointerdrag, 
                replacement = Instantiate(rectTransform.gameObject, replacementParent);
                replacement.GetComponent<DragDropItem>().enabled = false;

                //rehash
                //this is definitely bad practice haha
                InventoryItem currItem = inventoryUI.GetInventoryItem(gameObject);
                inventoryUI.AddInventoryIcon(replacement, currItem);

                //move the pointerdrag object to be a child of potion crafting menu, so that it doesn't get covered when dragging it around
                rectTransform.gameObject.transform.SetParent(iconDragParent);
            }
            else
            { //it was already placed in an ingredient craft slot, so no need to instantiate more instances 
              //set the craft slot slotFilled to false AND then set it to null
                itemUI.GetCraftSlot().SlotFilled = false;
                itemUI.SetCraftSlot(null);
            }
        }

        itemUI.iconAudioSource.clip = itemUI.beginDragSound;
        itemUI.iconAudioSource.Play();

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }


    public void OnPointerUp(PointerEventData eventData)
    {

        if (inventoryType == InventoryType.Potion)
        {
            //snap potion back to original position
            rectTransform.anchoredPosition = origPos;
        }
        else {
            //replacement needs to function as a normal icon now
            if (replacement != null) {
                replacement.GetComponent<DragDropItem>().enabled = true;
            }
            

            
            
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        itemUI.iconAudioSource.clip = itemUI.endDragSound;
        itemUI.iconAudioSource.Play();

        if (inventoryType == InventoryType.Ingredient)
        {
            if (itemUI.GetCraftSlot() == null) {
                Destroy(eventData.pointerDrag.gameObject);
            }
                

        }


    }

    public InventoryType GetInventoryType() {
        return inventoryType;
    }


}
