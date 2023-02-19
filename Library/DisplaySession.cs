using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Script for managing and displaying the results screen.
public class DisplaySession : MonoBehaviour
{
    private Countdown cd;
    private Reward rw;

    private GameObject results;
    private TextMeshProUGUI sessions;
    private TextMeshProUGUI time;
    private TextMeshProUGUI rewards;

    private void Awake() {
        cd = GameObject.Find("Timer").GetComponent<Countdown>();
        rw = GameObject.Find("Reward").GetComponent<Reward>();
        results = GameObject.Find("Results");
        sessions = results.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        time = results.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        rewards = results.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        results.SetActive(false);
    }

    // Display results of the current session
    public void Display() {
        results.SetActive(true);
        sessions.text += " " + cd.GetSessions();
        time.text += " " + cd.GetTime();
        rewards.text += "\n" + rw.GiveRewards();
    }

    // Load the next scene
    public void LeaveDisplay() {
        SceneManager.LoadScene("Outside");
    }
}
