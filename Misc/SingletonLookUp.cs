using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Retrieval script for looking up Singleton-classes.
// Convenient for scripts where many singletons are needed.
public class SingletonLookUp : MonoBehaviour
{
    private Chest chest;
    private ChestManager chestManager;
    private Headgear headgear;
    private Inventory inventory;
    private InventoryManager inventoryManager;
    private ItemDescription itemDescription;
    private Music music;
    private PlaySound playSound;
    private AffinityBanner affinityBanner;
    private GameObject player;
    private ItemLookUp itemLookUp;
    private NPCState npcState;
    private Persistence persistence;
    private RewardManager rewardManager;
    private PetPersistence petPersistence;
    private ItemPet itemPet;
    private HeadgearPersistence headgearPersistence;
    private ShopManager shopManager;
    private Notice notice;
    private PanCamera panCamera;

    private void Awake() {
        chest = GameObject.Find("Chest").GetComponent<Chest>();
        chestManager = GameObject.Find("InventoryManager").GetComponent<ChestManager>();
        headgear = GameObject.FindWithTag("Player").GetComponent<Headgear>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        itemDescription = GameObject.Find("Description").GetComponent<ItemDescription>();
        music = GameObject.Find("MusicManager").GetComponent<Music>();
        playSound = GameObject.Find("AudioPlayer").GetComponent<PlaySound>();
        affinityBanner = GameObject.Find("AffinityBanner").GetComponent<AffinityBanner>();
        player = GameObject.FindWithTag("Player");
        itemLookUp = GameObject.Find("ItemManager").GetComponent<ItemLookUp>();
        npcState = GameObject.Find("NPCs").GetComponent<NPCState>();
        persistence = GameObject.Find("Persistence").GetComponent<Persistence>();
        rewardManager = GameObject.Find("RewardManager").GetComponent<RewardManager>();
        petPersistence = GameObject.Find("Pet").GetComponent<PetPersistence>();
        itemPet = GameObject.Find("ItemManager").GetComponent<ItemPet>();
        headgearPersistence = GameObject.Find("HeadgearManager").GetComponent<HeadgearPersistence>();
        shopManager = GameObject.Find("ShopView").GetComponent<ShopManager>();
        notice = GameObject.Find("Notice").GetComponent<Notice>();
        panCamera = GameObject.Find("Start Camera").GetComponent<PanCamera>();
    }

    public Chest GetChest() {
        return chest;
    }

    public ChestManager GetChestManager() {
        return chestManager;
    }

    public Headgear GetHeadgear() {
        return headgear;
    }

    public Inventory GetInventory() {
        return inventory;
    }

    public InventoryManager GetInventoryManager() {
        return inventoryManager;
    }

    public ItemDescription GetItemDescription() {
        return itemDescription;
    }

    public Music GetMusic() {
        return music;
    }

    public PlaySound GetPlaySound() {
        return playSound;
    }

    public AffinityBanner GetAffinityBanner() {
        return affinityBanner;
    }

    public GameObject GetPlayer() {
        return player;
    }

    public ItemLookUp GetItemLookUp() {
        return itemLookUp;
    }

    public NPCState GetNPCState() {
        return npcState;
    }

    public Persistence GetPersistence() {
        return persistence;
    }

    public RewardManager GetRewardManager() {
        return rewardManager;
    }

    public PetPersistence GetPetPersistence() {
        return petPersistence;
    }

    public ItemPet GetItemPet() {
        return itemPet;
    }

    public HeadgearPersistence GetHeadgearPersistence() {
        return headgearPersistence;
    }

    public ShopManager GetShopManager() {
        return shopManager;
    }

    public Notice GetNotice() {
        return notice;
    }

    public PanCamera GetPanCamera() {
        return panCamera;
    }
}
