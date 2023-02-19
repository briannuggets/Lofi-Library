using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for retrieving and loading NPC affinity levels
public class NPCState : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> npcLookUp;

    // Sets affinity levels based on the retrieved states array
    public void LoadNPCStates(int[] states) {
        for (int i = 0; i < states.Length; i++) {
            NPC npc = npcLookUp[i].GetComponent<NPC>();
            if (npc != null) {
                npc.LoadNPCState(states[npc.GetID()]);
            }
        }
    }

    // Returns an array of the current NPC affinity levels
    public int[] GetCurrentStates() {
        int[] states = new int[npcLookUp.Count];
        foreach (GameObject g in npcLookUp) {
            NPC npc = g.GetComponent<NPC>();
            if (npc != null) {
                states[npc.GetID()] = npc.affinity;
            }
        }
        return states;
    }
}
