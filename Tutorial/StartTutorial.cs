using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

// Script to control the tutorial window
public class StartTutorial : MonoBehaviour
{
    [SerializeField]
    private List<TutorialClip> visualComponent;
    
    [SerializeField]
    private List<string> textComponent;

    private VideoPlayer tutorialVideo;
    private RawImage tutorialImage;
    private TextMeshProUGUI tutorialText;
    private TextMeshProUGUI pageNumber;

    private int currentPage;
    private int maxPage;

    public bool open;

    private SingletonLookUp lookup;

    void Awake() {
        lookup = GameObject.Find("SingletonLookUp").GetComponent<SingletonLookUp>();
    }

    void Start() {
        open = false;
        currentPage = 0;
        maxPage = textComponent.Count - 1;
        tutorialVideo = transform.GetChild(0).GetChild(0).GetComponent<VideoPlayer>();
        tutorialImage = transform.GetChild(0).GetChild(1).GetComponent<RawImage>();
        tutorialText = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        pageNumber = transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>();
    }

    // Displays the next page of the tutorial window
    public void DisplayNextPage() {
        if (currentPage == maxPage) {
            // Reward player on tutorial end
            InventoryItem coins = new InventoryItem(lookup.GetItemLookUp().GetItem(0), 5);
            InventoryItem crystal = new InventoryItem(lookup.GetItemLookUp().GetItem(6), 1);
            lookup.GetInventoryManager().Add(coins);
            lookup.GetInventoryManager().Add(crystal);
            CloseTutorial();
            return;
        }
        currentPage++;
        lookup.GetPlaySound().Play(11);
        DisplayPage();
    }

    // Displays the previous page of the tutorial window
    public void DisplayPreviousPage() {
        if (currentPage == 0) {
            return;
        }
        currentPage--;
        lookup.GetPlaySound().Play(11);
        DisplayPage();
    }

    // Sets the UI components of the tutorial page, including image, text, and video content
    private void DisplayPage() {
        tutorialText.text = textComponent[currentPage];

        if (visualComponent[currentPage].image == null) {
            tutorialVideo.gameObject.SetActive(true);
            tutorialImage.gameObject.SetActive(false);
            tutorialVideo.clip = visualComponent[currentPage].clip;
        } else {
            tutorialVideo.gameObject.SetActive(false);
            tutorialImage.gameObject.SetActive(true);
            tutorialImage.texture = visualComponent[currentPage].image;
        }
        DisplayPageNumber();
    }

    // Displays the current page number
    private void DisplayPageNumber() {
        pageNumber.text = (currentPage + 1).ToString() + "/" + (maxPage + 1).ToString();
    }

    // Opens the tutorial window
    public void OpenTutorial() {
        open = true;
        lookup.GetPlayer().GetComponent<PlayerMovement>().canMove = false;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        tutorialVideo.gameObject.SetActive(true);
        tutorialImage.gameObject.SetActive(true);
        DisplayPage();
    }

    // Closes the tutorial window
    public void CloseTutorial() {
        open = false;
        lookup.GetPlayer().GetComponent<PlayerMovement>().canMove = true;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1000);
        tutorialVideo.gameObject.SetActive(false);
        tutorialImage.gameObject.SetActive(false);
    }
}
