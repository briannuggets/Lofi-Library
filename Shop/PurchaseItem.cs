using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script attached to each shop item to handle purchases
public class PurchaseItem : MonoBehaviour
{
    private ItemData data;
    [SerializeField]
    private ItemData currency;
    private SingletonLookUp lookup;

    void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    // Start is called before the first frame update
    void Start()
    {
        data = transform.GetChild(5).GetComponent<SlotData>().data;
    }

    // Purchase the item; handles cases for full inventory or insufficient funds.
    public void Purchase() {
        bool itemFound = false;
        if (lookup.GetInventoryManager().InventoryFull()) {
            if (lookup.GetInventoryManager().HasItem(data) != null) {
                itemFound = true;
            }
            if (!itemFound) {
                lookup.GetNotice().ShowNotice("Inventory full");
                lookup.GetPlaySound().Play(16);
                return;
            }
        }

        if (lookup.GetInventoryManager().NumItems(currency) >= data.cost) {
            lookup.GetInventory().RemoveX(currency, data.cost);
            lookup.GetInventory().Add(data);
            lookup.GetPlaySound().Play(15);
        } else {
            lookup.GetNotice().ShowNotice("Insufficient funds");
            lookup.GetPlaySound().Play(16);
        }
    }
}
