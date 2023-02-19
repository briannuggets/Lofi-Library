using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script for managing Chest UI
public class Chest : MonoBehaviour
{
    private RectTransform chestView;
    public bool open;
    private List<Transform> items;
    private SingletonLookUp lookup;

    void Awake() {
        items = new List<Transform>();
        chestView = GameObject.Find("ChestView").GetComponent<RectTransform>();
        foreach (Transform item in chestView.transform) {
            if (item.name.StartsWith("Item")) {
                items.Add(item);
            }
        }
        open = false;

        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    void Start() {
        UpdateChest();
    }

    // Closes the chest on "esc" or if the player moves too far away
    void Update() {
        if (Input.GetKeyDown("escape")) {
            if (!open) {
                return;
            }
            Invoke("CloseChestView", 0.05f);
        }
        if (open) {
            if (Vector2.Distance(lookup.GetPlayer().transform.position, transform.position) > 2f) {
                CloseChestView();
            }
        }
    }

    // On-click: On right click, if the player is close enough open the chest
    void OnMouseOver () {
        if (Input.GetMouseButtonDown(1)){
            if (open) {
                return;
            }
            if (Vector2.Distance(transform.position, lookup.GetPlayer().transform.position) < 2f) {
                Invoke("OpenChestView", 0.01f);
            }
        }
    }

    // Open the chest UI
    private void OpenChestView() {
        if (!lookup.GetPlayer().GetComponent<PlayerMovement>().canMove) {
            return;
        }
        chestView.anchoredPosition = new Vector3(0, 0, 0);
        open = true;
        lookup.GetPlaySound().Play(7);
    }

    // Closes the chest UI
    private void CloseChestView() {
        chestView.anchoredPosition = new Vector3(0, -1000, 0);
        open = false;
        lookup.GetPlaySound().Play(8);
    }

    // Update chest UI to reflect current chest items
    public void UpdateChest() {
        int index = 0;
        foreach (InventoryItem item in lookup.GetChestManager().chestInventory) {
            items[index].GetComponent<RawImage>().texture = item.data.icon;
            items[index].GetComponent<RawImage>().color = new Color(255, 255, 255, 1f);
            items[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.stackSize.ToString();
            items[index].transform.GetChild(1).GetComponent<SlotData>().data = item.data;
            index++;
        }

        for (int i = index; i < 18; i++) {
            items[index].GetComponent<RawImage>().texture = null;
            items[index].GetComponent<RawImage>().color = new Color(255, 255, 255, 0f);
            items[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
            items[index].transform.GetChild(1).GetComponent<SlotData>().data = null;
        }
    }
}
