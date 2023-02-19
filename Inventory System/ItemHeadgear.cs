using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for equiping headgear based on an item interface
public class ItemHeadgear : MonoBehaviour, ItemInterface
{
    private Headgear headgear;
    
    private void Awake() {
        headgear = GameObject.FindWithTag("Player").GetComponent<Headgear>();
    }

    // Equip the headgear
    public void Use(ItemData data) {
        headgear.EquipHeadgear(data.id);
    }
}
