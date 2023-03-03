using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    [TextArea(15, 20)]
    public string description;
    public Sprite icon;
}
