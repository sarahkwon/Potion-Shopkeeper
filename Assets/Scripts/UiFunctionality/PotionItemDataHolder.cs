using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItemDataHolder : MonoBehaviour
{
    private Item potion;

    public Item Potion
    {
        get { return potion; }
        set { potion = value; }
    }
}
