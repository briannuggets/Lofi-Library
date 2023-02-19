using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for handling on-click events on the shop
public class ShopOnClick : MonoBehaviour
{
    private SingletonLookUp lookup;
    
    void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    // Closes the shop on "esc" or if the player moves too far away
    void Update() {
        if (Input.GetKeyDown("escape")) {
            if (!lookup.GetShopManager().shopVisible) {
                return;
            }
            lookup.GetShopManager().CloseShopDelay();
            return;
        }
        if (lookup.GetShopManager().shopVisible) {
            if (Vector2.Distance(lookup.GetPlayer().transform.position, transform.position) > 2f) {
                lookup.GetShopManager().CloseShop();
            }
        }
    }

    // On-click: Opens the shop if the player is close enough
    void OnMouseOver(){
        if (Input.GetMouseButtonDown(1)) {
            if (Vector2.Distance(lookup.GetPlayer().transform.position, transform.position) > 2f) {
                return;
            }
            if (lookup.GetPanCamera().inUse) { // Do not open in title-screen
                return;
            }
            lookup.GetShopManager().OpenShop();
        }
    }
}
