using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script for manipulating UI banner when max-affinity is reached.
public class AffinityBanner : MonoBehaviour
{
    [SerializeField]
    private List<RawImage> banner;

    private PlaySound playSound;

    void Awake() {
        playSound = GameObject.Find("AudioPlayer").GetComponent<PlaySound>();
    }

    private void Start() {
        ResetBanner();
    }

    // Coroutine: Fades in the banner for 3 seconds, fades out in 5 seconds.
    private IEnumerator DisplayBanner() {
        float counter = 3f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            banner[0].color = new Color (0, 0, 0, banner[0].color.a + 0.1f);
            banner[1].color = new Color (255, 255, 255, banner[0].color.a + 0.1f);
            banner[2].color = new Color (255, 255, 255, banner[0].color.a + 0.1f);
            counter -= 0.1f;
        }
        counter = 5f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            banner[0].color = new Color (0, 0, 0, banner[0].color.a - 0.1f);
            banner[1].color = new Color (255, 255, 255, banner[0].color.a - 0.1f);
            banner[2].color = new Color (255, 255, 255, banner[0].color.a - 0.1f);
            counter -= 0.1f;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1000);
    }

    // Display the banner
    public void Display() {
        StopAllCoroutines();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 200);
        playSound.Play(5);
        ResetBanner();
        StartCoroutine(DisplayBanner());
    }

    // Resets banner component colors for re-use
    private void ResetBanner() {
        foreach (RawImage i in banner) {
            i.color = new Color(0, 0, 0, 0);
        }
    }
}
