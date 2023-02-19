using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static script: Used for storing and retrieving rewards between scenes
public class RewardManager : MonoBehaviour
{
    private static RewardManager rewardInstance;
    public List<InventoryItem> rewards;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        if (rewardInstance == null) {
            rewardInstance = this;
            rewards = new List<InventoryItem>();
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Add an item to the reward list
    public void Add(ItemData referenceData) {
        InventoryItem found = null;
        foreach (InventoryItem i in rewards) {
            if (referenceData.id == i.data.id) {
                found = i;
                break;
            }
        }
        if (found != null) {
            found.stackSize++;
        } else {
            rewards.Add(new InventoryItem(referenceData, 1));
        }
    }

    // Clear the reward list
    public void ClearRewards() {
        rewards.Clear();
    }

    // Retrieve the reward list
    public List<InventoryItem> GetRewards() {
        return rewards;
    }

}
