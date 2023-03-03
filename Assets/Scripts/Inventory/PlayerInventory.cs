using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryObject potionInventory;
    [SerializeField] private InventoryObject ingredientInventory;
    [SerializeField] private InventoryObject recipeInventory;
    [SerializeField] private InventoryUI potionInventoryUI;
    [SerializeField] private InventoryUI ingredientInventoryUI;
    [SerializeField] private InventoryUI recipeInventoryUI;

    [SerializeField] private InventoryObject potionSaleInventory;
    [SerializeField] private ItemSaleUI potionSaleUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void RemovePotionItem(Item _item, int _amount) {
        int index = potionInventory.RemoveItem(_item, _amount);
        //index still returns a value when the item is removed from list
        if (index != -1)
        {
            potionInventoryUI.UpdateItemUI(potionInventory.GetInventoryItem(index));
        }
        else if (index == -1) {
            potionInventoryUI.UpdateItemUI(null);
        }
        else
        {
            Debug.Log("ERROR: Null Index at RemovePotionItem in PlayerInventory.cs");
        }
        
    }

    public void EmptyAll() { 
        potionInventory.Container.Clear();
        recipeInventory.Container.Clear();
        potionSaleInventory.Container.Clear();
        ingredientInventory.Container.Clear();
    }

    public void AddPotionItem(Item _item, int _amount) {
        int index = potionInventory.AddItem(_item, _amount);
        if (index != -1)
        {
            
            potionInventoryUI.UpdateItemUI(potionInventory.GetInventoryItem(index));

            
        }
        else {
            Debug.Log("ERROR: Null Index at AddPotionItem in PlayerInventory.cs");
        }
        
    }

    public void SetPotionQty(Item _item, int value) { 
        
    }

    public void RemoveIngredientItem(Item _item) {}

    public void AddIngredientItem(Item _item) { 
        int index = ingredientInventory.AddItem(_item);
        if (index != -1)
        {
            ingredientInventoryUI.UpdateItemUI(ingredientInventory.GetInventoryItem(index));
        }
        else {
            Debug.Log("ERROR: Null Index at AddIngredientItem in PlayerInventory.cs");
        }
        
    }

    //returns the icon that has been instantiated
    public GameObject AddPotionSaleItem(Item _item) {
        int index = potionSaleInventory.AddItem(_item, 1);
        if (index != -1)
        {
            return potionSaleUI.UpdateItemUI(potionSaleInventory.GetInventoryItem(index));
        }
        else {
            Debug.Log("ERROR: Null Index at AddPotionSaleItem in PlayerInventory.cs");
        }

        return null;
        
    }

    public void RemovePotionSaleItem(Item _item, GameObject icon) {
        AddPotionItem(_item, 1); //add it back from temp inv to player inventory
        

        int index = potionSaleInventory.RemoveItem(_item, 1);
        potionSaleUI.DeleteItemUI(icon);
    }

    //where a customer removes the item
    public void RemovePotionSaleItem(DisplayTable table, Item _item, GameObject icon)
    {
        int index = potionSaleInventory.RemoveItem(_item, 1);
        potionSaleUI.DeleteItemUI(table, icon);
    }

    public bool HasPotionSaleItem(Item _item) {
        return potionSaleInventory.HasItem(_item);
    }

    public bool HasIngredientItem(Item _item) {
        return ingredientInventory.HasItem(_item);
    }

    public void AddRecipe(Item _item)
    {
        int index = recipeInventory.AddItem(_item);
        Debug.Log("runing 1");
        if (index != -1)
        {
            Debug.Log("Running 2");
            recipeInventoryUI.UpdateItemUI(recipeInventory.GetInventoryItem(index));


        }
        else
        {
            Debug.Log("ERROR: Null Index at AddPotionItem in PlayerInventory.cs");
        }

    }

    public bool HasRecipe(Item _item) {
        return recipeInventory.HasItem(_item);
    }


    public Item GetItemFromIcon(GameObject icon) {

        if (ingredientInventoryUI == null) {
            Debug.Log("inven null");
        }

        if (icon == null) {
            Debug.Log("icon null");
        }
      
        InventoryItem currItem = ingredientInventoryUI.GetInventoryItem(icon);
        if (currItem != null) {
            return currItem.item;
        }

        currItem = potionInventoryUI.GetInventoryItem(icon);
        if (currItem != null) {
            return currItem.item;
        }

        currItem = potionSaleUI.GetSaleItem(icon);
        if (currItem != null) {
            return currItem.item;
        }

        return null;
    }

}
