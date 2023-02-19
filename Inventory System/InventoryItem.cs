using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Item class including data and size, to be managed by InventoryManager
[Serializable]
public class InventoryItem 
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData source) {
        data = source;
        AddToStack();
    }

    public InventoryItem(ItemData source, int amount) {
        data = source;
        stackSize = amount;
    }

    // Increment item quantity
    public void AddToStack() {
        stackSize++;
    }

    // Decrement item quantity
    public void RemoveFromStack() {
        stackSize--;
    }

}

