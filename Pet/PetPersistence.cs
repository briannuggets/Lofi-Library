using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static script: saves the current pet when entering and leaving the library
public class PetPersistence : MonoBehaviour
{
    private static PetPersistence instance;
    private int petId;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null) {
            instance = this;
            petId = -1;
        } else {
            Destroy(gameObject);
        }
    }

    // Sets the Id of the current pet
    public void SetId(int id) {
        petId = id;
    }

    // Gets the current pet Id
    public int GetId() {
        return petId;
    }
}
