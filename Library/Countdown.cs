using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

// Script for managing the countdown timer (based on Pomodoro principle).
// ++ Handles application quit on dungeon ++
public class Countdown : MonoBehaviour
{
    private int defaultTimerLength;
    private int defaultBreakLength;
    private int extendedBreakLength;
    private TextMeshProUGUI timer;
    private bool breakTime;
    private int studySessions;
    private int totalTime;

    private PlaySound ps;

    private void Awake() {
        ps = GameObject.Find("AudioPlayer").GetComponent<PlaySound>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ps.Play(0);
        timer = GetComponent<TextMeshProUGUI>();
        defaultTimerLength = 1500;
        defaultBreakLength = 420;
        extendedBreakLength = 1200;
        breakTime = false;
        studySessions = 0;
        totalTime = 0;
        StartCoroutine(StartTimer(defaultTimerLength));
    }

    /**
     Updates timer based on countdown, oscillates between study sessions and 5 minute breaks.
     Every 4 sessions, take a longer 20 minute break.
    */
    private IEnumerator StartTimer(int seconds) {
        int counter = seconds;
        while (counter > 0) {
            yield return new WaitForSeconds(1);
            if (breakTime) {
                timer.text = "time for a break :)\n" + ConvertTime(counter);
            } else {
                timer.text = ConvertTime(counter);
            }
            counter--;
            totalTime++;
        }
        if (!breakTime) {
            ps.Play(2);
            studySessions++;
            breakTime = true;
            if (studySessions % 4 == 0) {
                StartCoroutine(StartTimer(extendedBreakLength));
            } else {
                StartCoroutine(StartTimer(defaultBreakLength));
            }
        } else {
            ps.Play(1);
            breakTime = false;
            StartCoroutine(StartTimer(defaultTimerLength));
        }
    }

    /**
     Converts time in seconds to readable format.
    */
    private string ConvertTime(int seconds) {
        int mins = seconds / 60;
        int secs = seconds % 60;
        if (secs < 10) {
            return mins + ":0" + secs; 
        }
        return mins + ":" + secs;
    }

    /**
     Pauses and clears the timer at the end of a session.
    */
    public void EndSession() {
        StopAllCoroutines();
        timer.text = "";
    }

    // Get the current timer
    public string GetTime() {
        return ConvertTime(totalTime);
    }

    // Get the number of sessions
    public int GetSessions() {
        return studySessions;
    }

    // Reset the title-screen variable if application exits while in the dungeon
    private void ExitGame() {
        string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerData saved = JsonUtility.FromJson<PlayerData>(json);

        File.Delete(Application.persistentDataPath + "/save.json");
        
        saved.wasInDungeon = false;

        string overwrite = JsonUtility.ToJson(saved);
        File.WriteAllText(Application.persistentDataPath + "/save.json", overwrite);
    }

    // On application quit
    void OnApplicationQuit() {
        ExitGame();
    }

    // Class for managing player save-data
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

}
