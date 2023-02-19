using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// Script for controlling the UI of the description box
public class ItemDescription : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rect;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI tip;
    private TextMeshProUGUI description;
    public bool open;

    // Start is called before the first frame update
    void Start()
    {
        open = false;
        rect = GetComponent<RectTransform>();
        itemName = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tip = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        description = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    // Open the description box
    public void DisplayDescription(Vector2 position) {
        rect.anchoredPosition = position;
        open = true;
    }

    // Close the description box
    public void CloseDescription() {
        rect.anchoredPosition = new Vector2(0, -500);
        open = false;
    }

    // Sets the text of the description
    public void SetText(string n, string t, string d) {
        itemName.text = n;
        tip.text = t;
        description.text = d;
    }

    // On click: On left click, close the description
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            CloseDescription();
        }
    }

    // Get the name of the item
    public string GetName() {
        return itemName.text;
    }
}
