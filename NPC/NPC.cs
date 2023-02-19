using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script for managing NPC behaviour along with NPC UI
public class NPC : MonoBehaviour
{
    public int affinity;
    private int maxaffinity;
    private AffinityBanner banner;
    [SerializeField]
    private ItemData fullAffinityGift;
    private bool giftGiven;
    private Inventory inventory;

    [SerializeField]
    private List<Texture> hearts;
    private RawImage heart;

    [SerializeField]
    private List<string> dialogue;
    private GameObject chatbox;
    private TextMeshProUGUI chat;

    [SerializeField]
    private int id;

    private IEnumerator affinityCoroutine;
    private IEnumerator chatCoroutine;

    private void Awake() {
        if (gameObject.name == "Harpist") {
            chatbox = transform.GetChild(0).GetChild(2).gameObject;
            chat = chatbox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            return;
        }
        banner = GameObject.Find("AffinityBanner").GetComponent<AffinityBanner>();
        heart = transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
        chatbox = transform.GetChild(0).GetChild(1).gameObject;
        chat = chatbox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxaffinity = 5;
        affinityCoroutine = null;
        chatCoroutine = null;
    }

    /**
     UI animation for NPC heart display based on affinity. Linger for 3 seconds and fade away.
    */
    private IEnumerator HeartFade() {
        heart.texture = hearts[affinity - 1];
        heart.color = new Color(255, 255, 255, 0f);
        heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1);

        float counter = 3f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            heart.color = new Color(255, 255, 255, heart.color.a + 0.05f);
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, heart.GetComponent<RectTransform>().anchoredPosition.y + 0.005f);
            counter -= 0.1f;
        }

        counter = 5f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            heart.color = new Color(255, 255, 255, heart.color.a - 0.05f);
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, heart.GetComponent<RectTransform>().anchoredPosition.y + 0.005f);
            counter -= 0.1f;
        }
    }

    /**
     UI animation for NPC chat box. Linger for 5 seconds and fade away.
    */
    private IEnumerator ShowChat(string line) {
        chat.text = line;
        chatbox.GetComponent<RawImage>().color = new Color(0.3f, 0.58f, 0.8f, 0.6f);
        chat.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 1f);

        int counter = 5;
        while (counter > 0) {
            yield return new WaitForSeconds(1);
            counter--;
        }

        float counter2 = 5f;
        while (counter2 > 0) {
            yield return new WaitForSeconds(0.1f);
            chatbox.GetComponent<RawImage>().color = new Color(0.3f, 0.58f, 0.8f, chatbox.GetComponent<RawImage>().color.a - 0.05f);
            chat.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, chat.GetComponent<TextMeshProUGUI>().color.a - 0.05f);
            counter2 -= 0.1f;
        }
        chatCoroutine = null;
    }

    // Displays the given chat line
    public void OnClickChat(string line) {
        if (chatCoroutine != null) {
            StopCoroutine(chatCoroutine);
        }
        chatCoroutine = ShowChat(line);
        StartCoroutine(chatCoroutine);
    }

    /**
     Increment NPC affinity - gives player a gift when max affinity is reached.
    */
    public void IncrementAffinity() {
        if (Giftable()) {
            if (affinityCoroutine != null) {
                StopCoroutine(affinityCoroutine);
            }
            if (chatCoroutine != null) {
                StopCoroutine(chatCoroutine);
            }

            if (affinity < maxaffinity) {
                affinity++;
            } else if (!giftGiven) {
                affinity++;
                inventory.Add(fullAffinityGift);
                banner.Display();
                giftGiven = true;
            } else {
                affinityCoroutine = HeartFade();
                StartCoroutine(affinityCoroutine);
                chatCoroutine = ShowChat(":)");
                StartCoroutine(chatCoroutine);
                return;
            }
            affinityCoroutine = HeartFade();
            StartCoroutine(affinityCoroutine);
            chatCoroutine = ShowChat(dialogue[affinity - 1]);
            StartCoroutine(chatCoroutine);
        }
    }

    // Returns true if the NPC has a max-affinity gift.
    public bool Giftable() {
        if (fullAffinityGift != null) {
            return true;
        }
        return false;
    }

    // Loads the saved affinity state of the NPC
    public void LoadNPCState(int savedAffinity) {
        affinity = savedAffinity;
        if (savedAffinity > 4) {
            giftGiven = true;
        } else if (savedAffinity == -1) {
            affinity = 0;
        } else {
            giftGiven = false;
        }
    }

    // Retrieve the ID of the NPC
    public int GetID() {
        return id;
    }
}
