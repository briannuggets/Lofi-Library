using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script for manipulating shop UI
public class ShopManager : MonoBehaviour
{
    public bool shopVisible;
    private RectTransform panel;
    private Scrollbar scroll;
    private PlaySound playSound;
    private PlayerMovement playerMovement;

    void Awake() {
        playSound = GameObject.Find("AudioPlayer").GetComponent<PlaySound>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        shopVisible = false;
        panel = GetComponent<RectTransform>();
        scroll = transform.GetChild(1).GetComponent<Scrollbar>();
        foreach (Transform item in transform.GetChild(0)) {
            SetDescription(item);
            SetImage(item);
        }
    }

    // On open reset the scroll value; do not open if the player is in the main menu
    public void OpenShop() {
        if (!playerMovement.canMove) {
            return;
        }
        panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, 0);
        scroll.value = 1;
        shopVisible = true;
        playSound.Play(7);
    }

    // Closes the shop
    public void CloseShop() {
        panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, 1000);
        shopVisible = false;
        playSound.Play(8);
    }

    // Closes the shop with a slight delay (to deal with 'esc' menu conflicts)
    public void CloseShopDelay() {
        Invoke("CloseShop", 0.05f);
    }

    // Retrieves the data of shop item.
    private ItemData GetData(Transform item) {
        return item.GetChild(5).GetComponent<SlotData>().data;
    }

    // Sets the UI description of a shop item.
    private void SetDescription(Transform item) {
        item.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetData(item).displayName + "\n" + GetData(item).itemDescription;
        item.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = GetData(item).cost + "g";
    }

    // Sets the UI image of a shop item.
    private void SetImage(Transform item) {
        item.GetChild(4).GetComponent<RawImage>().texture = GetData(item).icon;
    }
}
