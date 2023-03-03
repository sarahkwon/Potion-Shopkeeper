using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public enum InventoryType
    {
        Ingredient,
        Potion,
        Recipe
    }

    private Dictionary<GameObject, InventoryItem> inventoryIcons = new Dictionary<GameObject, InventoryItem>();
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public InventoryType inventoryType;

    [SerializeField] private GameObject itemIconPrefab;
    [SerializeField] private GameObject qtyTextPrefab;

    [SerializeField] private PlayerInventory playerInventory; //this is only for instantianting a prefab

    [SerializeField] private Transform tooltipUI;


    [SerializeField] private PotionCraftingUI potionCraftingUI; //this is only for recipe inventories

    public AudioSource iconAudioSource;

    //this is for ingredient prices
    public int whitePrice = 10;
    public int redPrice = 25;
    public int bluePrice = 25;
    public int yellowPrice = 30;
    public int greenPrice = 30;

    public int orangePrice = 50;

    public int purplePrice = 100;


    private void Awake()
    {
        
    }


    private void Start()
    {
        // get all child InventorySlots
        /*
        foreach (InventorySlot iSlot in GetComponentsInChildren<InventorySlot>())
        {
            inventorySlots.Add(iSlot);
        }
        inventorySlots.Sort();
        */



    }

    public void UpdateItemUI(InventoryItem inventoryItem) {
        if (inventoryItem == null) { //player has no more of that item, must delete icon from world

            while (true) {
                GameObject iconToRemove = GetFirstIconWithAmtZero();

                if (iconToRemove != null)
                {
                    DeleteItemUI(iconToRemove);
                }
                else {
                    break;
                }
                
            }

            return;
            
        }

        //first, get the gameobject that the item is attached to
        GameObject icon = GetIcon(inventoryItem);

        if (icon == null) //the item is not in the inventoryUI, must add it 
        {
            AddItemUI(inventoryItem);

            
        }
        else { //just need to update the amt UI text, if it's a potion
            if (inventoryType == InventoryType.Potion) {
                icon.GetComponentInChildren<TextMeshProUGUI>().text = inventoryItem.amount.ToString("n0");
            }
        }
    }

    private void AddItemUI(InventoryItem inventoryItem) {
        Item itemData = inventoryItem.item;

        if (inventoryType == InventoryType.Recipe) {
            Debug.Log("Is this ruybnning?>");
        }
        

        //the actual item's icon
        var obj = Instantiate(itemIconPrefab);
        obj.GetComponent<ItemUI>().iconAudioSource = iconAudioSource;
        obj.transform.SetParent(transform);
        obj.GetComponent<Image>().sprite = itemData.icon;
        obj.GetComponent<TooltipItem>().tooltipUI = tooltipUI; //set the tool tip reference to the tool tip ui

        obj.GetComponent<TooltipItem>().playerInventory = playerInventory;
        obj.transform.localScale = new Vector3(1, 1, 1);

        //the quantity text
        if (inventoryType == InventoryType.Potion)
        {
            var newQtyText = Instantiate(qtyTextPrefab);
            newQtyText.transform.SetParent(obj.transform);
            newQtyText.transform.localPosition = new Vector3(-2f, -23.3f, newQtyText.transform.localPosition.z);
            newQtyText.GetComponent<TextMeshProUGUI>().text = inventoryItem.amount.ToString("n0");
            newQtyText.transform.localScale = new Vector3(1, 1, 1);
        }

        
        //the price text for ingredients
        if (inventoryType == InventoryType.Ingredient) {
            var newPriceText = Instantiate(qtyTextPrefab);
            newPriceText.transform.SetParent(obj.transform);
            newPriceText.transform.localPosition = new Vector3(13.1f, -23.3f, newPriceText.transform.localPosition.z);

            int price = getIngredientPrice(itemData);
            string formattedText = price.ToString();
            newPriceText.GetComponent<TextMeshProUGUI>().text = "$" + formattedText;
            newPriceText.transform.localScale = new Vector3(1, 1, 1);
        }


        inventoryIcons.Add(obj, inventoryItem); //add to dictionary

        //recipe's onclick functionality
        if (inventoryType == InventoryType.Recipe)
        {
            obj.GetComponent<Button>().onClick.AddListener(delegate { potionCraftingUI.SendRecipeData(inventoryItem.item); });
        }

        PlaceItemInFirstSlot(obj, inventoryItem);
    }

    public int getIngredientPrice(IngredientType type)
    {

        if (type== IngredientType.White)
        {
            return whitePrice;
        }

        if (type == IngredientType.Red)
        {
            return redPrice;
        }

        if (type == IngredientType.Blue)
        {
            return bluePrice;
        }

        if (type == IngredientType.Yellow)
        {
            return yellowPrice;
        }

        if (type == IngredientType.Green)
        {
            return greenPrice;
        }

        if (type == IngredientType.Purple)
        {
            return purplePrice;
        }

        return 0;
    }

    public int getIngredientPrice(Item item) {
        IngredientItem ingredItem = (IngredientItem)item;

        if (ingredItem.ingredientType == IngredientType.White) {
            return whitePrice;
        }

        if (ingredItem.ingredientType == IngredientType.Red)
        {
            return redPrice;
        }

        if (ingredItem.ingredientType == IngredientType.Blue)
        {
            return bluePrice;
        }

        if (ingredItem.ingredientType == IngredientType.Yellow)
        {
            return yellowPrice;
        }

        if (ingredItem.ingredientType == IngredientType.Green)
        {
            return greenPrice;
        }

        if (ingredItem.ingredientType == IngredientType.Purple)
        {
            return purplePrice;
        }

        return 0;
    }

    private GameObject GetFirstIconWithAmtZero()
    {
        foreach (var i in inventoryIcons)
        {
            if (i.Value.amount <= 0)
            {
                return i.Key;
            }
        }

        return null;
    }

    private void PlaceItemInFirstSlot(GameObject itemIcon, InventoryItem inventoryItem) {
        foreach (InventorySlot iSlot in inventorySlots) { 
            if (iSlot.SlotFilled == false) {
                itemIcon.GetComponent<ItemUI>().SetInventorySlot(iSlot);
                iSlot.SlotFilled = true;

                //snap the icon to the available slot 
                itemIcon.GetComponent<RectTransform>().anchoredPosition = iSlot.GetComponent<RectTransform>().anchoredPosition;

                break;
            } 
        }
    }

    public void DeleteItemUI(GameObject itemIcon) {
        //in order to delete an item's icon

        //must set slotFilled of that inventory slot to false 
        InventorySlot invSlot = itemIcon.GetComponent<ItemUI>().GetInventorySlot();
        invSlot.SlotFilled = false;

        //remove entry from dictionary
        inventoryIcons.Remove(itemIcon);

        //destroy the game object
        Destroy(itemIcon);


    }

    //given the game object, return the item data that it is linked to 
    public InventoryItem GetInventoryItem(GameObject itemIcon) {

        if (inventoryIcons.ContainsKey(itemIcon)) {
            return inventoryIcons[itemIcon];
        }

        return null;
               
    }


    //given the item, return the game object that it is linked to
    public GameObject GetIcon(InventoryItem inventoryItem) {
        foreach (var i in inventoryIcons) {
            if (i.Value == inventoryItem) {
                return i.Key;
            }
        }

        return null;
    }


    // add a new entry to the  dictionary 
    public void AddInventoryIcon(GameObject itemIcon, InventoryItem inventoryItem)
    {
        inventoryIcons.Add(itemIcon, inventoryItem);
    }

    public void RemoveInventoryIcon(GameObject itemIcon)
    {
        inventoryIcons.Remove(itemIcon);
    }




}
