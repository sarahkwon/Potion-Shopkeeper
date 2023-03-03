using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSaleUI : MonoBehaviour
{
    //a dictionary of icons that are in the potion sale slots 
    private Dictionary<GameObject, InventoryItem> potionSaleIcons = new Dictionary<GameObject, InventoryItem>();
    private List<PotionSaleSlot> potionSaleSlots = new List<PotionSaleSlot>();

    private DisplayTable currentDisplayTable;
    private Dictionary<int, PotionSaleData> potionSaleInfoList = new Dictionary<int, PotionSaleData>();

    [SerializeField] private GameObject itemIconPrefab;
    [SerializeField] private GameObject qtyTextPrefab;

    [SerializeField] private GameObject sectionParent;

    [SerializeField] private TMP_InputField priceInputText;
    [SerializeField] private Button displayPotionButton;

    [SerializeField] private PlayerInventory playerInventory; //only for instantiating icon prefab

    [SerializeField] private Transform tooltipUI;

    private UIController uiController;
    private bool itemSaleMenuOpen = false;

    public AudioClip placePotionSound;

    public bool ItemSaleMenuOpen {
        get { return itemSaleMenuOpen; }
        set { itemSaleMenuOpen = value; }
    }

    private void Start()
    {
        displayPotionButton.onClick.AddListener(OnDisplayButtonClick);
    }


    public void openDisplayTableUI(DisplayTable displayTable)
    {
        if (uiController == null) {
            uiController = FindObjectOfType<UIController>();
        }
        
        uiController.ToggleItemSaleMenu(displayTable, true);
        uiController.TogglePotionInventory(true);
        itemSaleMenuOpen = true;
    }

    public void closeDisplayTableUI()
    {
        uiController.ToggleItemSaleMenu(null, false);
        uiController.TogglePotionInventory(false);
        itemSaleMenuOpen = false;
    }

    public void DeleteItemUI(GameObject icon) {
        //in order to delete an item's icon

        //remove the item from display, if it was put up
        currentDisplayTable.RemoveFromSale();

        //must set slotFilled of that inventory slot to false 
        PotionSaleSlot saleSlot = icon.GetComponent<ItemUI>().GetPotionSaleSlot();
        saleSlot.SlotFilled = false;

        //remove entry from dictionary
        potionSaleIcons.Remove(icon);

        //set display table icon to null
        currentDisplayTable.ItemIcon = null;

        //destroy the game object
        Destroy(icon);
    }

    //function overload for the case where a customer purchases a potion
    public void DeleteItemUI(DisplayTable table, GameObject icon) {

        table.RemoveFromSale();

        //must set slotFilled of that inventory slot to false 
        PotionSaleSlot saleSlot = icon.GetComponent<ItemUI>().GetPotionSaleSlot();
        saleSlot.SlotFilled = false;

        //remove entry from dictionary
        potionSaleIcons.Remove(icon);

        //set display table icon to null
        table.ItemIcon = null;

        //destroy the game object
        Destroy(icon);

        priceInputText.text = "0"; //set the price input text back to 0

    }

    //it returns the newly instantiated icon
    public GameObject UpdateItemUI(InventoryItem inventoryItem) {

        Item itemData = inventoryItem.item;



        //the actual item's icon
        var obj = Instantiate(itemIconPrefab);
        obj.transform.SetParent(potionSaleInfoList[currentDisplayTable.id].potionSaleSection.transform, false);
        obj.GetComponent<Image>().sprite = itemData.icon;
        obj.GetComponent<TooltipItem>().playerInventory = playerInventory;
        obj.GetComponent<TooltipItem>().tooltipUI = tooltipUI;
        obj.transform.localScale = new Vector3(1,1,1);

        potionSaleIcons.Add(obj, inventoryItem); //add to dictionary

        //we need a record of that icon, so set it in displayTable
        currentDisplayTable.ItemIcon = obj;

        return obj;

    }

    public Item GetItem(GameObject icon) {
        return potionSaleIcons[icon].item;
    }

    public void SetCurrentDisplayTable(DisplayTable table) {
        currentDisplayTable = table;

    }

    public void TogglePotionSaleSection(DisplayTable displayTable, bool value)
    {
        if (value == true)
        {
            SetCurrentDisplayTable(displayTable);

            if (displayTable != null)
            {
                if (!potionSaleInfoList.ContainsKey(currentDisplayTable.id))
                {
                    //instantiate a new section for the display table
                    var newSection = Instantiate(currentDisplayTable.section);
                    newSection.transform.SetParent(sectionParent.transform);
                    newSection.transform.localScale = new Vector3(1f, 1f, 1f);

                    newSection.transform.localPosition = new Vector3(0, 0, 0); //reset the position

                    PotionSaleData newPotionSaleData = new PotionSaleData();
                    newPotionSaleData.potionSaleSection = newSection;

                    potionSaleInfoList.Add(currentDisplayTable.id, newPotionSaleData);
                }
                else //a section was already created for this display table
                {
                    potionSaleInfoList[displayTable.id].potionSaleSection.SetActive(true);

                    //set the potion sale slot to filled for when we display what this displayTable has
                    if (potionSaleInfoList[currentDisplayTable.id].potionSaleSection.GetComponentInChildren<ItemUI>() != null)
                    {
                        potionSaleInfoList[currentDisplayTable.id].potionSaleSection.GetComponentInChildren<ItemUI>().GetPotionSaleSlot().SlotFilled = true;
                    }
                }

                potionSaleInfoList[currentDisplayTable.id].potionSaleSection.SetActive(value);
                priceInputText.text = value ? GetPrice(displayTable).ToString() : "";
            }

        }
        else {
            //update the potion sale slot to not filled since we're hiding the ui
            if (currentDisplayTable != null) {
                if (potionSaleInfoList.ContainsKey(currentDisplayTable.id) && potionSaleInfoList[currentDisplayTable.id].potionSaleSection.GetComponentInChildren<ItemUI>() != null)
                {
                    potionSaleInfoList[currentDisplayTable.id].potionSaleSection.GetComponentInChildren<ItemUI>().GetPotionSaleSlot().SlotFilled = false;
                }


                //hide the potion sale section
                potionSaleInfoList[currentDisplayTable.id].potionSaleSection.SetActive(value);
            }
                

                

            //set the displaytable to null
            SetCurrentDisplayTable(null);



        }




    }

    //given the game object, return the item data that it is linked to 
    public InventoryItem GetSaleItem(GameObject itemIcon)
    {
        if (potionSaleIcons.ContainsKey(itemIcon))
        {
            return potionSaleIcons[itemIcon];
        }

        return null;

    }

    private void OnDisplayButtonClick() {
        
        

        int currPrice = 0;
        if (priceInputText.text == "")
        {
            Debug.Log("Please set a price for the potion!");
        }
        else {
            int a;
            currPrice = int.TryParse(priceInputText.text, out a) ? int.Parse(priceInputText.text) : 0;
            SetPrice(currentDisplayTable, currPrice);

            GameObject obj = potionSaleInfoList[currentDisplayTable.id].GetPotionIcon();
            if (obj != null)
            {
                currentDisplayTable.PutUpForSale(potionSaleIcons[obj].item);
                uiController.tableAudioSource.clip = placePotionSound;
                uiController.tableAudioSource.Play();
            }
            else {
                Debug.Log("Please drag in the potion you want to sell!");
            }


            //toggle the itemsale menu and inventory to inactive
            closeDisplayTableUI();
        }

        
    }

    public void SetPrice(DisplayTable displayTable, int value)
    {
        potionSaleInfoList[displayTable.id].price = value;
    }

    public int GetPrice(DisplayTable displayTable) {
        if (displayTable != null && potionSaleInfoList.ContainsKey(displayTable.id)) {
            return potionSaleInfoList[displayTable.id].price;
        }

        return 0;
        
    }




}

public class PotionSaleData {
    public GameObject potionSaleSection;
    public int price;

    public List<GameObject> GetPotionIcons() {
        List<GameObject> result = new List<GameObject>();
        foreach (var i in potionSaleSection.GetComponentsInChildren<ItemUI>()) {
            result.Add(i.gameObject);
        }

        return result;
    }

    public GameObject GetPotionIcon() {
        ItemUI obj = potionSaleSection.GetComponentInChildren<ItemUI>();
        if (obj != null)
        {
            return obj.gameObject;
        }
        else {
            return null;
        }
       
    }
}
