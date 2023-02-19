using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script for fading a button when the player is idle.
public class ButtonFade : MonoBehaviour
{
    private Image button;
    private TextMeshProUGUI text;
    private bool visible;

    // Start is called before the first frame update
    void Start()
    {
        visible = true;
        button = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Detecting mouse movement makes the button visible; otherwise begin fade.
        if (Input.GetAxis("Mouse X") != 0) {
            visible = true;
            StopAllCoroutines();
            button.color = new Color(255, 255, 255, 1);
            text.color = new Color(0, 0, 0, 1);
        } else if (visible) {
            StartCoroutine(Fade());
        }
    }

    /**
     Fades out a UI button after 5 seconds.
    */
    private IEnumerator Fade() {
        visible = false;
        int wait = 5;
        while (wait > 0) {
            yield return new WaitForSeconds(1);
            wait -= 1;
        }

        float counter = 10f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            button.color = new Color(255, 255, 255, button.color.a - 0.1f);
            text.color = new Color(0, 0, 0, text.color.a - 0.1f);
            counter -= 0.1f;
        }
    }

    /**
     Disables a button after it is clicked.
    */
    public void DisableButton() {
        button.GetComponent<Button>().interactable = false;
    }
}
