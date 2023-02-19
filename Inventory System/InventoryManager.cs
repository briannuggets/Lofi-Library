using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for managing the player's current inventory, as well adding and removing items
public class InventoryManager : MonoBehaviour
{
    private Dictionary<ItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory;
    private SingletonLookUp lookup;

    void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    void Start() {
        // DontDestroyOnLoad(gameObject);
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<ItemData, InventoryItem>();
    }

    // Add an item to inventory given reference data
    public void Add(ItemData referenceData) {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value)) { // Increment stack
            value.AddToStack();
        } else { // Add to inventory
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            itemDictionary.Add(referenceData, newItem);
        }
        lookup.GetInventory().UpdateInventory();
    }

    // Add an item to inventory given an inventory item
    public void Add(InventoryItem inventoryItem) {
        if (itemDictionary.TryGetValue(inventoryItem.data, out InventoryItem value)) {
            int currentAmount = value.stackSize;
            int addedAmount = inventoryItem.stackSize;
            inventory.Remove(value);
            inventory.Add(new InventoryItem(inventoryItem.data, currentAmount + addedAmount));
        } else {
            inventory.Add(inventoryItem);
            itemDictionary.Add(inventoryItem.data, inventoryItem);
        }
        lookup.GetInventory().UpdateInventory();
    }

    // Remove 1 of the current item from the player's inventory
    public void Remove(ItemData referenceData) {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value)) {
            value.RemoveFromStack();
            if (value.stackSize == 0) {
                inventory.Remove(value);
                itemDictionary.Remove(referenceData);
            }
            lookup.GetInventory().UpdateInventory();
        }
    }

    // Remove all of the given item from the player's inventory
    public void RemoveAll(ItemData referenceData) {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value)) {
            inventory.Remove(value);
            itemDictionary.Remove(referenceData);
        }
    }

    // Returns true if the item exists in the player's inventory
    public InventoryItem HasItem(ItemData referenceData) {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value)) {
            return value;
        }
        return null;
    }

    // Given an item, return the current item stack-size in the inventory
    public int NumItems(ItemData referenceData) {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value)) {
            return value.stackSize;
        }
        return 0;
    }

    // Load a saved inventory state
    public void LoadSavedInventory(int[] items) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] > 0) {
                Add(new InventoryItem(lookup.GetItemLookUp().GetItem(i), items[i]));
            }
        }
    }

    // Get the current inventory state in array format
    public int[] GetCurrentInventory() {
        int[] current = new int[lookup.GetItemLookUp().TableSize()];
        foreach (InventoryItem i in inventory) {
            current[i.data.id] = i.stackSize;
        }
        return current;
    }

    // Updates UI for both inventory and chest
    public void UpdateUI() {
        lookup.GetInventory().UpdateInventory();
        lookup.GetChest().UpdateChest();
    }

    // Returns true if inventory is full
    public bool InventoryFull() {
        if (inventory.Count > 7) {
            return true;
        }
        return false;
    }
}
