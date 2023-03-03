using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventoryItem> Container = new List<InventoryItem>();

    public int AddItem(Item _item, int _amount = 1) {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++) {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                if (Container[i].amount > 99)
                {
                    Container[i].amount = 99;
                }
                hasItem = true;
                return i;
            }
        }

        if (!hasItem) {
            if (_amount > 99) {
                _amount = 99;
            }
            Container.Add(new InventoryItem(_item, _amount));
            
            return Container.Count - 1;
        }

        return -1; //should never reach here
    }

    /* return values: 
     * 0 - inf = index
     * -1 = removed item
     * -2 = _item does not exist
     */
    public int RemoveItem(Item _item, int _amount) {
        for (int i = 0; i < Container.Count; i++) {
            if (Container[i].item == _item)
            {
                Container[i].RemoveAmount(_amount);
                if (Container[i].amount <= 0) {
                    //Container[i].amount = 0;
                    Container.RemoveAt(i);
                    return -1;
                    
                }
                return i;
            }
        }

        return -2;

    }

    public bool HasItem(Item _item) {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                return true;
            }
        }

        return false;
    }

    public int SetItemQty(Item _item, int _value) {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].SetAmount(_value);
                return i;
            }
        }


        return -1; //specified item does not exist
    }

    public InventoryItem GetInventoryItem(int index) {
        if (index >= 0 && index < Container.Count)
        {
            return Container[index];
        }

        return null;
    }
}

[System.Serializable]
public class InventoryItem {
    public Item item;
    public int amount;

    public InventoryItem(Item _item, int _amount) {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }

    public void RemoveAmount(int value) {
        amount -= value;
    }

    public void SetAmount(int value) {
        amount = value;
    }

}