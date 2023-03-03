using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTable : MonoBehaviour, IInteractable
{
    public int id; 

    [SerializeField] private string _prompt;
    
    
    [SerializeField] ItemSaleUI itemSaleUI;
    [SerializeField] private float potionPosition = 0.9f;

    public GameObject section; //it wil be the parent for the instantiated icons in potion sale slots

    

    private bool isItemForSale = false;
    public bool IsItemForSale
    {
        get { return isItemForSale; }
        set { isItemForSale = value; }
    }

    private PotionItem itemForSale;
    private GameObject itemForSaleDisplay;

    private bool _interactable = true;
    public bool isInteractable
    {
        get { return _interactable; }
        set { _interactable = value; }
    }

    private bool isLocked = false;
    public bool IsLocked
    {
        get { return isLocked; }
        set { isLocked = value; }
    }

    private GameObject itemIcon;
    public GameObject ItemIcon
    {
        get { return itemIcon; }
        set { itemIcon = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public string InteractionPrompt => _prompt;
    public bool Interact(Interactor interactor) {
        if (!itemSaleUI.ItemSaleMenuOpen)
        {
            itemSaleUI.openDisplayTableUI(this);
        }
        else {
            itemSaleUI.closeDisplayTableUI();
        }
        
        return true;
    }

    public void PutUpForSale(Item item) {
        PotionItem potionItem = item as PotionItem;
        itemForSale = potionItem;

        if (itemForSaleDisplay != null) {
            Destroy(itemForSaleDisplay);
        }
        itemForSaleDisplay = Instantiate(potionItem.prefab);
        itemForSaleDisplay.transform.SetParent(gameObject.transform);
        itemForSaleDisplay.transform.localPosition = new Vector3(0, potionPosition, 0);
        
        isItemForSale = true;
    }

    public void RemoveFromSale() {
        if (itemForSale != null) {
            Destroy(itemForSaleDisplay);
            isItemForSale = false;
            itemForSale = null;
        }
    }

    public KeyValuePair<PotionItem, int> GetPotionInfo() {
        int price = itemSaleUI.GetPrice(this);

        return new KeyValuePair<PotionItem, int>(itemForSale, price);
    }

}

/* just an extra note for future me:
 * when customers purchase an item (isItemForSale must be true):
    * 
    * customer gets potionSaleData by accesing itemSaleUI.potionsaleInfolist[this.id]
    * customer evaluates price 
    * if the price is at a purchase point, isItemForSale = false
        * run some code for the customer to purchase
    * PlayerInventory.RemovePotionSaleItem(item, icon)
        * to get the icon, it SHOULD be potionSaleData.GetPotionIcon() 
        * 
*/
