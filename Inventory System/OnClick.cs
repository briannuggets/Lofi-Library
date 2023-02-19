using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// Script for managing on-click events on inventory items
public class OnClick : MonoBehaviour, IPointerClickHandler
{
    private Canvas canvas;
    private GameObject itemManager;
    private SingletonLookUp lookup;

    private void Awake() {
        canvas = transform.parent.parent.GetComponent<Canvas>();
        itemManager = GameObject.Find("ItemManager");
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    // On left-click: display item description
    // On right-click: if the chest is open, transfer items
    // On right-click: if the item is usable, use the item
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            lookup.GetPlaySound().Play(11);
            ItemData data = transform.GetChild(1).GetComponent<SlotData>().data;
            if (data == null) {
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
            if (data == null) {
                lookup.GetPlaySound().Play(10);
                return;
            }
            if (lookup.GetChest().open) {
                lookup.GetChestManager().Add(data);
                lookup.GetInventory().UpdateInventory();
                lookup.GetChest().UpdateChest();
                lookup.GetPlaySound().Play(9);
                return;
            }

            if (data.type == "egg") {
                itemManager.GetComponent<ItemPet>().Use(data);
            } else if (data.type == "headgear") {
                lookup.GetPlaySound().Play(14);
                itemManager.GetComponent<ItemHeadgear>().Use(data);
            } else if (data.type == "staff") {
                itemManager.GetComponent<ItemRain>().Use(data);
            } else {
                lookup.GetPlaySound().Play(13);
            }
        }
    }
}
