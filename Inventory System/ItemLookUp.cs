using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Look-up class for retrieving all items in the game
public class ItemLookUp : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> itemTable;

    // Get an item given the item id
    public ItemData GetItem(int id) {
        if (id < 0) {
            return null;
        }
        if (id < itemTable.Count) {
            return itemTable[id];
        }
        return null;
    }

    // Returns the table size
    public int TableSize() {
        return itemTable.Count;
    }
}
