using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{

    private PotionItem potion; //the potion that the customer is going to purchase
    private int potionPrice; //the price of the potion

    private bool willPurchase = false; //will be set to true when the customer wants to purchase the potion 

    public PlayerInventory playerInventory;
    public PlayerBank playerBank;

    public Sprite excitedReaction;
    public Sprite happyReaction;
    public Sprite disappointedReaction;
    public Sprite angryReaction;

    private bool showReaction = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int ReactToItemPrice(PotionItem _potion, int _potionPrice) {
        if (_potionPrice <= _potion.cheapPriceMax)
        {
            ShowCustomerReaction(excitedReaction);

            return 0;
        }
        else if (_potionPrice <= _potion.standardPriceMax)
        {
            ShowCustomerReaction(happyReaction);

            return 1;
        }
        else if (_potionPrice <= _potion.expensivePriceMax)
        {
            ShowCustomerReaction(disappointedReaction);

            return 2;
        }
        else {
            ShowCustomerReaction(angryReaction);

            return 3;
        }
    }

    public void TakePotionFromTable(DisplayTable table) {
        if (table.ItemIcon != null) {
            KeyValuePair<PotionItem, int> info = table.GetPotionInfo();
            potion = info.Key;
            potionPrice = info.Value;
            table.RemoveFromSale();
            playerInventory.RemovePotionSaleItem(table, potion, table.ItemIcon);
        }


    }

    public void MakePurchase() {
        playerBank.AddCoins(potionPrice);
        
    }

    public void ShowCustomerReaction(Sprite reaction) {
        GetComponent<ReactionUI>().UpdateReactionSprite(reaction);
        GetComponent<ReactionUI>().ShowReactionUI();
        StartCoroutine(reactionCounter(3));
    }

    public void HideCustomerReaction() {
        GetComponent<ReactionUI>().HideReactionUI();
    }

    IEnumerator reactionCounter(int seconds) {
        int counter = seconds;
        while (counter > 0) {
            yield return new WaitForSeconds(1);
            counter--;
        }
        HideCustomerReaction();
    }

 
}
