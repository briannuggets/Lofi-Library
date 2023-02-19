using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// Script to enable "cheat-mode"; allows player to get unlimited items
public class CheatBox : MonoBehaviour
{
    private RectTransform panel;
    private bool open;
    private TMP_InputField input;
    private SingletonLookUp lookup;

    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<RectTransform>();
        open = false;
        input = GetComponent<TMP_InputField>();
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (lookup.GetPanCamera().inUse) {
                    return;
                }
                ToggleCheat();
            }
        }
        if (open) {
            if (Input.GetKeyUp(KeyCode.Return)) {
                ParseText(input.text);
                input.text = "";
            }
            input.Select();
            input.ActivateInputField();
        }
    }

    // Toggles the cheat box UI
    private void ToggleCheat() {
        if (open) {
            lookup.GetPlayer().GetComponent<PlayerMovement>().canMove = true;
            open = false;
            panel.anchoredPosition = new Vector2(0, 1000);
        } else {
            input.Select();
            lookup.GetPlayer().GetComponent<PlayerMovement>().canMove = false;
            open = true;
            panel.anchoredPosition = new Vector2(0, 0);
        }
    }

    // Parse text for cheat-mode; handles cases for inventory full and invalid input.
    // Format: "get <item id> <quantity>"
    private void ParseText(string text) {
        string[] split = text.Split(' ');
        if (split.Length != 3) {
            lookup.GetNotice().ShowNotice("Invalid command");
            lookup.GetPlaySound().Play(16);
            return;
        }

        if (split[0] != "get") {
            lookup.GetNotice().ShowNotice("Invalid command");
            lookup.GetPlaySound().Play(16);
            return;
        }

        // Limit quantity to positive numbers < 1000
        int quantity = 0;
        if (!Int32.TryParse(split[2], out quantity)) {
            lookup.GetNotice().ShowNotice("Invalid quantity");
            lookup.GetPlaySound().Play(16);
            return;
        }
        if (quantity < 1 || quantity > 999) {
            lookup.GetNotice().ShowNotice("Invalid quantity");
            lookup.GetPlaySound().Play(16);
            return;
        }

        // Parse valid item id
        int x = -1;
        if (!Int32.TryParse(split[1], out x)) {
            lookup.GetNotice().ShowNotice("Item not found");
            lookup.GetPlaySound().Play(16);
            return;
        }
        ItemData data = lookup.GetItemLookUp().GetItem(x);
        if (data == null) {
            lookup.GetNotice().ShowNotice("Item not found");
            lookup.GetPlaySound().Play(16);
            return;
        }

        // Handle cases for full inventory
        if (lookup.GetInventoryManager().InventoryFull()) {
            if (lookup.GetInventoryManager().HasItem(data) == null) {
                lookup.GetNotice().ShowNotice("Inventory full");
                lookup.GetPlaySound().Play(16);
                return;
            }
            lookup.GetPlaySound().Play(15);
            for (int i = 0; i < quantity; i++) {
                lookup.GetInventory().Add(data);
            }
            return;
        }

        // Correct usage; inventory not full
        lookup.GetPlaySound().Play(15);
        for (int i = 0; i < quantity; i++) {
            lookup.GetInventory().Add(data);
        }
    }
}
