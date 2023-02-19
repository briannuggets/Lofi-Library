using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script for managing scene transitions
public class SceneTransition : MonoBehaviour
{
    private SingletonLookUp lookup;
    private ItemRain itemRain;

    private void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    void Start() {
        itemRain = lookup.GetItemLookUp().gameObject.GetComponent<ItemRain>();
        itemRain.RemoveLowPass();
    }
    
    // Transitions scene to the library; save all player data and apply a lowpass-filter to rain sounds
    private void ChangeScene() {
        lookup.GetPlayer().SetActive(false);
        lookup.GetPlayer().transform.position = new Vector2(1.1f, 1.2f);
        lookup.GetPersistence().SaveDataOnSceneChange();
        itemRain.ApplyLowPass();
        SceneManager.LoadScene("Library");
    }

    // On-click: On right-click, if the player is close enough, change the scene.
    void OnMouseOver(){
        if(Input.GetMouseButtonDown(1)){
            if (Vector2.Distance(transform.position, lookup.GetPlayer().transform.position) < 2.5f) {
                ChangeScene();
            }
        }
    }
}
