using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script for manipulating inventory UI and inventory items
public class Inventory : MonoBehaviour
{
    private RectTransform panel;
    private List<Transform> items;
    private Animator playerAnimator;
    private bool fading;

    private SingletonLookUp lookup;

    private void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
        playerAnimator = lookup.GetPlayer().GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fading = true;
        panel = GetComponent<RectTransform>();
        items = new List<Transform>();
        foreach (Transform item in transform) {
            if (item.name.StartsWith("Item")) {
                items.Add(item);
            }
        }
        UpdateInventory();
    }

    // Moves inventory slots off screen during the title-screen and on idle.
    private void FixedUpdate() {
        if (lookup.GetPanCamera().inUse) {
            panel.anchoredPosition = new Vector2(0, 2000);
            return;
        }

        if (playerAnimator.GetBool("idle") && !fading) {
            StartCoroutine(IdleCountdown());
        } else if (!playerAnimator.GetBool("idle")) {
            StopAllCoroutines();
            fading = false;
            panel.anchoredPosition = new Vector2(0, 100);
        }

        if (Input.GetAxis("Mouse X") != 0) {
            StopAllCoroutines();
            fading = false;
            panel.anchoredPosition = new Vector2(0, 100);
        }
    }

    // Coroutine: moves inventory slots off-screen on player idle.
    private IEnumerator IdleCountdown() {
        // Idle timer
        fading = true;
        int counter = 10;
        while (counter > 0) {
            yield return new WaitForSeconds(1);
            counter--;
        }

        // Inventory fade away
        float counter2 = 2f;
        while (counter2 > 0) {
            yield return new WaitForSeconds(0.01f);
            panel.anchoredPosition = new Vector2(0, panel.anchoredPosition.y - 1);
            counter2 = counter2 - 0.01f;
        }
    }

    // Update inventory UI to reflect current player inventory
    public void UpdateInventory() {
        int index = 0;
        foreach (InventoryItem i in lookup.GetInventoryManager().inventory) {
            items[index].GetComponent<RawImage>().texture = i.data.icon;
            items[index].GetComponent<RawImage>().color = new Color(255, 255, 255, 1f);
            items[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.stackSize.ToString();
            items[index].transform.GetChild(1).GetComponent<SlotData>().data = i.data;
            index++;
            if (index > 7) {
                break;
            }
        }
        for (int i = index; i < 8; i++) {
            items[index].GetComponent<RawImage>().texture = null;
            items[index].GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
            items[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
            items[index].transform.GetChild(1).GetComponent<SlotData>().data = null;
        }
    }

    // Add an item to player inventory
    public void Add(ItemData item) {
        lookup.GetInventoryManager().Add(item);
        UpdateInventory();
    }

    // Remove 1 item from player inventory
    public void Remove(ItemData item) {
        lookup.GetInventoryManager().Remove(item);
        UpdateInventory();
    }

    // Remove x quantity of a given item from player inventory
    public void RemoveX(ItemData item, int x) {
        for (int i = 0; i < x; i++) {
            lookup.GetInventoryManager().Remove(item);
        }
        UpdateInventory();
    }

}
