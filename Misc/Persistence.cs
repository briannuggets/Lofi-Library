using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script for setting and loading save data on scene change and application exit
public class Persistence : MonoBehaviour
{
    private SingletonLookUp lookup;
    private MenuManager menuManager;
    private Image transition;

    void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
        menuManager = GameObject.Find("Menu").GetComponent<MenuManager>();
        transition = GameObject.Find("TitleTransition").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json")) {
            string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
            PlayerData saved = JsonUtility.FromJson<PlayerData>(json);

            // Player position
            lookup.GetPlayer().SetActive(true);
            lookup.GetPlayer().transform.position = saved.position;

            // Items
            lookup.GetInventoryManager().LoadSavedInventory(saved.invItems);
            lookup.GetChestManager().LoadSavedChest(saved.chestItems);
            lookup.GetChest().UpdateChest();
            lookup.GetInventory().UpdateInventory();

            // NPCs
            lookup.GetNPCState().LoadNPCStates(saved.npcStates);

            // Pet
            lookup.GetItemPet().SetPet(saved.activePet);

            // Headgear
            lookup.GetHeadgear().EquipHeadgear(saved.activeHeadgear);
            lookup.GetHeadgear().SetHeadGearOnLoad(saved.activeHeadgear);

            // Music
            if (!saved.musicToggled) {
                menuManager.ToggleMusic();
            } else {
                StartCoroutine(lookup.GetMusic().FadeInMusic());
            }

            // Add rewards on scene change
            if (lookup.GetRewardManager().GetRewards().Count > 0) {
                foreach (InventoryItem item in lookup.GetRewardManager().GetRewards()) {
                    lookup.GetChestManager().AddReward(item, item.stackSize);
                }
                lookup.GetRewardManager().ClearRewards();
                lookup.GetChest().UpdateChest();
                lookup.GetNotice().ShowNotice("Rewards sent to chest");
            }

            // Check if player came from dungeon, disable titlescreen accordingly
            if (saved.wasInDungeon) {
                StartCoroutine(FadeIn());
                lookup.GetPanCamera().DisableTitle();
            }
        } else {
            lookup.GetMusic().gameObject.GetComponent<AudioSource>().time = 0f;
        }
    }

    // Save current player state and data
    public void SavePlayerData(bool inDungeon) {
        File.Delete(Application.persistentDataPath + "/save.json");
        PlayerData pd = new PlayerData();

        // Player position
        pd.position = lookup.GetPlayer().transform.position;

        // Items
        pd.invItems = lookup.GetInventoryManager().GetCurrentInventory();
        pd.chestItems = lookup.GetChestManager().GetCurrentChest();

        // NPC
        pd.npcStates = lookup.GetNPCState().GetCurrentStates();

        // Pet
        pd.activePet = lookup.GetPetPersistence().GetId();

        // Headgear
        pd.activeHeadgear = lookup.GetHeadgearPersistence().GetId();

        // Music Toggle
        pd.musicToggled = menuManager.MusicToggled();

        // In Dungeon
        pd.wasInDungeon = inDungeon;

        string overwrite = JsonUtility.ToJson(pd);
        File.WriteAllText(Application.persistentDataPath + "/save.json", overwrite);
    }

    // Coroutine: Fade in transition (called when returning from dungeon)
    private IEnumerator FadeIn() {
        transition.gameObject.SetActive(true);
        transition.color = new Color(1f, 1f, 1f, 1f);
        float counter = 3f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            transition.color = new Color(1f, 1f, 1f, transition.color.a - 0.04f);
            counter = counter - 0.1f;
        }
        transition.gameObject.SetActive(false);
    }

    // Variables to manage player save-data
    public class PlayerData {
        public Vector3 position;
        public int[] invItems;
        public int[] chestItems;
        public int[] npcStates;
        public int activePet;
        public int activeHeadgear;
        public bool musicToggled;
        public bool wasInDungeon;
    }

    // Save player data on application quit; enable title screen on next load
    void OnApplicationQuit() {
        SavePlayerData(false);
    }

    // Save player data on scene change; disable title screen on next load
    public void SaveDataOnSceneChange() {
        SavePlayerData(true);
    }
}
