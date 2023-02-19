using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

// Script for manipulating the main menu UI and its specific functions
public class MenuManager : MonoBehaviour
{
    private RectTransform panel;
    private AudioSource music;
    private TextMeshProUGUI musicText;
    private SingletonLookUp lookup;
    private RectTransform confirmation;
    private StartTutorial tutorial;
    private bool open;

    // Start is called before the first frame update
    void Awake()
    {
        music = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        musicText = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
        confirmation = GameObject.Find("Confirmation").GetComponent<RectTransform>();
        tutorial = GameObject.Find("Tutorial").GetComponent<StartTutorial>();
    }

    void Start() {
        open = false;
        panel = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lookup.GetChest().open) {
            return;
        }
        if (lookup.GetShopManager().shopVisible) {
            return;
        }
        if (lookup.GetPanCamera().inUse) {
            return;
        }
        if (tutorial.open) {
            return;
        }
        
        if (Input.GetKeyDown("escape")) {
            if (!open) {
                OpenMenu();
                lookup.GetPlaySound().Play(7);
                return;
            }
            CloseMenu();
            lookup.GetPlaySound().Play(8);
        }
    }

    // Open the main menu
    private void OpenMenu() {
        lookup.GetPlayer().GetComponent<PlayerMovement>().canMove = false;
        panel.anchoredPosition = new Vector2(0, 0);
        open = true;
    }

    // Close the main menu
    private void CloseMenu() {
        lookup.GetPlayer().GetComponent<PlayerMovement>().canMove = true;
        panel.anchoredPosition = new Vector2(0, 1000);
        DetriggerConfirmation();
        open = false;
    }

    // Toggle music on and off
    public void ToggleMusic() {
        if (!MusicToggled()) {
            music.volume = 1.0f;
            musicText.text = "Music Off";
        } else {
            music.volume = 0f;
            musicText.text = "Music On";
        }
        lookup.GetPlaySound().Play(11);
    }

    // Check if the music is toggled through the menu button
    public bool MusicToggled() {
        if (musicText.text == "Music On") {
            return false;
        }
        return true;
    }

    // Quit the game; saves automatically through Persistence script
    public void SaveAndQuit() {
        Application.Quit();
    }

    // Toggle confirmation UI for reset
    public void TriggerConfirmation() {
        confirmation.anchoredPosition = new Vector2(0, 0);
    }

    // Detoggle confirmation UI for reset
    public void DetriggerConfirmation() {
        confirmation.anchoredPosition = new Vector2(0, 1000);
    }

    // Reset the game by deleting the save file
    public void ResetGame() {
        lookup.GetPlaySound().Play(11);
        File.Delete(Application.persistentDataPath + "/save.json");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
