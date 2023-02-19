using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to manage items being added and removed from the chest
public class ChestManager : MonoBehaviour
{
    public List<InventoryItem> chestInventory;
    private InventoryManager inventoryManager;
    private ItemLookUp itemLookUp;

    void Awake() {
        itemLookUp = GameObject.Find("ItemManager").GetComponent<ItemLookUp>();
    }

    // Start is called before the first frame update
    void Start()
    {
        chestInventory = new List<InventoryItem>();
        inventoryManager = GetComponent<InventoryManager>();
    }

    // Add an item to the chest and remove it from player inventory
    public void Add(ItemData referenceData) {
        InventoryItem found = null;
        foreach (InventoryItem item in inventoryManager.inventory) {
            if (item.data.id == referenceData.id) {
                found = item;
                break;
            }
        }
        if (found != null) {
            inventoryManager.RemoveAll(found.data);

            int amountInsideChest = 0;
            foreach (InventoryItem i in chestInventory) {
                if (i.data.id == referenceData.id) {
                    amountInsideChest = i.stackSize;
                    chestInventory.Remove(i);
                    break;
                }
            }
            chestInventory.Add(new InventoryItem(referenceData, found.stackSize + amountInsideChest));
            inventoryManager.UpdateUI();
        }
    }

    // Add a reward to the chest
    public void AddReward(InventoryItem reward, int amountRewarded) {
        int amountInChest = 0;
        foreach (InventoryItem item in chestInventory) {
            if (item.data.id == reward.data.id) {
                amountInChest = item.stackSize;
                chestInventory.Remove(item);
                break;
            }
        }
        chestInventory.Add(new InventoryItem(reward.data, amountInChest + amountRewarded));
    }

    // Remove an item from the chest and add it to the inventory
    public void Remove(ItemData referenceData) {
        InventoryItem found = null;
        foreach (InventoryItem item in chestInventory) {
            if (item.data.id == referenceData.id) {
                found = item;
                break;
            }
        }
        if (found != null) {
            chestInventory.Remove(found);
            inventoryManager.Add(found);
            inventoryManager.UpdateUI();
        }
    }

    // Given a chest-state, load items into the chest
    public void LoadSavedChest(int[] items) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] > 0) {
                chestInventory.Add(new InventoryItem(itemLookUp.GetItem(i), items[i]));
            }
        }
    }

    // Get current chest inventory state in array format
    public int[] GetCurrentChest() {
        int[] current = new int[itemLookUp.TableSize()];
        foreach (InventoryItem i in chestInventory) {
            current[i.data.id] = i.stackSize;
        }
        return current;
    }
}
