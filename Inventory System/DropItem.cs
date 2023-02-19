using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Script for manipulating inventory on item drop
public class DropItem : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private string typeOfDrop;

    private SingletonLookUp lookup;

    private NPC npc;

    private void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    private void Start() {
        npc = transform.parent.parent.GetComponent<NPC>();
    }

    // On valid item drop, remove the item from inventory
    public void OnDrop(PointerEventData eventData) {
        ItemData d = eventData.pointerDrag.transform.GetChild(1).GetComponent<SlotData>().data;
        eventData.pointerDrag.GetComponent<DragDrop>().SnapBack();
        if (d.type == typeOfDrop) {
            if (typeOfDrop == "affinity" && npc.Giftable()) {
                lookup.GetInventoryManager().Remove(d);
                lookup.GetPlaySound().Play(6);
            } else if (typeOfDrop == "music" && npc.GetID() == 0) {
                lookup.GetInventoryManager().Remove(d);
            }
        }
    }
}
