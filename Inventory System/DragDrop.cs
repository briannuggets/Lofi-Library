using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Script for handling drag and drop operations on items
public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Vector2 defaultPosition;

    private CanvasGroup canvasGroup;

    private RectTransform icon;

    private ItemData data;

    private SingletonLookUp lookup;

    private void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    void Start() {
        icon = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Check if item was dragged; if true, update opacity
    public void OnBeginDrag(PointerEventData eventData) {
        data = transform.GetChild(1).GetComponent<SlotData>().data;
        if (data == null) {
            return;
        }
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }
    
    // Update object position on drag
    public void OnDrag(PointerEventData eventData) {
        if (data == null) {
            return;
        }
        icon.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // Check if drop was valid and run desired effects
    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (data == null || !data.draggable) {
            SnapBack();
            return;
        }

        bool found = false;
        foreach (GameObject g in eventData.hovered) {
            if (g.name == "MusicDrop" && data.type == "music") {
                lookup.GetMusic().Play(data.musicVolume - 1);
                found = true;
                break;
            } else if (g.name == "AffinityDrop" && data.type == "affinity") {
                g.transform.parent.parent.GetComponent<NPC>().IncrementAffinity();
                found = true;
                break;
            }
        }
        if (!found) { // Snap back into place
            SnapBack();
        }
    }

    // Snap a dragged item back into place
    public void SnapBack() {
        lookup.GetPlaySound().Play(12);
        icon.anchoredPosition = defaultPosition;
    }
}
