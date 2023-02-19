using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for spawning and despawning pets implementing the item interface
public class ItemPet : MonoBehaviour, ItemInterface
{
    [SerializeField]
    private GameObject slime;

    [SerializeField]
    private GameObject slimeHat;

    private GameObject currentPet;
    private int currentPetId;

    private SingletonLookUp lookup;

    private void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    // Sets the pet on application load
    public void SetPet(int id) {
        if (id == -1) {
            currentPet = null;
            currentPetId = id;
            lookup.GetPetPersistence().SetId(-1);
            return;
        }
        if (GetPet(id) == null) {
            return;
        }
        currentPetId = id;
        currentPet = Instantiate(GetPet(id), lookup.GetPlayer().transform.position, Quaternion.identity);
        lookup.GetPetPersistence().SetId(currentPetId);
    }

    // Look-up function to get a pet given the pet id
    private GameObject GetPet(int id) {
        if (id == 5) {
            return slime;
        } else if (id == 10) {
            return slimeHat;
        }
        return null;
    }

    // Spawn and despawn the pet
    public void Use(ItemData data) {
        GameObject pet = GetPet(data.id);
        if (currentPetId == -1) { // No current pet
            currentPetId = data.id;
            currentPet = Instantiate(pet, lookup.GetPlayer().transform.position, Quaternion.identity);
            lookup.GetPlaySound().Play(1);
        } else if (currentPetId == data.id) { // Despawn pet
            Destroy(currentPet);
            currentPet = null;
            currentPetId = -1;
            lookup.GetPlaySound().Play(2);
        } else { // Remove current pet, spawn new pet
            currentPetId = data.id;
            Destroy(currentPet);
            currentPet = Instantiate(pet, lookup.GetPlayer().transform.position, Quaternion.identity);
            lookup.GetPlaySound().Play(1);
        }
        lookup.GetPetPersistence().SetId(currentPetId);
    }
}
