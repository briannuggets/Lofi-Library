using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for calculating reward drops
public class Reward : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> rewards;
    private int affinityDropRate; // out of 1000
    private int notesDropRate; // out of 1000

    private Countdown cd;
    private RewardManager rm;

    private void Awake() {
        cd = GameObject.Find("Timer").GetComponent<Countdown>();
        rm = GameObject.Find("RewardManager").GetComponent<RewardManager>();
    }

    void Start() {
        affinityDropRate = 333;
        notesDropRate = 800;
    }

    // Gives rewards at the end of a session depending on time studied
    public string GiveRewards() {
        string description = "";
        if (cd.GetSessions() == 0) {
            return description;
        }

        // Give coins
        int coins = 0;
        for (int i = 0; i < cd.GetSessions(); i++) {
            coins += Random.Range(1, 3);
        }
        rm.rewards.Add(new InventoryItem(rewards[0], coins));
        description = coins + "x Silver Coins\n";

        // Roll for other rewards
        int affinityItems = 0;
        for (int i = 0; i < cd.GetSessions(); i++) {
            int roll = Random.Range(0, 1000);
            if (roll < affinityDropRate) {
                rm.Add(rewards[1]);
                affinityItems++;
            }
            if (roll < notesDropRate) {
                int note = Random.Range(2, 6);
                rm.Add(rewards[note]);
                description += "1x " + rewards[note].displayName + "\n";
            }
        }
        if (affinityItems > 0) {
            description += affinityItems.ToString() + "x " + rewards[1].displayName + "\n";
        }
        return description;
    }
}
