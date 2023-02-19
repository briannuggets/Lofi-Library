using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Script to manage on-click events for chest items
public class ChestOnClick : MonoBehaviour, IPointerClickHandler
{
    private Canvas canvas;
    private SingletonLookUp lookup;
    
    void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
        canvas = transform.parent.parent.GetComponent<Canvas>();
    }
    
    // On-Click: On left click, display item description
    // On-Click: On right click, remove item from the chest
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            lookup.GetPlaySound().Play(11);
            ItemData data = transform.GetChild(1).GetComponent<SlotData>().data;
            if (data == null) { // Player clicks on empty slot
                lookup.GetItemDescription().CloseDescription();
                return;
            }
            if (lookup.GetItemDescription().open && lookup.GetItemDescription().GetName() == data.displayName) {
                lookup.GetItemDescription().CloseDescription();
                return;
            }
            lookup.GetItemDescription().DisplayDescription(eventData.position / canvas.scaleFactor);
            lookup.GetItemDescription().SetText(data.displayName, data.tip, data.itemDescription);
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            ItemData data = transform.GetChild(1).GetComponent<SlotData>().data;
            if (data == null) { // Player clicks on empty slot
                lookup.GetPlaySound().Play(10);
                return;
            }
            if (lookup.GetInventoryManager().InventoryFull()) { // Do not remove if full inventory
                lookup.GetPlaySound().Play(16);
                lookup.GetNotice().ShowNotice("Inventory full");
                return;
            }
            lookup.GetPlaySound().Play(9);
            lookup.GetChestManager().Remove(data);
            lookup.GetInventory().UpdateInventory();
            lookup.GetChest().UpdateChest();
        }
    }
}
