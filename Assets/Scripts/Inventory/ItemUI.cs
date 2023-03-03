using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    private InventorySlot inventorySlot;
    private PotionSaleSlot potionSaleSlot;
    private IngredientCraftSlot craftSlot;

    public AudioClip beginDragSound;
    public AudioClip endDragSound;
    public AudioSource iconAudioSource;

    private void Start()
    {

    }


    public InventorySlot GetInventorySlot() {
        return inventorySlot;
    }

    public void SetInventorySlot(InventorySlot slot) {
        inventorySlot = slot;
    }

    public PotionSaleSlot GetPotionSaleSlot() {
        return potionSaleSlot;
    }

    public void SetPotionSaleSlot(PotionSaleSlot slot) {
        potionSaleSlot = slot;
    }

    public IngredientCraftSlot GetCraftSlot() {
        return craftSlot;
    }

    public void SetCraftSlot(IngredientCraftSlot slot) {
        craftSlot = slot;
    }
}
