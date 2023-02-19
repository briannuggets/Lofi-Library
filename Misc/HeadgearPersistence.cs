using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static script: Saves and loads player headgear on scene-change
public class HeadgearPersistence : MonoBehaviour
{
    private static HeadgearPersistence instance;
    private int headgearId;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null) {
            instance = this;
            headgearId = -1;
        } else {
            Destroy(gameObject);
        }
    }

    // Sets the current headgear Id
    public void SetId(int id) {
        headgearId = id;
    }

    // Gets the current headgear Id
    public int GetId() {
        return headgearId;
    }
}
