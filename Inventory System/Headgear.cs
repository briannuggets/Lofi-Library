using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for equipping player headgear
public class Headgear : MonoBehaviour
{
    [SerializeField]
    private Sprite[] bunnyBand;
    [SerializeField]
    private Sprite[] wizardHat;
    private PlayerMovement player;
    private int currentHeadgear;
    private SingletonLookUp lookup;

    void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    // Equip headgear given the headgear id
    public void EquipHeadgear(int id) {
        if (id == currentHeadgear || id == -1) {
            currentHeadgear = -1;
            player.WearHeadgear(null);
            lookup.GetHeadgearPersistence().SetId(-1);
            return;
        }
        Sprite[] gear = GetHeadgear(id);
        currentHeadgear = id;
        player.WearHeadgear(gear);
        lookup.GetHeadgearPersistence().SetId(currentHeadgear);
    }

    // Sets headgear when loading into the game
    public void SetHeadGearOnLoad(int id) {
        if (GetHeadgear(id) == null) {
            return;
        }
        lookup.GetPlayer().transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = GetHeadgear(id)[0];
    }

    // Look-up function given a headgear id
    private Sprite[] GetHeadgear(int id) {
        if (id == 7) {
            return bunnyBand;
        } else if (id == 9) {
            return wizardHat;
        }
        return null;
    }
}
