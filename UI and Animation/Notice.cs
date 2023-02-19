using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Script for setting, displaying and fading notices.
public class Notice : MonoBehaviour
{
    private TextMeshProUGUI noticeText;
    
    // Start is called before the first frame update
    void Start()
    {
        noticeText = GetComponent<TextMeshProUGUI>();
    }

    // Show a notice on the screen
    public void ShowNotice(string text) {
        StopAllCoroutines();
        noticeText.text = text;
        noticeText.color = new Color(1, 1, 1, 1);
        StartCoroutine(FadeText());
    }

    // Coroutine: Shows a notice for 2 seconds, then fade out over 2 seconds
    private IEnumerator FadeText() {
        float counter = 2f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            counter = counter - 0.1f;
        }

        float time = 2f;
        while (time > 0) {
            yield return new WaitForSeconds(0.1f);
            noticeText.color = new Color(1, 1, 1, noticeText.color.a - 0.06f);
            time = time - 0.1f;
        }
    }
}
