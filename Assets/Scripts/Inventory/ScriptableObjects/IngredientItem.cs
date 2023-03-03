using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    White,
    Red,
    Yellow,
    Green,
    Blue,
    Purple
}

[CreateAssetMenu(fileName = "New Ingredient", menuName = "ScriptableObjects/Item/IngredientItem")]
public class IngredientItem : Item
{

    public IngredientType ingredientType;
    public int price;

}
