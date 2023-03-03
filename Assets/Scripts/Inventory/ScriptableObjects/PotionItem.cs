using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "ScriptableObjects/Item/PotionItem")]
public class PotionItem : Item
{

    public GameObject prefab;

    //too cheap range
    public int cheapPriceMax;
    //anything <= cheapPriceMax is too cheap 

    //good standard price range
    public int standardPriceMax;

    //a bit too expensive range
    public int expensivePriceMax;

    //wayyy too expensive
    //anything >= than expensivePriceMax is going to make customers storm out

    //an array of ingredients that make up the potion recipe
    public List<IngredientType> recipe;
}
